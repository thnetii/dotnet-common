using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace THNETII.Common
{
    /// <summary>
    /// Provides common extension methods for .NET types.
    /// </summary>
    [DebuggerStepThrough]
    public static class CommonExtensions
    {
        /// <summary>
        /// Guards a value or an instance against being <c>null</c>, returning an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type to return. <typeparamref name="T"/> should be a type where checks against <c>null</c> are sensible.</typeparam>
        /// <param name="x">The value to check against <c>null</c>.</param>
        /// <param name="otherwise">The value to return in case <paramref name="x"/> is <c>null</c>.</param>
        /// <returns>The value of <paramref name="x"/> if <paramref name="x"/> is not <c>null</c>; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        public static T NotNull<T>(this T x, T otherwise)
            where T : class => !(x is null) ? x : otherwise;

        /// <summary>
        /// Checks whether the specified value is not <c>null</c> and returns it through the out-parameter.
        /// </summary>
        /// <typeparam name="T">The type to return. <typeparamref name="T"/> should be a type where checks against <c>null</c> are sensible.</typeparam>
        /// <param name="x">The value to check against <c>null</c>.</param>
        /// <param name="value">Always returns back the value of <paramref name="x"/>.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is not <c>null</c>; otherwise, <c>false</c>.</returns>
        public static bool TryNotNull<T>(this T x, out T value) where T : class
        {
            value = x;
            return !(value is null);
        }

        /// <summary>
        /// Guards a value or an instance against being <c>null</c>, creating an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type to return. <typeparamref name="T"/> should be a type where checks against <c>null</c> are sensible.</typeparam>
        /// <param name="x">The value to check against <c>null</c>.</param>
        /// <param name="otherwiseFactory">The function to invoke in case <paramref name="x"/> is <c>null</c>.</param>
        /// <returns>The value of <paramref name="x"/> if <paramref name="x"/> is not <c>null</c>; otherwise, the value returned from <paramref name="otherwiseFactory"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="otherwiseFactory"/> is <c>null</c>.</exception>
        public static T NotNull<T>(this T x, Func<T> otherwiseFactory)
             where T : class
        {
            if (!(x is null))
                return x;
            else if (otherwiseFactory is null)
                throw new ArgumentNullException(nameof(otherwiseFactory));
            return otherwiseFactory();
        }

        /// <summary>
        /// Guards a string against being <c>null</c> or empty, returning an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <c>null</c> or the empty string.</param>
        /// <param name="otherwise">The value to return in case <paramref name="s"/> is <c>null</c> or empty.</param>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <c>null</c> nor the empty string; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        public static string NotNullOrEmpty(this string s, string otherwise) => string.IsNullOrEmpty(s) ? otherwise : s;

        /// <summary>
        /// Checks whether the specified string is neither <c>null</c> nor empty and returns it through the out-parameter.
        /// </summary>
        /// <param name="s">The string to check against <c>null</c> or the empty string.</param>
        /// <param name="value">Always returns back the value of <paramref name="s"/>.</param>
        /// <returns><c>true</c> if <paramref name="s"/> is neither <c>null</c> nor the empty string; otherwise, <c>false</c>.</returns>
        public static bool TryNotNullOrEmpty(this string s, out string value)
        {
            value = s;
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Guards a value or an instance against being <c>null</c>, creating an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <c>null</c> or the empty string.</param>
        /// <param name="otherwiseFactory">The function to invoke in case <paramref name="s"/> is <c>null</c> or empty.</param>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <c>null</c> nor the empty string; otherwise, the value returned from <paramref name="otherwiseFactory"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="otherwiseFactory"/> is <c>null</c>.</exception>
        public static string NotNullOrEmpty(this string s, Func<string> otherwiseFactory)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (otherwiseFactory is null)
                    throw new ArgumentNullException(nameof(otherwiseFactory));
                return otherwiseFactory();
            }
            return s;
        }

        /// <summary>
        /// Guards a string against being <c>null</c>, empty or whitespace-only, returning an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <c>null</c> or whitespace.</param>
        /// <param name="otherwise">The value to return in case <paramref name="s"/> is <c>null</c>, empty or whitespace-only.</param>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <c>null</c>, the empty string nor whitespace-only; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        public static string NotNullOrWhiteSpace(this string s, string otherwise) => string.IsNullOrWhiteSpace(s) ? otherwise : s;

        /// <summary>
        /// Checks whether the specified string is neither <c>null</c>, empty nor white-space only and returns it through the out-parameter.
        /// </summary>
        /// <param name="s">The string to check against <c>null</c>, the empty string and white-space only.</param>
        /// <param name="value">Always returns back the value of <paramref name="s"/>.</param>
        /// <returns><c>true</c> if <paramref name="s"/> is neither <c>null</c>, the empty string nor white-space only; otherwise, <c>false</c>.</returns>
        public static bool TryNotNullOrWhiteSpace(this string s, out string value)
        {
            value = s;
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Guards a string against being <c>null</c>, empty or whitespace-only, creating an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <c>null</c> or whitespace.</param>
        /// <param name="otherwiseFactory">The function to invoke in case <paramref name="s"/> is <c>null</c>, empty or whitespace-only. Must not be <c>null</c></param>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <c>null</c>, the empty string nor whitespace-only; otherwise, the value returned from <paramref name="otherwiseFactory"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="otherwiseFactory"/> is <c>null</c>.</exception>
        public static string NotNullOrWhiteSpace(this string s, Func<string> otherwiseFactory)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                if (otherwiseFactory is null)
                    throw new ArgumentNullException(nameof(otherwiseFactory));
                return otherwiseFactory();
            }
            return s;
        }

        /// <summary>
        /// Returns the specified array instance or a sero-length array of the same type if the specified arrays is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The array to check against <c>null</c>.</param>
        /// <returns>
        /// Returns <paramref name="array"/> if it is non-<c>null</c>; otherwise, a zero-length <typeparamref name="T"/>-array is returned.
        /// The return value of this method is guaranteed to be non-<c>null</c>.
        /// </returns>
        public static T[] ZeroLengthIfNull<T>(this T[] array) => array ?? Array.Empty<T>();

        /// <summary>
        /// Returns the specified <see cref="IEnumerable{T}"/> instance, or an empty <see cref="IEnumerable{T}"/> if the specified enumerable is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The type that is being enumerated by <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The enumerable to check against <c>null</c>.</param>
        /// <returns>
        /// Returns <paramref name="enumerable"/> if it is non-<c>null</c>; otherwise, an empty <see cref="IEnumerable{T}"/> is returned.
        /// The return value of this method is guaranteed to be non-<c>null</c>.
        /// </returns>
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> enumerable) => enumerable ?? Enumerable.Empty<T>();
    }
}
