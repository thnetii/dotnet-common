using System;
using System.Collections.Generic;

namespace THNETII.Common.Linq
{
    public static partial class EnumerableExtensions
    {
        /// <summary>
        /// Returns the element at a specified index in a sequence, returning the
        /// specified default value if no value is available at the specified position.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="default">The value to return if <paramref name="enumerable"/> does not contain an element at position <paramref name="index"/>.</param>
        /// <returns>The element at the specified position in the source sequence, or <paramref name="default"/> if no value is available.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
        public static T ElementAtOrDefault<T>(this IEnumerable<T> enumerable, int index, T @default) =>
            enumerable.ElementAtOrDefault(index, () => @default);

        /// <summary>
        /// Returns the element at a specified index in a sequence, or calculates
        /// the value to return if no value is available at the specified position.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="defaultFactory">The function to invoke to produce the value to return if <paramref name="enumerable"/> does not contain an element at position <paramref name="index"/>.</param>
        /// <returns>The element at the specified position in the source sequence, or the return value from invoking <paramref name="defaultFactory"/> if no value is available.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        public static T ElementAtOrDefault<T>(this IEnumerable<T> enumerable, int index, Func<T> defaultFactory)
        {
            if (index < 0)
                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array when index >= array.Length: break;
                case T[] array:
                    return array[index];
                case IList<T> list when index >= list.Count: break;
                case IList<T> list:
                    return list[index];
                case IReadOnlyList<T> list when index >= list.Count: break;
                case IReadOnlyList<T> list:
                    return list[index];
                default:
                case var e:
                    using (var enumerator = e.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (index == 0)
                            {
                                return enumerator.Current;
                            }
                            index--;
                        }
                    }
                    break;
            }
            return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
        }
    }
}
