using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq
{
    public static partial class StringLinqExtensions
    {
        /// <summary>
        /// Returns the character at a specified position in the string.
        /// </summary>
        /// <param name="s">The string to return a character from.</param>
        /// <param name="index">The zero-based index of the character to retrieve.</param>
        /// <returns>The character at the specified position in the string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        /// <seealso cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/>
        public static char ElementAt(this string s, int index) =>
            s.ThrowIfNull(nameof(s))[index];

        /// <summary>
        /// Returns the character at a specified position in the string or a
        /// default value if the specified index is not valid in the string.
        /// </summary>
        /// <param name="s">The string to return a character from.</param>
        /// <param name="index">The zero-based index of the character to retrieve.</param>
        /// <param name="default">The value to return if <paramref name="index"/> is outside of the valid range of indicies for <paramref name="s"/>.</param>
        /// <returns>The character at position <paramref name="index"/> in <paramref name="s"/>; or <paramref name="default"/> if <paramref name="index"/> is outside the valid range of indicies in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        /// <seealso cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
        public static char ElementAtOrDefault(this string s, int index, char @default = default) =>
            s.ElementAtOrDefault(index, () => @default);

        public static char ElementAtOrDefault(this string s, int index, Func<char> defaultFactory)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (index < 0 || index >= s.Length)
                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return s[index];
        }
    }
}
