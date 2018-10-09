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
        /// Guards an instance against being <c>null</c>, returning an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type to return. <typeparamref name="T"/> should be a type where checks against <c>null</c> are sensible.</typeparam>
        /// <param name="x">The value to check against <c>null</c>.</param>
        /// <param name="otherwise">The value to return in case <paramref name="x"/> is <c>null</c>.</param>
        /// <returns>The value of <paramref name="x"/> if <paramref name="x"/> is not <c>null</c>; otherwise, the value of <paramref name="otherwise"/> is returned.</returns>
        /// <remarks>This extension method is a convenience method that allows for functional-style chaining invocations instead of having to write an equivalent <c>if</c>-statement.</remarks>
        public static T NotNull<T>(this T x, T otherwise)
            where T : class => !(x is null) ? x : otherwise;

        /// <summary>
        /// Guards an instance against being <c>null</c>, creating an alternative default value if the check fails.
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
        /// Guards a string against being <c>null</c> or the empty string, creating an alternative default value if the check fails.
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
        /// Guards an array against being <c>null</c> or empty, returning an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="otherwise">The value to return in case <paramref name="array"/> is <c>null</c> or empty.</param>
        /// <returns>
        /// <paramref name="otherwise"/> if <paramref name="array"/> is <c>null</c> or empty;
        /// otherwise, <paramref name="array"/>.
        /// </returns>
        public static T[] NotNullOrEmpty<T>(this T[] array, T[] otherwise)
        {
            switch (array)
            {
                case null:
                case var _ when array.Length < 1:
                    return otherwise;
                default:
                    return array;
            }
        }

        /// <summary>
        /// Guards an array against being <c>null</c> or empty, creating an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="otherwiseFactory">The function to produce the value to return in case <paramref name="array"/> is <c>null</c> or empty.</param>
        /// <returns>
        /// The return value of <paramref name="otherwiseFactory"/> if <paramref name="array"/> is <c>null</c> or empty;
        /// otherwise, <paramref name="array"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is empty and <paramref name="otherwiseFactory"/> is <c>null</c>.</exception>
        public static T[] NotNullOrEmpty<T>(this T[] array, Func<T[]> otherwiseFactory)
        {
            switch (array)
            {
                case null:
                case var _ when array.Length < 1:
                    if (otherwiseFactory is null)
                        throw new ArgumentNullException(nameof(otherwiseFactory));
                    return otherwiseFactory();
                default:
                    return array;
            }
        }

        /// <summary>
        /// Guards an enumerable against being <c>null</c> or empty, returning an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <param name="otherwise">The value to return in case <paramref name="enumerable"/> is <c>null</c> or empty.</param>
        /// <returns>
        /// <paramref name="otherwise"/> if <paramref name="enumerable"/> is <c>null</c> or empty;
        /// otherwise, a wrapper around <paramref name="enumerable"/> enumerating the same elements.
        /// </returns>
        /// <remarks>
        /// In order to ensure that <paramref name="enumerable"/> is not empty, the enumerable has to be expanded by checking
        /// if the first element is availble. To prevent double evaluation of the enumerable, the created enumerator is
        /// preserved and wrapped in the new enumerable that is returned.
        /// </remarks>
        public static IEnumerable<T> NotNullOrEmpty<T>(this IEnumerable<T> enumerable, IEnumerable<T> otherwise)
        {
            switch (enumerable)
            {
                case null:
                    return otherwise;
                case T[] array:
                    if (array.Length < 1)
                        return otherwise;
                    break;
                case ICollection<T> collection:
                    if (collection.Count < 1)
                        return otherwise;
                    break;
                case string str:
                    if (str.Length < 1)
                        return otherwise;
                    break;
                default:
                    var enumerator = enumerable.GetEnumerator();
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        return otherwise;
                    }
                    IEnumerable<T> wrapAroundEnumerable()
                    {
                        using (enumerator)
                        {
                            do
                            {
                                yield return enumerator.Current;
                            } while (enumerator.MoveNext());
                        }
                    }
                    return wrapAroundEnumerable();
            }
            return enumerable;
        }

        /// <summary>
        /// Guards an enumerable against being <c>null</c> or empty, creating an alternative default value if the check fails.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="enumerable">The enumerable to check.</param>
        /// <param name="otherwiseFactory">The function to produce the value to return in case <paramref name="enumerable"/> is <c>null</c> or empty.</param>
        /// <returns>
        /// The return value of <paramref name="otherwiseFactory"/> if <paramref name="enumerable"/> is <c>null</c> or empty;
        /// otherwise, a wrapper around <paramref name="enumerable"/> enumerating the same elements.
        /// </returns>
        /// <remarks>
        /// In order to ensure that <paramref name="enumerable"/> is not empty, the enumerable has to be expanded by checking
        /// if the first element is availble. To prevent double evaluation of the enumerable, the created enumerator is
        /// preserved and wrapped in the new enumerable that is returned.
        /// </remarks>
        public static IEnumerable<T> NotNullOrEmpty<T>(this IEnumerable<T> enumerable, Func<IEnumerable<T>> otherwiseFactory)
        {
            switch (enumerable)
            {
                case null:
                    break;
                case T[] array:
                    if (array.Length < 1)
                        break;
                    return enumerable;
                case ICollection<T> collection:
                    if (collection.Count < 1)
                        break;
                    return enumerable;
                case string str:
                    if (str.Length < 1)
                        break;
                    return enumerable;
                default:
                    var enumerator = enumerable.GetEnumerator();
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        break;
                    }
                    IEnumerable<T> wrapAroundEnumerable()
                    {
                        using (enumerator)
                        {
                            do
                            {
                                yield return enumerator.Current;
                            } while (enumerator.MoveNext());
                        }
                    }
                    return wrapAroundEnumerable();
            }
            if (otherwiseFactory is null)
                throw new ArgumentNullException(nameof(otherwiseFactory));
            return otherwiseFactory();
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
        /// Checks whether the specified instance is not <c>null</c> and returns it through the out-parameter.
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
        /// Checks whether the specified array is neither <c>null</c> nor empty and returns it through the out-parameter.
        /// </summary>
        /// <param name="array">The array to check against <c>null</c> or the empty array.</param>
        /// <param name="value">Always returns back the value of <paramref name="array"/>.</param>
        /// <returns><c>true</c> if <paramref name="array"/> is neither <c>null</c> nor empty; otherwise, <c>false</c>.</returns>
        public static bool TryNotNullOrEmpty<T>(this T[] array, out T[] value)
        {
            value = array;
            return !(array is null) && array.Length >= 1;
        }

        /// <summary>
        /// Checks whether the specified enumerable is neither <c>null</c> nor empty and returns it through the out-parameter.
        /// </summary>
        /// <param name="enumerable">The enumerable to check against <c>null</c> or the empty array.</param>
        /// <param name="value">Receives a wrapper around <paramref name="enumerable"/> enumerating the same items.</param>
        /// <returns><c>true</c> if <paramref name="enumerable"/> is neither <c>null</c> nor empty; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// In order to ensure that <paramref name="enumerable"/> is not empty, the enumerable has to be expanded by checking
        /// if the first element is availble. To prevent double evaluation of the enumerable, the created enumerator is
        /// preserved and wrapped in the new enumerable that is returned.
        /// </remarks>
        public static bool TryNotNullOrEmpty<T>(this IEnumerable<T> enumerable, out IEnumerable<T> value)
        {
            value = enumerable;
            switch (enumerable)
            {
                case null:
                    return false;
                case T[] array:
                    return array.Length >= 1;
                case ICollection<T> collection:
                    return collection.Count >= 1;
                case string str:
                    return str.Length >= 1;
                default:
                    var enumerator = enumerable.GetEnumerator();
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Dispose();
                        return false;
                    }
                    IEnumerable<T> wrapAroundEnumerable()
                    {
                        using (enumerator)
                        {
                            do
                            {
                                yield return enumerator.Current;
                            } while (enumerator.MoveNext());
                        }
                    }
                    value = wrapAroundEnumerable();
                    return true;
            }
        }

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
