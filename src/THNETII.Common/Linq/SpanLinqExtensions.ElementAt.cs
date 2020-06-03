using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq
{
    public static partial class SpanLinqExtensions
    {
        /// <summary>
        /// Returns the element at a specified index in a read-only span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="span"/>.</typeparam>
        /// <param name="span">The <see cref="ReadOnlySpan{T}"/> to return an element from.</param>
        /// <param name="index">The zero-base index of the element to retrieve.</param>
        /// <returns>A read-only reference to the element at the specified position in the span.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0
        /// or
        /// greater than or equal to the <see cref="ReadOnlySpan{T}.Length"/> of <paramref name="span"/>.
        /// </exception>
        /// <seealso cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/>
        public static ref readonly T ElementAt<T>(this ReadOnlySpan<T> span, int index)
        {
            try { return ref span[index]; }
            catch (IndexOutOfRangeException rangeExcept)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(index),
                    index,
                    rangeExcept.Message
                    );
            }
        }

        /// <summary>
        /// Returns the element at a specified index in a read-only span or a default value if the index is out of range.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="span"/>.</typeparam>
        /// <param name="span">The <see cref="ReadOnlySpan{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="default">An optional read-only reference to the value to return if <paramref name="index"/> is out of range.</param>
        /// <returns>
        /// A read-only reference to <paramref name="default"/> if <paramref name="index"/> is outside the valid range of <paramref name="span"/>;
        /// otherwise, a read-only reference to the element at the specified position in the span.
        /// </returns>
        /// <seealso cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
        public static ref readonly T ElementAtOrDefault<T>(this ReadOnlySpan<T> span, int index, in T @default = default)
        {
            if (index < 0 || index >= span.Length)
                return ref @default;
            return ref span[index];
        }

        /// <summary>
        /// Returns the element at a specified index in a read-only span or produces value if the index is out of range.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="span"/>.</typeparam>
        /// <param name="span">The <see cref="ReadOnlySpan{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="defaultFactory">The function to produce the value to return if <paramref name="index"/> is out of range.</param>
        /// <returns>
        /// A read-only reference to the return value of <paramref name="defaultFactory"/> if <paramref name="index"/> is outside the valid range of <paramref name="span"/>;
        /// otherwise, a read-only reference to the element at the specified position in the span.
        /// </returns>
        /// <seealso cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
        /// <exception cref="ArgumentNullException"><paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        public static ref readonly T ElementAtOrDefault<T>(this ReadOnlySpan<T> span, int index, RefReadOnlyFunc<T> defaultFactory)
        {
            if (index < 0 || index >= span.Length)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[index];
        }

        /// <summary>
        /// Returns the element at a specified index in a span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="span"/>.</typeparam>
        /// <param name="span">The <see cref="Span{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>A reference to the element at the specified position in the span.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0
        /// or
        /// greater than or equal to the <see cref="Span{T}.Length"/> of <paramref name="span"/>.
        /// </exception>
        /// <seealso cref="Enumerable.ElementAt{TSource}(IEnumerable{TSource}, int)"/>
        public static ref T ElementAt<T>(this Span<T> span, int index)
        {
            try { return ref span[index]; }
            catch (IndexOutOfRangeException rangeExcept)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(index), index, rangeExcept.Message
                    );
            }
        }

        /// <summary>
        /// Returns the element at a specified index in a span or a default value if the index is out of range.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="span"/>.</typeparam>
        /// <param name="span">The <see cref="Span{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="default">A reference to the value to return if <paramref name="index"/> is out of range.</param>
        /// <returns>
        /// A reference to <paramref name="default"/> if <paramref name="index"/> is outside the valid range of <paramref name="span"/>;
        /// otherwise, a reference to the element at the specified position in the span.
        /// </returns>
        /// <seealso cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
        public static ref T ElementAtOrDefault<T>(this Span<T> span, int index, ref T @default)
        {
            if (index < 0 || index >= span.Length)
                return ref @default;
            return ref span[index];
        }

        /// <summary>
        /// Returns the element at a specified index in a span or produces value if the index is out of range.
        /// </summary>
        /// <typeparam name="T">The type of the elements in <paramref name="span"/>.</typeparam>
        /// <param name="span">The <see cref="Span{T}"/> to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <param name="defaultFactory">The function to produce the value to return if <paramref name="index"/> is out of range.</param>
        /// <returns>
        /// A read-only reference to the return value of <paramref name="defaultFactory"/> if <paramref name="index"/> is outside the valid range of <paramref name="span"/>;
        /// otherwise, a read-only reference to the element at the specified position in the span.
        /// </returns>
        /// <seealso cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)"/>
        /// <exception cref="ArgumentNullException"><paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        public static ref T ElementAtOrDefault<T>(this Span<T> span, int index, RefFunc<T> defaultFactory)
        {
            if (index < 0 || index >= span.Length)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[index];
        }
    }
}
