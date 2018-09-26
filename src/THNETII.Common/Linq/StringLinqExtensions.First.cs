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
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        /// <exception cref="IndexOutOfRangeException"><paramref name="s"/> is empty.</exception>
        /// <seealso cref="Enumerable.First{TSource}(IEnumerable{TSource})"/>
        public static char First(this string s) => s.ElementAt(0);

        /// <summary>
        /// Returns the first character (i.e. the 0th character) in the string
        /// or a default value if the string is empty.
        /// </summary>
        /// <param name="s">The string to return the first character of.</param>
        /// <param name="default">The value to return if <paramref name="s"/> is empty.</param>
        /// <returns>The first character in <paramref name="s"/>; or <paramref name="default"/> if <paramref name="s"/> is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        public static char FirstOrDefault(this string s, char @default = default) =>
            s.FirstOrDefault(() => @default);

        public static char FirstOrDefault(this string s, Func<char> defaultFactory) =>
            s.ElementAtOrDefault(0, defaultFactory);
    }
}
