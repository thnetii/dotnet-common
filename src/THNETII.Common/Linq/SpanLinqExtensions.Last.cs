using System;

namespace THNETII.Common.Linq
{
    public static partial class SpanLinqExtensions
    {
        /// <summary>
        /// Returns the last element of a read-only span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <returns>A read-only reference to the last element in <paramref name="span"/>.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="span"/> is empty.</exception>
        public static ref readonly T Last<T>(this ReadOnlySpan<T> span)
        {
            try { return ref span[span.Length - 1]; }
            catch (IndexOutOfRangeException indexExcept)
            {
                throw new InvalidOperationException("The span is empty.", indexExcept);
            }
        }

        /// <summary>
        /// Returns the last element of a read-only span, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <param name="default">An optional read-only reference to the value to return, if <paramref name="span"/> is empty.</param>
        /// <returns>
        /// A read-only reference to <paramref name="default"/> if <paramref name="span"/> is empty;
        /// otherwise, a read-only reference to the last element in <paramref name="span"/>.
        /// </returns>
        public static ref readonly T LastOrDefault<T>(this ReadOnlySpan<T> span, in T @default = default)
        {
            if (span.IsEmpty)
                return ref @default;
            return ref span[span.Length - 1];
        }

        /// <summary>
        /// Returns the last element of a read-only span, or produces a value if no element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <param name="defaultFactory">The function to produce the return value if <paramref name="span"/> is empty.</param>
        /// <returns>
        /// A read-only reference to the return value of <paramref name="defaultFactory"/> if <paramref name="span"/> is empty;
        /// otherwise, a read-only reference to the last element in <paramref name="span"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="span"/> is empty and <paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        public static ref readonly T LastOrDefault<T>(this ReadOnlySpan<T> span, RefReadOnlyFunc<T> defaultFactory)
        {
            if (span.IsEmpty)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[span.Length - 1];
        }

        /// <summary>
        /// Returns the last element of a span.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <returns>A reference to the last element in <paramref name="span"/>.</returns>
        /// <exception cref="InvalidOperationException"><paramref name="span"/> is empty.</exception>
        public static ref T Last<T>(this Span<T> span)
        {
            try { return ref span[span.Length - 1]; }
            catch (IndexOutOfRangeException indexExcept)
            {
                throw new InvalidOperationException("The span is empty.", indexExcept);
            }
        }

        /// <summary>
        /// Returns the last element of a span, or a default value if no element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <param name="default">A reference to the value to return, if <paramref name="span"/> is empty.</param>
        /// <returns>
        /// A reference to <paramref name="default"/> if <paramref name="span"/> is empty;
        /// otherwise, a reference to the last element in <paramref name="span"/>.
        /// </returns>
        public static ref T LastOrDefault<T>(this Span<T> span, ref T @default)
        {
            if (span.IsEmpty)
                return ref @default;
            return ref span[span.Length - 1];
        }

        /// <summary>
        /// Returns the last element of a span, or produces a value if no element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the span.</typeparam>
        /// <param name="span">The span to return the last element of.</param>
        /// <param name="defaultFactory">The function to produce the return value if <paramref name="span"/> is empty.</param>
        /// <returns>
        /// A reference to the return value of <paramref name="defaultFactory"/> if <paramref name="span"/> is empty;
        /// otherwise, a reference to the last element in <paramref name="span"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="span"/> is empty and <paramref name="defaultFactory"/> is <see langword="null"/>.</exception>
        public static ref T LastOrDefault<T>(this Span<T> span, RefFunc<T> defaultFactory)
        {
            if (span.IsEmpty)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[span.Length - 1];
        }
    }
}
