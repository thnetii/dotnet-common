using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Collections.Generic
{
    /// <summary>
    /// Provides extension methods for enumerable types that provide random access
    /// by indexing.
    /// </summary>
    /// <remarks>
    /// Indexeable enumerable-like types are
    /// <list type="bullet">
    /// <item><c>T[]</c> (array of T)</item>
    /// <item><see cref="string"/></item>
    /// <item><see cref="IList{T}"/></item>
    /// <item><see cref="IReadOnlyList{T}"/></item>
    /// <item><see cref="Span{T}"/></item>
    /// <item><see cref="ReadOnlySpan{T}"/></item>
    /// </list>
    /// </remarks>
    public static class IndexedCollectionExtensions
    {
        #region First / ElementAt / Last
        #region FirstIndexed
        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to return the first element of.</param>
        /// <returns>The first element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        /// <remarks>If the enumerable does not support access by index, an appropriate LINQ method will be invoked.</remarks>
        /// <seealso cref="Enumerable.First{TSource}(IEnumerable{TSource})"/>
        public static T FirstIndexed<T>(this IEnumerable<T> enumerable) => enumerable.ElementAtIndex(0);

        /// <summary>
        /// Returns the first character (i.e. the 0th character) in the string.
        /// </summary>
        /// <param name="s">The string to return the first character of.</param>
        /// <returns>The first character in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        public static char FirstIndexed(this string s) => s.ElementAtIndex(0);

        /// <summary>
        /// Returns a read-only reference to the first element in the span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <returns>A read-only reference to the first element in the span.</returns>
        public static ref readonly T FirstIndexed<T>(this ReadOnlySpan<T> span) => ref span[0];

        /// <summary>
        /// Returns a reference to the first element in the span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <returns>A reference to the first element in the span.</returns>S
        public static ref T FirstIndexed<T>(this Span<T> span) => ref span[0];
        #endregion
        #region LastIndexed
        /// <summary>
        /// Returns the last indexed item (i.e. the element at index <c>count - 1</c>) in the indexed enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to return the last element of.</param>
        /// <returns>The last element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        /// <seealso cref="Enumerable.Last{TSource}(IEnumerable{TSource})"/>
        public static T LastIndexed<T>(this IEnumerable<T> enumerable)
        {
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array:
                    return array[array.Length - 1];
                case IList<T> list:
                    return list[list.Count - 1];
                case IReadOnlyList<T> list:
                    return list[list.Count - 1];
                default: return enumerable.Last();
            }
        }

        /// <summary>
        /// Returns the last character (i.e. the character at index <c>length - 1</c>) in the string.
        /// </summary>
        /// <param name="s">The string to return the last character of.</param>
        /// <returns>The last character in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        public static char LastIndexed(this string s)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            return s[s.Length - 1];
        }

        /// <summary>
        /// Returns a read-only reference to the last element in the span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <returns>A read-only reference to the last element in the span.</returns>
        public static ref readonly T LastIndexed<T>(this ReadOnlySpan<T> span) =>
            ref span.ElementAtIndex(span.Length - 1);

        /// <summary>
        /// Returns a read-only reference to the last element in the span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <returns>A read-only reference to the last element in the span.</returns>
        public static ref T LastIndexed<T>(this Span<T> span) =>
            ref span.ElementAtIndex(span.Length - 1);
        #endregion
        #region ElementAtIndex
        /// <summary>
        /// Returns the element at a specified position in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>The element at the specified position in the specified enumerable.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        /// <seealso cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/>
        public static T ElementAtIndex<T>(this IEnumerable<T> enumerable, int index)
        {
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array:
                    return array[index];
                case IList<T> list:
                    return list[index];
                case IReadOnlyList<T> list:
                    return list[index];
                default:
                    return enumerable.ElementAt(index);
            }
        }

        /// <summary>
        /// Returns the character at a specified position in the string.
        /// </summary>
        /// <param name="s">The string to return a character from.</param>
        /// <param name="index">The zero-based index of the character to retrieve.</param>
        /// <returns>The character at the specified position in the string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        public static char ElementAtIndex(this string s, int index) =>
            s.ThrowIfNull(nameof(s))[index];

        /// <summary>
        /// Returns a read-only reference to the element at a specified position in the span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>A read-only reference to the element at the specified position in the specified span.</returns>
        public static ref readonly T ElementAtIndex<T>(this ReadOnlySpan<T> span, int index) =>
            ref span[index];

        /// <summary>
        /// Returns a reference to the element at a specified position in the span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>A reference to the element at the specified position in the specified span.</returns>
        public static ref T ElementAtIndex<T>(this Span<T> span, int index) =>
            ref span[index];
        #endregion
        #endregion
        #region First / ElementAt / Last (or default)
        #region FirstIndexedOrDefault (with default value)
        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the indexed enumerable or
        /// a default value if the enumerable contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="enumerable">The enumerable to return the first element of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns><paramref name="default"/> if <paramref name="enumerable"/> is empty; otherwise the first element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        public static T FirstIndexedOrDefault<T>(this IEnumerable<T> enumerable, T @default = default) =>
            enumerable.ElementAtIndexOrDefault(index: 0, @default);

        /// <summary>
        /// Returns the first character (i.e. the 0th character) in the string
        /// or a default value if the string is empty.
        /// </summary>
        /// <param name="s">The string to return the first character of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns>The first character in <paramref name="s"/>; or <paramref name="default"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        public static char FirstIndexedOrDefault(this string s, char @default = default) =>
            s.ElementAtIndexOrDefault(index: 0, @default);

        /// <summary>
        /// Returns a read-only reference to the first element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns>A read-only reference to the first element in the span.</returns>
        public static ref readonly T FirstIndexedOrDefault<T>(this ReadOnlySpan<T> span, in T @default = default) =>
            ref span.ElementAtIndexOrDefault(index: 0, @default);

        /// <summary>
        /// Returns a reference to the first element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns>A reference to the first element in the span.</returns>
        public static ref T FirstIndexedOrDefault<T>(this Span<T> span, ref T @default) =>
            ref span.ElementAtIndexOrDefault(index: 0, ref @default);
        #endregion
        #region FirstIndexedOrDefault (with default factory)
        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the indexed enumerable or
        /// a default value if the enumerable contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="enumerable">The enumerable to return the first element of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="enumerable"/> is empty.</param>
        /// <returns>The return value from invoking <paramref name="defaultFactory"/> if <paramref name="enumerable"/> is empty; otherwise the first element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is empty and <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        public static T FirstIndexedOrDefault<T>(this IEnumerable<T> enumerable, Func<T> defaultFactory) =>
           enumerable.ElementAtIndexOrDefault(index: 0, defaultFactory);

        /// <summary>
        /// Returns the first character (i.e. the 0th character) in the string
        /// or a default value if the string is empty.
        /// </summary>
        /// <param name="s">The string to return the first character of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="s"/> is empty.</param>
        /// <returns>The return value from invoking <paramref name="defaultFactory"/> if <paramref name="s"/> is empty; otherwise the first character in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is empty and <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        public static char FirstIndexedOrDefault(this string s, Func<char> defaultFactory) =>
            s.ElementAtIndexOrDefault(index: 0, defaultFactory);

        /// <summary>
        /// Returns a read-only reference to the first element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="span"/> is empty.</param>
        /// <returns>A read-only reference to the return value from invoking <paramref name="defaultFactory"/> if <paramref name="span"/> is empty; otherwise a read-only reference to the first elemnt in <paramref name="span"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="span"/> is empty and <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        public static ref readonly T FirstIndexedOrDefault<T>(this ReadOnlySpan<T> span, RefReadOnlyFunc<T> defaultFactory) =>
            ref span.ElementAtIndexOrDefault(index: 0, defaultFactory);

        /// <summary>
        /// Returns a reference to the first element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="span"/> is empty.</param>
        /// <returns>A reference to the return value from invoking <paramref name="defaultFactory"/> if <paramref name="span"/> is empty; otherwise a read-only reference to the first elemnt in <paramref name="span"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="span"/> is empty and <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        public static ref T FirstIndexedOrDefault<T>(this Span<T> span, RefFunc<T> defaultFactory) =>
            ref span.ElementAtIndexOrDefault(index: 0, defaultFactory);
        #endregion
        #region LastIndexedOrDefault (with default value)
        /// <summary>
        /// Returns the last indexed item (i.e. the element at index <c>count - 1</c>) in the indexed enumerable or
        /// a default value if the enumerable is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to return the last element of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns>The last element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        public static T LastIndexedOrDefault<T>(this IEnumerable<T> enumerable, T @default = default)
        {
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array when array.Length < 1:
                    return @default;
                case T[] array:
                    return array[array.Length - 1];
                case IList<T> list when list.Count < 1:
                    return @default;
                case IList<T> list:
                    return list[list.Count - 1];
                case IReadOnlyList<T> list when list.Count < 1:
                    return @default;
                case IReadOnlyList<T> list:
                    return list[list.Count - 1];
                default:
                    using (var enumerator = enumerable.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            T result;
                            do
                            {
                                result = enumerator.Current;
                            } while (enumerator.MoveNext());
                            return result;
                        }
                    }
                    return @default;
            }
        }

        /// <summary>
        /// Returns the last character (i.e. the character at position <c>length - 1</c>) in the string
        /// or a default value if the string is empty.
        /// </summary>
        /// <param name="s">The string to return the last character of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns>The last character in <paramref name="s"/>; or <paramref name="default"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        public static char LastIndexedOrDefault(this string s, char @default = default)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (s.Length < 1)
                return @default;
            return s[s.Length - 1];
        }

        /// <summary>
        /// Returns a read-only reference to the last element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns>A read-only reference to the last element in the span.</returns>
        public static ref readonly T LastIndexedOrDefault<T>(this ReadOnlySpan<T> span, in T @default = default)
        {
            if (span.IsEmpty)
                return ref @default;
            return ref span[span.Length - 1];
        }

        /// <summary>
        /// Returns a reference to the last element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <param name="default">Reference to a default value to return if no value is available.</param>
        /// <returns>A reference to the last element in the span.</returns>
        public static ref T LastIndexedOrDefault<T>(this Span<T> span, ref T @default)
        {
            if (span.IsEmpty)
                return ref @default;
            return ref span[span.Length - 1];
        }
        #endregion
        #region LastIndexedOrDefault (with default factory)
        /// <summary>
        /// Returns the last indexed item (i.e. the element at index <c>count - 1</c>) in the indexed enumerable or
        /// a default value if the enumerable is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
        /// <param name="enumerable">The enumerable to return the last element of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="enumerable"/> is empty.</param>
        /// <returns>The return value from invoking <paramref name="defaultFactory"/> if <paramref name="enumerable"/> is empty; otherwise the last element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is empty and <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        public static T LastIndexedOrDefault<T>(this IEnumerable<T> enumerable, Func<T> defaultFactory)
        {
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array when array.Length < 1:
                    return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
                case T[] array:
                    return array[array.Length - 1];
                case IList<T> list when list.Count < 1:
                    return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
                case IList<T> list:
                    return list[list.Count - 1];
                case IReadOnlyList<T> list when list.Count < 1:
                    return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
                case IReadOnlyList<T> list:
                    return list[list.Count - 1];
                default:
                    using (var enumerator = enumerable.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            T result;
                            do
                            {
                                result = enumerator.Current;
                            } while (enumerator.MoveNext());
                            return result;
                        }
                    }
                    return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            }
        }

        /// <summary>
        /// Returns the last character (i.e. the character at position <c>length - 1</c>) in the string
        /// or a default value if the string is empty.
        /// </summary>
        /// <param name="s">The string to return the last character of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="s"/> is empty.</param>
        /// <returns>The return value from invoking <paramref name="defaultFactory"/> if <paramref name="s"/> is empty; otherwise the last character in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is empty and <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        public static char LastIndexedOrDefault(this string s, Func<char> defaultFactory)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (s.Length < 1)
                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return s[s.Length - 1];
        }

        /// <summary>
        /// Returns a read-only reference to the last element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="span"/> is empty.</param>
        /// <returns>The a read-only reference to the return value from invoking <paramref name="defaultFactory"/> if <paramref name="span"/> is empty; otherwise a read-only reference to the last element in <paramref name="span"/>.</returns>
        public static ref readonly T LastIndexedOrDefault<T>(this ReadOnlySpan<T> span, RefReadOnlyFunc<T> defaultFactory)
        {
            if (span.IsEmpty)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[span.Length - 1];
        }

        /// <summary>
        /// Returns a reference to the last element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="span"/> is empty.</param>
        /// <returns>The a reference to the return value from invoking <paramref name="defaultFactory"/> if <paramref name="span"/> is empty; otherwise a reference to the last element in <paramref name="span"/>.</returns>
        public static ref T LastIndexedOrDefault<T>(this Span<T> span, RefFunc<T> defaultFactory)
        {
            if (span.IsEmpty)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[span.Length - 1];
        }
        #endregion
        #region ElementAtIndexOrDefault (with default value)
        public static T ElementAtIndexOrDefault<T>(this IEnumerable<T> enumerable, int index, T @default = default)
        {
            if (index < 0)
                throw new IndexOutOfRangeException(nameof(index) + " must not be negative.");
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array when index >= array.Length:
                    return @default;
                case T[] array:
                    return array[index];
                case IList<T> list when index >= list.Count:
                    return @default;
                case IList<T> list:
                    return list[index];
                case IReadOnlyList<T> list when index >= list.Count:
                    return @default;
                case IReadOnlyList<T> list:
                    return list[index];
                default:
                    using (var enumerator = enumerable.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (index == 0)
                            {
                                return enumerator.Current;
                            }
                            index--;
                        }
                        return @default;
                    }
            }
        }

        public static char ElementAtIndexOrDefault(this string s, int index, char @default = default)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (index < 0)
                throw new IndexOutOfRangeException(nameof(index) + " must not be negative.");
            if (index >= s.Length)
                return @default;
            return s[index];
        }

        public static ref readonly T ElementAtIndexOrDefault<T>(this ReadOnlySpan<T> span, int index, in T @default = default)
        {
            if (index < 0 || index >= span.Length)
                return ref @default;
            return ref span[index];
        }

        public static ref T ElementAtIndexOrDefault<T>(this Span<T> span, int index, ref T @default)
        {
            if (index < 0 || index >= span.Length)
                return ref @default;
            return ref span[index];
        }
        #endregion
        #region ElementAtIndexOrDefault (with default factory)
        public static T ElementAtIndexOrDefault<T>(this IEnumerable<T> enumerable, int index, Func<T> defaultFactory)
        {
            if (index < 0)
                throw new IndexOutOfRangeException(nameof(index) + " must not be negative.");
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array when index >= array.Length:
                    return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
                case T[] array:
                    return array[index];
                case IList<T> list when index >= list.Count:
                    return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
                case IList<T> list:
                    return list[index];
                case IReadOnlyList<T> list when index >= list.Count:
                    return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
                case IReadOnlyList<T> list:
                    return list[index];
                default:
                    using (var enumerator = enumerable.GetEnumerator())
                    {
                        for (int i = 0; i < index; i++)
                        {
                            if (!enumerator.MoveNext())
                                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
                        }
                        return enumerator.Current;
                    }
            }
        }

        public static char ElementAtIndexOrDefault(this string s, int index, Func<char> defaultFactory)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (index < 0)
                throw new IndexOutOfRangeException(nameof(index) + " must not be negative.");
            if (index >= s.Length)
                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return s[index];
        }

        public static ref readonly T ElementAtIndexOrDefault<T>(this ReadOnlySpan<T> span, int index, RefReadOnlyFunc<T> defaultFactory)
        {
            if (index < 0 || index >= span.Length)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[index];
        }

        public static ref T ElementAtIndexOrDefault<T>(this Span<T> span, int index, RefFunc<T> defaultFactory)
        {
            if (index < 0 || index >= span.Length)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[index];
        }
        #endregion
        #endregion
    }
}
