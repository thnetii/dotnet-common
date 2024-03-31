using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq;

public static partial class StringLinqExtensions
{
    /// <summary>
    /// Returns the last character (i.e. the character at index <c>length - 1</c>) in the string.
    /// </summary>
    /// <param name="s">The string to return the last character of.</param>
    /// <returns>The last character in <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="s"/> is empty.</exception>
    /// <seealso cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/>
    public static char Last(this string s)
    {
        if (s is null)
            throw new ArgumentNullException(nameof(s));
        else if (s.Length < 1)
            throw new InvalidOperationException("The string is empty.");
        return s[s.Length - 1];
    }

    /// <summary>
    /// Returns the last character (i.e. the character at position <c>length - 1</c>) in the string
    /// or a default value if the string is empty.
    /// </summary>
    /// <param name="s">The string to return the last character of.</param>
    /// <param name="default">The value to return if <paramref name="s"/> is empty.</param>
    /// <returns>The last character in <paramref name="s"/>; or <paramref name="default"/> if <paramref name="s"/> is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.</exception>
    /// <seealso cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})"/>
    public static char LastOrDefault(this string s, char @default = default) =>
        s.LastOrDefault(() => @default);

    /// <summary>
    /// Returns the last character (i.e. the character at position <c>length - 1</c>) in the string
    /// or creates a value if the string is empty.
    /// </summary>
    /// <param name="s">The string to return the last character of.</param>
    /// <param name="defaultFactory">The function to produce the return value if <paramref name="s"/> is empty.</param>
    /// <returns>The last character in <paramref name="s"/>; or the return value of <paramref name="defaultFactory"/> if <paramref name="s"/> is empty.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="s"/> is <see langword="null"/>.<br/><em>-or-</em><br/><paramref name="s"/> is empty and <paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
    /// <seealso cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})"/>
    public static char LastOrDefault(this string s, Func<char> defaultFactory)
    {
        if (s is null)
            throw new ArgumentNullException(nameof(s));
        if (s.Length < 1)
            return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
        return s[s.Length - 1];
    }
}
