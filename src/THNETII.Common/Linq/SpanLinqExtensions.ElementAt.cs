using System;

namespace THNETII.Common.Linq
{
    public static partial class SpanLinqExtensions
    {
        public static ref readonly T ElementAt<T>(this ReadOnlySpan<T> span, int index) =>
            ref span[index];

        public static ref readonly T ElementAtOrDefault<T>(this ReadOnlySpan<T> span, int index, in T @default = default)
        {
            if (index < 0 || index >= span.Length)
                return ref @default;
            return ref span[index];
        }

        public static ref readonly T ElementAtOrDefault<T>(this ReadOnlySpan<T> span, int index, RefReadOnlyFunc<T> defaultFactory)
        {
            if (index < 0 || index >= span.Length)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[index];
        }

        public static ref T ElementAt<T>(this Span<T> span, int index) =>
            ref span[index];

        public static ref T ElementAtOrDefault<T>(this Span<T> span, int index, ref T @default)
        {
            if (index < 0 || index >= span.Length)
                return ref @default;
            return ref span[index];
        }

        public static ref T ElementAtOrDefault<T>(this Span<T> span, int index, RefFunc<T> defaultFactory)
        {
            if (index < 0 || index >= span.Length)
                return ref defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            return ref span[index];
        }
    }
}
