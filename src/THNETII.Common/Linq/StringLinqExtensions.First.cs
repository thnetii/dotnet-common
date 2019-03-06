using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq
{
    public static partial class StringLinqExtensions
    {
        /// <summary>
        /// Returns the first character (i.e. the 0th character) in the string.
        /// </summary>
        /// <param name="s">The string to return the first character of.</param>
        /// <returns>The first character in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="s"/> is empty.</exception>
        /// <seealso cref="Enumerable.First{TSource}(IEnumerable{TSource})"/>
        public static char First(this string s)
        {
            try { return s.ElementAt(0); }
            catch (ArgumentOutOfRangeException argExcept)
            {
                throw new InvalidOperationException("The string is empty.", argExcept);
            }
        }

        /// <summary>
        /// Returns the first character (i.e. the 0th character) in the string
        /// or a default value if the string is empty.
        /// </summary>
        /// <param name="s">The string to return the first character of.</param>
        /// <param name="default">The value to return if <paramref name="s"/> is empty.</param>
        /// <returns>The first character in <paramref name="s"/>; or <paramref name="default"/> if <paramref name="s"/> is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
        public static char FirstOrDefault(this string s, char @default = default) =>
            s.FirstOrDefault(() => @default);

        /// <summary>
        /// Returns the first character (i.e. the 0th character) in the string
        /// or a creates a value if the string is empty.
        /// </summary>
        /// <param name="s">The string to return the first character of.</param>
        /// <param name="defaultFactory">The function that produces the value to return if <paramref name="s"/> is empty.</param>
        /// <returns>The first character in <paramref name="s"/>; or the return value of <paramref name="defaultFactory"/> if <paramref name="s"/> is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/> or <paramref name="s"/> is empty and <paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        public static char FirstOrDefault(this string s, Func<char> defaultFactory) =>
            s.ElementAtOrDefault(0, defaultFactory);
    }
}
