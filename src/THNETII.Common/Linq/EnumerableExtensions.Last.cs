using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Returns the last element of a sequence, or a specified default value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return the last element of.</param>
        /// <param name="default">The value to return if <paramref name="enumerable"/> contains no elements.</param>
        /// <returns><paramref name="default"/> if <paramref name="enumerable"/> is empty; otherwise, the last element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <c>null</c>.</exception>
        /// <seealso cref="System.Linq.Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})"/>
        public static T LastOrDefault<T>(this IEnumerable<T> enumerable, T @default) =>
            enumerable.LastOrDefault(() => @default);

        /// <summary>
        /// Returns the last element of a sequence, or produces a value if the sequence contains no elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">An <see cref="IEnumerable{T}"/> to return the last element of.</param>
        /// <param name="defaultFactory">The function to prodruce the return value if <paramref name="enumerable"/> contains no elements.</param>
        /// <returns>The return value of <paramref name="defaultFactory"/> if <paramref name="enumerable"/> is empty; otherwise, the last element in <paramref name="enumerable"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        /// <seealso cref="System.Linq.Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})"/>
        public static T LastOrDefault<T>(this IEnumerable<T> enumerable, Func<T> defaultFactory)
        {
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array when array.Length < 1: break;
                case T[] array:
                    return array[array.Length - 1];
                case IList<T> list when list.Count < 1: break;
                case IList<T> list:
                    return list[list.Count - 1];
                case IReadOnlyList<T> list when list.Count < 1: break;
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
                    break;
            }
            return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
        }
    }
}
