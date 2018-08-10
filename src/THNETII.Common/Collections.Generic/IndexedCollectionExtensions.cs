using System;
using System.Collections.Generic;

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
        /// <param name="source">The array to return the first element of.</param>
        /// <returns>The first element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> contains no elements.</exception>
        public static T FirstIndexed<T>(this T[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Length < 1)
                throw new InvalidOperationException("Array contains no elements");
            return source[0];
        }

        /// <summary>
        /// Returns the first indexed item (i.e. the 0th element) in the array or
        /// a default value if the array contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="source">The array to return the first element of.</param>
        /// <returns><c>default(<typeparamref name="T"/>)</c> if <paramref name="source"/> is empty; otherwise the first element in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static T FirstIndexedOrDefault<T>(this T[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Length < 1)
                return default;
            return source[0];
        }

        /// <summary>
        /// Returns the first indexed character (i.e. the 0th element) in the string.
        /// </summary>
        /// <param name="source">The string to return the first character of.</param>
        /// <returns>The first character in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is the empty string.</exception>
        public static char FirstIndexed(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Length < 1)
                throw new InvalidOperationException("String contains no characters");
            return source[0];
        }

        /// <summary>
        /// Returns the first indexed character (i.e. the 0th element) in the string or
        /// the default character if the string is empty.
        /// </summary>
        /// <param name="source">The string to return the first character of.</param>
        /// <returns><c>default(<see cref="char"/>)</c> if <paramref name="source"/> is the empty string; otherwise the first character in <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is <c>null</c>.</exception>
        public static char FirstIndexedOrDefault(this string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Length < 1)
                return default;
            return source[0];
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
        public static T FirstIndexedOrDefault<T>(this IList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            else if (source.Count < 1)
                return default;
            return source[0];
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
