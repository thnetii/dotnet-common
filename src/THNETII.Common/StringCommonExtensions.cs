using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

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
    }
}
