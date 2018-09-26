using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq
{
    public static partial class StringLinqExtensions
    {
        /// <summary>
        /// Returns the last character (i.e. the character at index <c>length - 1</c>) in the string.
        /// </summary>
        /// <param name="s">The string to return the last character of.</param>
        /// <returns>The last character in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        /// <seealso cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/>
        public static char Last(this string s)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            return s[s.Length - 1];
        }

        /// <summary>
        /// Returns the last character (i.e. the character at position <c>length - 1</c>) in the string
        /// or a default value if the string is empty.
        /// </summary>
        /// <param name="s">The string to return the last character of.</param>
        /// <param name="default">The value to return if <paramref name="s"/> is empty.</param>
        /// <returns>The last character in <paramref name="s"/>; or <paramref name="default"/> if <paramref name="s"/> is empty.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        /// <seealso cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})"/>
        public static char LastOrDefault(this string s, char @default = default) =>
            s.LastOrDefault(() => @default);

        public static char LastOrDefault(this string s, Func<char> defaultFactory)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (s.Length < 1)
                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return s[s.Length - 1];
        }
    }
}
