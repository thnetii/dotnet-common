using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.Common.Collections.Generic
{
    /// <summary>
    /// Provides extension methods for indexable types.
    /// </summary>
    public static class IndexedCollectionExtensions
    {
        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to return the first element of.</param>
        /// <returns>The first element in <paramref name="array"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="array"/> contains no elements.</exception>
        public static T FirstIndexed<T>(this T[] array)
        {
            try { return ElementAtIndex(array, 0); }
            catch (IndexOutOfRangeException rangeException)
            { throw new InvalidOperationException("Array contains no elements", rangeException); }
        }

        public static T ElementAtIndex<T>(this T[] array, int index) =>
            array.ThrowIfNull(nameof(array))[index];

        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the array or
        /// a default value if the array contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to return the first element of.</param>
        /// <param name="default">Optional default value to return if there is no value available.</param>
        /// <returns><c>default(<typeparamref name="T"/>)</c> if <paramref name="array"/> is empty; otherwise the first element in <paramref name="array"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
        public static T FirstIndexedOrDefault<T>(this T[] array, T @default = default) =>
            ElementAtIndexOrDefault(array, 0, @default);

        public static T FirstIndexedOrDefault<T>(this T[] array, Func<T> defaultFactory) =>
            ElementAtIndexOrDefault(array, 0, defaultFactory);

        public static T ElementAtIndexOrDefault<T>(this T[] array, int index, T @default = default)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            else if (index < 0 || index >= array.Length)
                return @default;
            return array[index];
        }

        public static T ElementAtIndexOrDefault<T>(this T[] array, int index, Func<T> defaultFactory)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            else if (index < 0 || index >= array.Length)
                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return array[index];
        }

        /// <summary>
        /// Returns the first indexed character (i.e. the 0th element) in the string.
        /// </summary>
        /// <param name="string">The string to return the first character of.</param>
        /// <returns>The first character in <paramref name="string"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="string"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="string"/> is the empty string.</exception>
        public static char FirstIndexed([SuppressMessage("Naming", "CA1720:Identifier contains type name")] this string @string)
        {
            try { return ElementAtIndex(@string, 0); }
            catch (IndexOutOfRangeException rangeException)
            { throw new InvalidOperationException("String contains no characters", rangeException); }
        }

        public static char ElementAtIndex([SuppressMessage("Naming", "CA1720:Identifier contains type name")] this string @string, int index) =>
            @string.ThrowIfNull(nameof(@string))[index];

        /// <summary>
        /// Returns the first indexed character (i.e. the 0th element) in the string or
        /// the default character if the string is empty.
        /// </summary>
        /// <param name="string">The string to return the first character of.</param>
        /// <returns><c>default(<see cref="char"/>)</c> if <paramref name="string"/> is the empty string; otherwise the first character in <paramref name="string"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="string"/> is <c>null</c>.</exception>
        public static char FirstIndexedOrDefault([SuppressMessage("Naming", "CA1720:Identifier contains type name")] this string @string, char @default = default) =>
            ElementAtIndexOrDefault(@string, 0, @default);

        public static char FirstIndexedOrDefault([SuppressMessage("Naming", "CA1720:Identifier contains type name")] this string @string, Func<char> defaultFactory) =>
            ElementAtIndexOrDefault(@string, 0, defaultFactory);

        public static char ElementAtIndexOrDefault([SuppressMessage("Naming", "CA1720:Identifier contains type name")] this string @string, int index, char @default = default)
        {
            if (@string == null)
                throw new ArgumentNullException(nameof(@string));
            else if (index < 0 || index >= @string.Length)
                return @default;
            return @string[index];
        }

        public static char ElementAtIndexOrDefault([SuppressMessage("Naming", "CA1720:Identifier contains type name")] this string @string, int index, Func<char> defaultFactory)
        {
            if (@string == null)
                throw new ArgumentNullException(nameof(@string));
            else if (index < 0 || index >= @string.Length)
                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return @string[index];
        }

        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list to return the first element of.</param>
        /// <returns>The first element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
        public static T FirstIndexed<T>(this IList<T> source)
        {
            try { return ElementAtIndex(source, 0); }
            catch (IndexOutOfRangeException rangeException)
            { throw new InvalidOperationException("List does not contain any elements.", rangeException); }
        }

        public static T ElementAtIndex<T>(this IList<T> list, int index) =>
            list.ThrowIfNull(nameof(list))[index];

        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the list or
        /// a default value if the list contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to return the first element of.</param>
        /// <returns><c>default(T)</c> if <paramref name="list"/> is empty; otherwise the first element in <paramref name="list"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <c>null</c>.</exception>
        public static T FirstIndexedOrDefault<T>(this IList<T> list, T @default = default) =>
            ElementAtIndexOrDefault(list, 0, @default);

        public static T ElementAtIndexOrDefault<T>(this IList<T> list, int index, T @default = default)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            else if (index < 0 || index >= list.Count)
                return default;
            return list[index];
        }

        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list to return the first element of.</param>
        /// <returns>The first element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
        public static T FirstIndexed<T>(this IReadOnlyList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Count < 1)
                throw new InvalidOperationException();
            return source[0];
        }

        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the list or
        /// a default value if the list contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list to return the first element of.</param>
        /// <returns><c>default(T)</c> if <paramref name="source"/> is empty; otherwise the first element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static T FirstIndexedOrDefault<T>(this IReadOnlyList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Count < 1)
                return default;
            return source[0];
        }

        /// <summary>
        /// Returns the last indexed item (i.e. the 0th element) in the array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array to return the last element of.</param>
        /// <returns>The last element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
        public static T LastIndexed<T>(this T[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Length < 1)
                throw new InvalidOperationException();
            return source[source.Length - 1];
        }

        /// <summary>
        /// Returns the last indexed item (i.e. the 0th element) in the array or
        /// a default value if the array contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array to return the last element of.</param>
        /// <returns><c>default(<typeparamref name="T"/>)</c> if <paramref name="source"/> is empty; otherwise the last element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static T LastIndexedOrDefault<T>(this T[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Length < 1)
                return default;
            return source[source.Length - 1];
        }

        /// <summary>
        /// Returns the last indexed character (i.e. the 0th element) in the string.
        /// </summary>
        /// <param name="source">The string to return the last character of.</param>
        /// <returns>The last character in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is the empty string.</exception>
        public static char LastIndexed(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Length < 1)
                throw new InvalidOperationException("String contains no characters");
            return source[source.Length - 1];
        }

        /// <summary>
        /// Returns the last indexed character (i.e. the 0th element) in the string or
        /// the default character if the string is empty.
        /// </summary>
        /// <param name="source">The string to return the last character of.</param>
        /// <returns><c>default(<see cref="char"/>)</c> if <paramref name="source"/> is the empty string; otherwise the last character in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static char LastIndexedOrDefault(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Length < 1)
                return default;
            return source[source.Length - 1];
        }

        /// <summary>
        /// Returns the last indexed item (i.e. the 0th element) in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list to return the last element of.</param>
        /// <returns>The last element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
        public static T LastIndexed<T>(this IList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Count < 1)
                throw new InvalidOperationException();
            return source[source.Count - 1];
        }

        /// <summary>
        /// Returns the last indexed item (i.e. the 0th element) in the list or
        /// a default value if the list contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list to return the last element of.</param>
        /// <returns><c>default(T)</c> if <paramref name="source"/> is empty; otherwise the last element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static T LastIndexedOrDefault<T>(this IList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Count < 1)
                return default;
            return source[source.Count - 1];
        }

        /// <summary>
        /// Returns the last indexed item (i.e. the 0th element) in the list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list to return the last element of.</param>
        /// <returns>The last element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
        public static T LastIndexed<T>(this IReadOnlyList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Count < 1)
                throw new InvalidOperationException();
            return source[source.Count - 1];
        }

        /// <summary>
        /// Returns the last indexed item (i.e. the 0th element) in the list or
        /// a default value if the list contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="source">The list to return the last element of.</param>
        /// <returns><c>default(T)</c> if <paramref name="source"/> is empty; otherwise the last element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static T LastIndexedOrDefault<T>(this IReadOnlyList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Count < 1)
                return default;
            return source[source.Count - 1];
        }
    }
}
