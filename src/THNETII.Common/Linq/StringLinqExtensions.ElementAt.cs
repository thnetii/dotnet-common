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
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is less than <c>0</c> (zero) or greater than or equal to the number of characters in <paramref name="s"/>.</exception>
        /// <seealso cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/>
        public static char ElementAt(this string s, int index)
        {
            try { return s.ThrowIfNull(nameof(s))[index]; }
            catch (IndexOutOfRangeException indexExcept)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, indexExcept.Message);
            }
        }

        /// <summary>
        /// Returns the character at a specified position in the string or a
        /// default value if the specified index is not valid in the string.
        /// </summary>
        /// <param name="s">The string to return a character from.</param>
        /// <param name="index">The zero-based index of the character to retrieve.</param>
        /// <param name="default">The value to return if <paramref name="index"/> is outside of the valid range of indicies for <paramref name="s"/>.</param>
        /// <returns>The character at position <paramref name="index"/> in <paramref name="s"/>; or <paramref name="default"/> if <paramref name="index"/> is outside the valid range of indicies in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
        public static char ElementAtOrDefault(this string s, int index, char @default = default) =>
            s.ElementAtOrDefault(index, () => @default);

        /// <summary>
        /// Returns the character at a specified position in the string or 
        /// produces a value if the specified index is not valid in the string.
        /// </summary>
        /// <param name="s">The string to return a character from.</param>
        /// <param name="index">The zero-based index of the character to retrieve.</param>
        /// <param name="defaultFactory">The function to produce the value to return if <paramref name="index"/> is outside of the valid range of indicies for <paramref name="s"/>.</param>
        /// <returns>The character at position <paramref name="index"/> in <paramref name="s"/>; or the return value of <paramref name="defaultFactory"/> if <paramref name="index"/> is outside the valid range of indicies in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/> or <paramref name="s"/> is empty and <paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        /// <seealso cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
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
