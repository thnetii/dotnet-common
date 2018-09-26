using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq
{
    public static partial class SpanLinqExtensions
    {
        public static ref readonly T First<T>(this ReadOnlySpan<T> span) =>
            ref span.ElementAt(0);

        /// <summary>
        /// Returns a read-only reference to the first element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns>A read-only reference to the first element in the span.</returns>
        /// <seealso cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
        public static ref readonly T FirstOrDefault<T>(this ReadOnlySpan<T> span, in T @default = default) =>
            ref span.ElementAtOrDefault(index: 0, @default);

        /// <summary>
        /// Returns a read-only reference to the first element in the span or
        /// calculates a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="span"/> is empty.</param>
        /// <returns>A read-only reference to the return value from invoking <paramref name="defaultFactory"/> if <paramref name="span"/> is empty; otherwise a read-only reference to the first elemnt in <paramref name="span"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="span"/> is empty and <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        /// <seealso cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
        public static ref readonly T FirstOrDefault<T>(this ReadOnlySpan<T> span, RefReadOnlyFunc<T> defaultFactory) =>
            ref span.ElementAtOrDefault(index: 0, defaultFactory);

        public static ref T First<T>(this Span<T> span) =>
            ref span.ElementAt(0);

        /// <summary>
        /// Returns a reference to the first element in the span or
        /// a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <param name="default">Optional default value to return if no value is available.</param>
        /// <returns>A reference to the first element in the span.</returns>
        /// <seealso cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
        public static ref T FirstOrDefault<T>(this Span<T> span, ref T @default) =>
            ref span.ElementAtOrDefault(index: 0, ref @default);

        /// <summary>
        /// Returns a reference to the first element in the span or
        /// calculates a default value if the span is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the first element of.</param>
        /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="span"/> is empty.</param>
        /// <returns>A reference to the return value from invoking <paramref name="defaultFactory"/> if <paramref name="span"/> is empty; otherwise a read-only reference to the first elemnt in <paramref name="span"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="span"/> is empty and <paramref name="defaultFactory"/> is <c>null</c>.</exception>
        /// <seealso cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
        public static ref T FirstOrDefault<T>(this Span<T> span, RefFunc<T> defaultFactory) =>
            ref span.ElementAtOrDefault(index: 0, defaultFactory);
    }
}
