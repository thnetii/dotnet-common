using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq
{
    partial class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether a sequence contains any elements and returns
        /// an enumerable that continues the enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> to check for emptiness.</param>
        /// <param name="nonEmpty">Receives a wrapper around the <see cref="IEnumerator{T}"/> instance and it can be used to continue the enumeration if <see langword="true"/> is returned.</param>
        /// <returns><see langword="true"/> if the source sequence contains any elements; otherwise, <see langword="false"/>.</returns>
        /// <seealso cref="Enumerable.Any{T}(IEnumerable{T})"/>
        /// <remarks>
        /// In order to determine whether an <see cref="IEnumerable{T}"/> contains
        /// any elements, an <see cref="IEnumerator{T}"/> for that enumerable has
        /// to be instatiated by calling <see cref="IEnumerable{T}.GetEnumerator"/>
        /// on that enumerable.
        /// In non-repeatable cases or in cases where enumeration of the
        /// enumerable is not idempotent, it is problematic to enumerate the
        /// enumerable after <see cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
        /// has been called. This overload method returns a wrapping <see cref="IEnumerable{T}"/>
        /// instance in the <paramref name="nonEmpty"/> parameter that wraps
        /// around the instantiated <see cref="IEnumerator{T}"/> instance.
        /// <para>
        /// If <paramref name="enumerable"/> is empty, this method returns
        /// <see langword="false"/> and <paramref name="nonEmpty"/> is set to
        /// <see langword="null"/>.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
        public static bool Any<T>(this IEnumerable<T> enumerable, out IEnumerable<T> nonEmpty)
        {
            if (enumerable is null)
                throw new ArgumentNullException(nameof(enumerable));
            var enumerator = enumerable.GetEnumerator();
            if (enumerator.MoveNext())
            {
                nonEmpty = WrapEnumerator(enumerator);
                return true;
            }
            nonEmpty = null;
            return false;

            IEnumerable<T> WrapEnumerator(IEnumerator<T> e)
            {
                do
                {
                    yield return e.Current; 
                } while (e.MoveNext());
                e.Dispose();
            }
        }

        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition
        /// and returns an enumerable that continues the enumeration.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="enumerable"/>.</typeparam>
        /// <param name="enumerable">The <see cref="IEnumerable{T}"/> whose elements to apply the <paramref name="predicate"/> to.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="nonEmpty">Receives a wrapper around the <see cref="IEnumerator{T}"/> instance and it can be used to continue the enumeration if <see langword="true"/> is returned.</param>
        /// <returns><see langword="true"/> if any elements in the <paramref name="enumerable"/> sequence pass the test in the specified <paramref name="predicate"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// In order to determine whether an <see cref="IEnumerable{T}"/> contains
        /// any elements that satisfy the condition, an <see cref="IEnumerator{T}"/>
        /// for that enumerable has to be instatiated by calling
        /// <see cref="IEnumerable{T}.GetEnumerator"/> on <paramref name="enumerable"/>.
        /// In non-repeatable cases or in cases where enumeration of the
        /// enumerable is not idempotent, it is problematic to enumerate the
        /// enumerable after <see cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>
        /// has been called. This overload method returns a wrapping <see cref="IEnumerable{T}"/>
        /// instance in the <paramref name="nonEmpty"/> parameter that wraps
        /// around the instantiated <see cref="IEnumerator{T}"/> instance.
        /// <para>
        /// If no elements in <paramref name="enumerable"/> satisfy the test
        /// specified in <paramref name="predicate"/>, this method returns
        /// <see langword="false"/> and <paramref name="nonEmpty"/> is set to
        /// <see langword="null"/>.
        /// </para>
        /// </remarks>
        /// <seealso cref="Enumerable.Any{T}(IEnumerable{T}, Func{T, bool})"/>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> or <paramref name="predicate"/> is <see langword="null"/>.</exception>
        public static bool Any<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, out IEnumerable<T> nonEmpty)
        {
            return enumerable.Where(predicate).Any(out nonEmpty);
        }
    }
}
