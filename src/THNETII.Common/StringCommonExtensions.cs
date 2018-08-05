using System;

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
        /// true if the value parameter occurs within this string, or if value is the empty
        /// string (<c>""</c>); otherwise, false.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="s"/> or <paramref name="value"/> is <c>null</c>.
        /// </exception>
        /// <seealso cref="string.Contains(string)"/>
        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (value == null)
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
    }
}
