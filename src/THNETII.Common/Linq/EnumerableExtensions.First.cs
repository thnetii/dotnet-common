using System;
using System.Collections.Generic;

namespace THNETII.Common.Linq
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence, or a specified default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
        /// <param name="default">The value to return if <paramref name="enumerable"/> contains no elements.</param>
        /// <returns><paramref name="default"/> if <paramref name="enumerable"/> is empty; otherwise, the first element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        /// <seealso cref="System.Linq.Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, T @default) =>
            enumerable.ElementAtOrDefault(0, @default);

        /// <summary>
        /// Returns the first element of a sequence, or produces a value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
        /// <param name="defaultFactory">The function to prodruce the return value if <paramref name="enumerable"/> contains no elements.</param>
        /// <returns>The return value of <paramref name="defaultFactory"/> if <paramref name="enumerable"/> is empty; otherwise, the first element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        /// <seealso cref="System.Linq.Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T> defaultFactory) =>
            enumerable.ElementAtOrDefault(0, defaultFactory);
    }
}
