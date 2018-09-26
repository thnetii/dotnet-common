using System;

namespace THNETII.Common.Linq
{
    public static partial class SpanLinqExtensions
    {
        public static ref readonly T Last<T>(this ReadOnlySpan<T> span) =>
            ref span[span.Length - 1];

        public static ref readonly T LastOrDefault<T>(this ReadOnlySpan<T> span, in T @default = default)
        {
            if (span.IsEmpty)
                return ref @default;
            return ref span[span.Length - 1];
        }

        public static ref readonly T LastOrDefault<T>(this ReadOnlySpan<T> span, RefReadOnlyFunc<T> defaultFactory)
        {
            if (span.IsEmpty)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[span.Length - 1];
        }

        public static ref T Last<T>(this Span<T> span) =>
            ref span[span.Length - 1];

        public static ref T LastOrDefault<T>(this Span<T> span, ref T @default)
        {
            if (span.IsEmpty)
                return ref @default;
            return ref span[span.Length - 1];
        }

        public static ref T LastOrDefault<T>(this Span<T> span, RefFunc<T> defaultFactory)
        {
            if (span.IsEmpty)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[span.Length - 1];
        }
    }
}
