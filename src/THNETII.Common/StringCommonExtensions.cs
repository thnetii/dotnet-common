using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using THNETII.Common.Text;

namespace THNETII.Common
{
    /// <summary>
    /// Provides common extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringCommonExtensions
    {
        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this string
        /// when compared using the specified comparison option.
        /// </summary>
        /// <param name="s">The source string</param>
        /// <param name="value">The string to seek.</param>
        /// <param name="comparisonType">One of the enumeration values that determines how the contents of <paramref name="s"/> and <paramref name="value"/> are compared.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="value"/> parameter occurs within this string, or if <paramref name="value"/> is the empty
        /// string (<c>""</c>); otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="s"/> or <paramref name="value"/> is <c>null</c>.
        /// </exception>
        /// <seealso cref="string.Contains(string)"/>
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (value is null)
                throw new ArgumentNullException(nameof(value));
            var v = value.AsSpan();
            for (var remaining = s.AsSpan(); remaining.Length >= value.Length;
                remaining = remaining.Slice(start: 1))
            {
                if (remaining.StartsWith(v, comparisonType))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Enumerates individual lines from a multi-line string.
        /// </summary>
        /// <param name="s">The string to read from.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> that enumerates the individual lines in <paramref name="s"/>.</returns>
        /// <remarks>
        /// <para>Each enumerated line is guaranteed to be non-null.</para>
        /// <para>The enumerated lines do not contain the terminating carriage return or line feed characters.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        /// <seealso cref="StringReader.ReadLine"/>
        public static IEnumerable<string> EnumerateLines(this string s)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            return s.YieldLines();
        }

        private static IEnumerable<string> YieldLines(this string s)
        {
            using (var reader = new StringReader(s))
            {
                for (string line = reader.ReadLine(); !(line is null); line = reader.ReadLine())
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// Performs a multi-replace operation where every occurrence of each
        /// of the specified strings are replaced with their respective
        /// replacement strings.
        /// </summary>
        /// <param name="s">The string to perform the replacements on.</param>
        /// <param name="mappings">
        /// The pairs of string values, where the first value in each pair
        /// specifies the sub-string <paramref name="s"/> to search for, and the
        /// second string specifies the value with which each occurrence should
        /// be replaced.
        /// </param>
        /// <returns>
        /// <paramref name="s"/> if <paramref name="mappings"/> is empty, or
        /// no occurrence of the specified substrings was found; otherwise,
        /// a new string in which all occurrences of the first value in <paramref name="mappings"/>
        /// is replaced with the second value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The <see cref="Replace"/> method is suited to replace operations
        /// where there a fixed set of replacements to made, such as escaping,
        /// or un-escaping operations.
        /// </para>
        /// <note>
        /// The order of the replacements specified by <paramref name="mappings"/>
        /// is relevant. Earlier replacements are preferred over later ones.
        /// </note>
        /// <para>
        /// This method is optimized for linear performance and memory allocation
        /// overhead. Thus, calling this method is more efficient than successively
        /// calling <see cref="string.Replace(string, string)"/>.
        /// </para>
        /// <para>
        /// If the resulting string has a length that is less than or equal to
        /// the length of <paramref name="s"/>, only a single memory allocation
        /// is required to perform all replacements.
        /// </para>
        /// </remarks>
        /// <seealso cref="string.Replace(string, string)"/>
        /// <seealso href="https://nim-lang.org/docs/strutils.html#multiReplace%2Cstring%2Cvarargs%5B%5D"/>
        [SuppressMessage("Usage", "PC001: API not supported on all platforms")]
        public static unsafe string Replace(this string s,
            IEnumerable<(string oldValue, string newValue)> mappings)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (mappings is null)
                throw new ArgumentNullException(nameof(mappings));
            var mappingList = mappings as IList<(string oldValue, string newValue)>
                ?? mappings.ToList();
            if (mappingList.Count < 1)
                return s;
            Span<char> first = stackalloc char[mappingList.Count];
            int i, j;
            for (i = 0, j = 0; i < mappingList.Count; i++)
            {
                var (oldValue, newValue) = mappingList[i];
                if (oldValue is null)
                    throw new ArgumentNullException(FormattableString.Invariant($"{nameof(mappings)}[{i}].{nameof(oldValue)}"));
                if (newValue is null)
                    throw new ArgumentNullException(FormattableString.Invariant($"{nameof(mappings)}[{i}].{nameof(newValue)}"));
                if (oldValue.Length < 1)
                    continue;
                first[j] = oldValue[0];
                j++;
            }
            if (j < first.Length)
                first = first.Slice(start: 0, length: j);
            if (first.IsEmpty)
                return s;
            ReadOnlySpan<char> span = s.AsSpan();
            StringBuilder builder = new StringBuilder(s.Length);
            int nextIdx, replacements = 0;
            for (nextIdx = span.IndexOfAny(first); nextIdx >= 0; nextIdx = span.IndexOfAny(first))
            {
                builder.Append(span.Slice(start: 0, length: nextIdx));
                span = span.Slice(nextIdx);
                bool replaced = false;
                foreach (var (token, replacement) in mappingList)
                {
                    if (token.Length < 1)
                        continue;
                    if (span.StartsWith(token.AsSpan()))
                    {
                        builder.Append(replacement);
                        span = span.Slice(token.Length);
                        replacements++;
                        replaced = true;
                        break;
                    }
                }
                if (!replaced)
                    span = span.Slice(start: 1);
            }
            if (replacements == 0)
                return s;
            return builder.ToString();
        }
    }
}
