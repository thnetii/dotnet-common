using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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
        /// Guards an instance against being <see langword="null"/>, returning an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type to return. <typeparamref name="T"/> should be a type where checks against <see langword="null"/> are sensible.</typeparam>
        /// <param name="x">The value to check against <see langword="null"/>.</param>
        /// <param name="otherwise">The value to return in case <paramref name="x"/> is <see langword="null"/>.</param>
        /// <returns>The value of <paramref name="x"/> if <paramref name="x"/> is not <see langword="null"/>; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        [return: NotNullIfNotNull("x")]
        [return: NotNullIfNotNull("otherwise")]
        public static T NotNull<T>(this T x, T otherwise)
            where T : class? => !(x is null) ? x : otherwise;

        /// <summary>
        /// Guards an instance against being <see langword="null"/>, creating an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type to return. <typeparamref name="T"/> should be a type where checks against <see langword="null"/> are sensible.</typeparam>
        /// <param name="x">The value to check against <see langword="null"/>.</param>
        /// <param name="otherwiseFactory">The function to invoke in case <paramref name="x"/> is <see langword="null"/>.</param>
        /// <returns>The value of <paramref name="x"/> if <paramref name="x"/> is not <see langword="null"/>; otherwise, the value returned from <paramref name="otherwiseFactory"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="otherwiseFactory"/> is <see langword="null"/>.</exception>
        [return: NotNull]
        public static T NotNull<T>(this T x, Func<T> otherwiseFactory)
             where T : class?
        {
            if (!(x is null))
                return x;
            else if (otherwiseFactory is null)
                throw new ArgumentNullException(nameof(otherwiseFactory));
            return otherwiseFactory();
        }

        /// <summary>
        /// Guards a string against being <see langword="null"/> or empty, returning an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <see langword="null"/> or the empty string.</param>
        /// <param name="otherwise">The value to return in case <paramref name="s"/> is <see langword="null"/> or empty.</param>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <see langword="null"/> nor the empty string; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        [return: NotNullIfNotNull("otherwise")]
        public static string? NotNullOrEmpty(this string? s, string? otherwise) =>
            string.IsNullOrEmpty(s) ? otherwise : s;

        /// <summary>
        /// Guards a string against being <see langword="null"/> or the empty string, creating an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <see langword="null"/> or the empty string.</param>
        /// <param name="otherwiseFactory">The function to invoke in case <paramref name="s"/> is <see langword="null"/> or empty.</param>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <see langword="null"/> nor the empty string; otherwise, the value returned from <paramref name="otherwiseFactory"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="otherwiseFactory"/> is <see langword="null"/>.</exception>
        public static string NotNullOrEmpty(this string? s, Func<string> otherwiseFactory)
        {
            if (string.IsNullOrEmpty(s))
            {
                if (otherwiseFactory is null)
                    throw new ArgumentNullException(nameof(otherwiseFactory));
                return otherwiseFactory();
            }
            return s!;
        }

        /// <summary>
        /// Guards an array against being <see langword="null"/> or empty, returning an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="otherwise">The value to return in case <paramref name="array"/> is <see langword="null"/> or empty.</param>
        /// <returns>
        /// <paramref name="otherwise"/> if <paramref name="array"/> is <see langword="null"/> or empty;
        /// otherwise, <paramref name="array"/>.
        /// </returns>
        [return: NotNullIfNotNull("otherwise")]
        public static T[]? NotNullOrEmpty<T>(this T[]? array, T[]? otherwise)
        {
            switch (array)
            {
                case null:
                case var empty when empty.Length < 1:
                    return otherwise;
                default:
                    return array;
            }
        }

        /// <summary>
        /// Guards an array against being <see langword="null"/> or empty, creating an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="otherwiseFactory">The function to produce the value to return in case <paramref name="array"/> is <see langword="null"/> or empty.</param>
        /// <returns>
        /// The return value of <paramref name="otherwiseFactory"/> if <paramref name="array"/> is <see langword="null"/> or empty;
        /// otherwise, <paramref name="array"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is empty and <paramref name="otherwiseFactory"/> is <see langword="null"/>.</exception>
        public static T[] NotNullOrEmpty<T>(this T[]? array, Func<T[]> otherwiseFactory)
        {
            switch (array)
            {
                case null:
                case var empty when empty.Length < 1:
                    if (otherwiseFactory is null)
                        throw new ArgumentNullException(nameof(otherwiseFactory));
                    return otherwiseFactory();
                default:
                    return array;
            }
        }

        /// <summary>
        /// Guards an enumerable against being <see langword="null"/> or empty, returning an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <param name="otherwise">The value to return in case <paramref name="enumerable"/> is <see langword="null"/> or empty.</param>
        /// <returns>
        /// <paramref name="otherwise"/> if <paramref name="enumerable"/> is <see langword="null"/> or empty;
        /// otherwise, a wrapper around <paramref name="enumerable"/> enumerating the same elements.
        /// </returns>
        /// <remarks>
        /// In order to ensure that <paramref name="enumerable"/> is not empty, the enumerable has to be expanded by checking
        /// if the first element is availble. To prevent double evaluation of the enumerable, the created enumerator is
        /// preserved and wrapped in the new enumerable that is returned.
        /// </remarks>
        [return: NotNullIfNotNull("otherwise")]
        public static IEnumerable<T>? NotNullOrEmpty<T>(this IEnumerable<T>? enumerable, IEnumerable<T>? otherwise)
        {
            switch (enumerable)
            {
                case null:
                case T[] { Length: 0 }:
                case ICollection<T> c when c.Count == 0:
                case string { Length: 0 }:
                    return otherwise;

                case T[] _:
                case ICollection<T> _:
                case string _:
                    break;

                default:
                case IEnumerable<T> e:
                    var enumerator = e.GetEnumerator();
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        return otherwise;
                    }
                    static IEnumerable<T> wrapAroundEnumerable(IEnumerator<T> enumerator)
                    {
                        using (enumerator)
                        {
                            do
                            {
                                yield return enumerator.Current;
                            } while (enumerator.MoveNext());
                        }
                    }
                    return wrapAroundEnumerable(enumerator);
            }
            return enumerable;
        }

        /// <summary>
        /// Guards an enumerable against being <see langword="null"/> or empty, creating an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <param name="otherwiseFactory">The function to produce the value to return in case <paramref name="enumerable"/> is <see langword="null"/> or empty.</param>
        /// <returns>
        /// The return value of <paramref name="otherwiseFactory"/> if <paramref name="enumerable"/> is <see langword="null"/> or empty;
        /// otherwise, a wrapper around <paramref name="enumerable"/> enumerating the same elements.
        /// </returns>
        /// <remarks>
        /// In order to ensure that <paramref name="enumerable"/> is not empty, the enumerable has to be expanded by checking
        /// if the first element is availble. To prevent double evaluation of the enumerable, the created enumerator is
        /// preserved and wrapped in the new enumerable that is returned.
        /// </remarks>
        public static IEnumerable<T> NotNullOrEmpty<T>(this IEnumerable<T>? enumerable, Func<IEnumerable<T>> otherwiseFactory)
        {
            switch (enumerable)
            {
                case null:
                case T[] { Length: 0 }:
                case ICollection<T> collection when collection.Count == 0:
                case string { Length: 0 }:
                    break;

                case T[] _:
                case ICollection<T> _:
                case string _:
                    return enumerable;

                default:
                case IEnumerable<T> e:
                    var enumerator = e.GetEnumerator();
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        break;
                    }
                    static IEnumerable<T> wrapAroundEnumerable(IEnumerator<T> enumerator)
                    {
                        using (enumerator)
                        {
                            do
                            {
                                yield return enumerator.Current;
                            } while (enumerator.MoveNext());
                        }
                    }
                    return wrapAroundEnumerable(enumerator);
            }
            if (otherwiseFactory is null)
                throw new ArgumentNullException(nameof(otherwiseFactory));
            return otherwiseFactory();
        }

        /// <summary>
        /// Guards a string against being <see langword="null"/>, empty or whitespace-only, returning an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <see langword="null"/> or whitespace.</param>
        /// <param name="otherwise">The value to return in case <paramref name="s"/> is <see langword="null"/>, empty or whitespace-only.</param>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <see langword="null"/>, the empty string nor whitespace-only; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        [return: NotNullIfNotNull("otherwise")]
        public static string? NotNullOrWhiteSpace(this string? s, string? otherwise) =>
            string.IsNullOrWhiteSpace(s) ? otherwise : s;

        /// <summary>
        /// Guards a string against being <see langword="null"/>, empty or whitespace-only, creating an alternative default value if the check fails.
        /// </summary>
        /// <param name="s">The string to check against <see langword="null"/> or whitespace.</param>
        /// <param name="otherwiseFactory">The function to invoke in case <paramref name="s"/> is <see langword="null"/>, empty or whitespace-only. Must not be <see langword="null"/></param>
        /// <returns>The value of <paramref name="s"/> if <paramref name="s"/> is neither <see langword="null"/>, the empty string nor whitespace-only; otherwise, the value returned from <paramref name="otherwiseFactory"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="otherwiseFactory"/> is <see langword="null"/>.</exception>
        public static string NotNullOrWhiteSpace(this string? s, Func<string> otherwiseFactory)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                if (otherwiseFactory is null)
                    throw new ArgumentNullException(nameof(otherwiseFactory));
                return otherwiseFactory();
            }
            return s!;
        }

        /// <summary>
        /// Checks whether the specified instance is not <see langword="null"/> and returns it through the out-parameter.
        /// </summary>
        /// <typeparam name="T">The type to return. <typeparamref name="T"/> should be a type where checks against <see langword="null"/> are sensible.</typeparam>
        /// <param name="x">The value to check against <see langword="null"/>.</param>
        /// <param name="value">Always returns back the value of <paramref name="x"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> is not <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryNotNull<T>(
            [NotNullWhen(returnValue: true)] this T x,
            [NotNullWhen(returnValue: true)] out T value) where T : class?
        {
            value = x;
            return !(value is null);
        }

        /// <summary>
        /// Checks whether the specified string is neither <see langword="null"/> nor empty and returns it through the out-parameter.
        /// </summary>
        /// <param name="s">The string to check against <see langword="null"/> or the empty string.</param>
        /// <param name="value">Always returns back the value of <paramref name="s"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> is neither <see langword="null"/> nor the empty string; otherwise, <see langword="false"/>.</returns>
        public static bool TryNotNullOrEmpty(
            [NotNullWhen(returnValue: true)] this string? s,
            [NotNullWhen(returnValue: true)] out string? value)
        {
            value = s;
            return !string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Checks whether the specified array is neither <see langword="null"/> nor empty and returns it through the out-parameter.
        /// </summary>
        /// <param name="array">The array to check against <see langword="null"/> or the empty array.</param>
        /// <param name="value">Always returns back the value of <paramref name="array"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="array"/> is neither <see langword="null"/> nor empty; otherwise, <see langword="false"/>.</returns>
        public static bool TryNotNullOrEmpty<T>(
            [NotNullWhen(returnValue: true)] this T[]? array,
            [NotNullWhen(returnValue: true)] out T[]? value)
        {
            value = array;
            return !(array is null) && array.Length >= 1;
        }

        /// <summary>
        /// Checks whether the specified enumerable is neither <see langword="null"/> nor empty and returns it through the out-parameter.
        /// </summary>
        /// <param name="enumerable">The enumerable to check against <see langword="null"/> or the empty array.</param>
        /// <param name="value">Receives a wrapper around <paramref name="enumerable"/> enumerating the same items.</param>
        /// <returns><see langword="true"/> if <paramref name="enumerable"/> is neither <see langword="null"/> nor empty; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// In order to ensure that <paramref name="enumerable"/> is not empty, the enumerable has to be expanded by checking
        /// if the first element is availble. To prevent double evaluation of the enumerable, the created enumerator is
        /// preserved and wrapped in the new enumerable that is returned.
        /// </remarks>
        public static bool TryNotNullOrEmpty<T>(
            [NotNullWhen(returnValue: true)] this IEnumerable<T>? enumerable,
            [NotNullWhen(returnValue: true)] out IEnumerable<T>? value)
        {
            value = enumerable;
            switch (enumerable)
            {
                case null:
                case T[] { Length: 0 }:
                case ICollection<T> c when c.Count == 0:
                case string { Length: 0 }:
                    return false;

                case T[] _:
                case ICollection<T> _:
                case string _:
                    return true;

                default:
                case IEnumerable<T> e:
                    var enumerator = e.GetEnumerator();
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        return false;
                    }
                    static IEnumerable<T> wrapAroundEnumerable(
                        IEnumerator<T> enumerator)
                    {
                        using (enumerator)
                        {
                            do
                            {
                                yield return enumerator.Current;
                            } while (enumerator.MoveNext());
                        }
                    }
                    value = wrapAroundEnumerable(enumerator);
                    return true;
            }
        }

        /// <summary>
        /// Checks whether the specified string is neither <see langword="null"/>, empty nor white-space only and returns it through the out-parameter.
        /// </summary>
        /// <param name="s">The string to check against <see langword="null"/>, the empty string and white-space only.</param>
        /// <param name="value">Always returns back the value of <paramref name="s"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="s"/> is neither <see langword="null"/>, the empty string nor white-space only; otherwise, <see langword="false"/>.</returns>
        public static bool TryNotNullOrWhiteSpace(
            [NotNullWhen(returnValue: true)] this string? s,
            [NotNullWhen(returnValue: true)] out string? value)
        {
            value = s;
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Returns the specified array instance or a sero-length array of the same type if the specified arrays is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The array to check against <see langword="null"/>.</param>
        /// <returns>
        /// Returns <paramref name="array"/> if it is non-<see langword="null"/>; otherwise, a zero-length <typeparamref name="T"/>-array is returned.
        /// The return value of this method is guaranteed to be non-<see langword="null"/>.
        /// </returns>
        public static T[] ZeroLengthIfNull<T>(this T[]? array) =>
            array ?? Array.Empty<T>();

        /// <summary>
        /// Returns the specified <see cref="IEnumerable{T}"/> instance, or an empty <see cref="IEnumerable{T}"/> if the specified enumerable is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The type that is being enumerated by <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The enumerable to check against <see langword="null"/>.</param>
        /// <returns>
        /// Returns <paramref name="enumerable"/> if it is non-<see langword="null"/>; otherwise, an empty <see cref="IEnumerable{T}"/> is returned.
        /// The return value of this method is guaranteed to be non-<see langword="null"/>.
        /// </returns>
        public static IEnumerable<T> EmptyIfNull<T>(
            this IEnumerable<T>? enumerable) =>
            enumerable ?? Enumerable.Empty<T>();
    }
}
