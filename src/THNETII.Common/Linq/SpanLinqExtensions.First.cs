using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq;

public static partial class SpanLinqExtensions
{
    /// <summary>
    /// Returns the first element of a read-only span.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the span.</typeparam>
    /// <param name="span">The span to return the first element of.</param>
    /// <returns>A read-only reference to the first element in the span.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="span"/> is empty.</exception>
    /// <seealso cref="Enumerable.First{TSource}(IEnumerable{TSource})"/>
    public static ref readonly T First<T>(this ReadOnlySpan<T> span)
    {
        try { return ref span.ElementAt(0); }
        catch (ArgumentOutOfRangeException argExcept)
        {
            throw new InvalidOperationException("The span is empty.", argExcept);
        }
    }

    /// <summary>
    /// Returns a read-only reference to the first element in the span or
    /// a default value if the span is empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the span.</typeparam>
    /// <param name="span">The span to return the first element of.</param>
    /// <param name="default">Optional default value to return if no value is available.</param>
    /// <returns>A read-only reference to the first element in the span.</returns>
    /// <seealso cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
    public static ref readonly T FirstOrDefault<T>(this ReadOnlySpan<T> span, in T @default) =>
        ref span.ElementAtOrDefault(index: 0, @default);

    /// <summary>
    /// Returns a read-only reference to the first element in the span or
    /// calculates a default value if the span is empty.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the span.</typeparam>
    /// <param name="span">The span to return the first element of.</param>
    /// <param name="defaultFactory">A function to invoke to produce the value to return if <paramref name="span"/> is empty.</param>
    /// <returns>A read-only reference to the return value from invoking <paramref name="defaultFactory"/> if <paramref name="span"/> is empty; otherwise a read-only reference to the first elemnt in <paramref name="span"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="span"/> is empty and <paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
    /// <seealso cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
    public static ref readonly T FirstOrDefault<T>(this ReadOnlySpan<T> span, RefReadOnlyFunc<T> defaultFactory) =>
        ref span.ElementAtOrDefault(index: 0, defaultFactory);

    /// <summary>
    /// Returns the first element of a span.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the span.</typeparam>
    /// <param name="span">The span to return the first element of.</param>
    /// <returns>A read-only reference to the first element in the span.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="span"/> is empty.</exception>
    /// <seealso cref="Enumerable.First{TSource}(IEnumerable{TSource})"/>
    public static ref T First<T>(this Span<T> span)
    {
        try { return ref span.ElementAt(0); }
        catch (ArgumentOutOfRangeException argExcept)
        {
            throw new InvalidOperationException("The span is empty.", argExcept);
        }
    }

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
    /// <exception cref="ArgumentNullException"><paramref name="span"/> is empty and <paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
    /// <seealso cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})"/>
    public static ref T FirstOrDefault<T>(this Span<T> span, RefFunc<T> defaultFactory) =>
        ref span.ElementAtOrDefault(index: 0, defaultFactory);
}
