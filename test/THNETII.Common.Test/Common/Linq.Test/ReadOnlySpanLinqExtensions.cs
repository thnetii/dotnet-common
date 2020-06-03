using System;
using System.Linq;
using Xunit;

namespace THNETII.Common.Linq.Test
{
    public class ReadOnlySpanLinqExtensions : LinqExtensionsWithIntsTest
    {
        class RefHolder
        {
            public int Value;
        }

        protected override object GetEmpty() => Array.Empty<int>();

        protected override object GetMoreThan5ButLessThan100() => Enumerable.Range(0, 10).ToArray();

        protected override int First(object source)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.First();
        }
        protected override int FirstOrDefault(object source)
        {
            source.ThrowIfNull(nameof(source));
            return default;
        }

        protected override int FirstOrDefault(object source, int @default)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.FirstOrDefault(@default);
        }

        protected override int FirstOrDefault(object source, Func<int> defaultFactory)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.FirstOrDefault(() =>
            {
                var @default = new RefHolder { Value = defaultFactory() };
                return ref @default.Value;
            });
        }

        protected override int Last(object source)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.Last();
        }

        protected override int LastOrDefault(object source)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.IsEmpty ? default : span.Last();
        }

        protected override int LastOrDefault(object source, int @default)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.LastOrDefault(@default);
        }

        protected override int LastOrDefault(object source, Func<int> defaultFactory)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.LastOrDefault(() =>
            {
                var @default = new RefHolder { Value = defaultFactory() };
                return ref @default.Value;
            });
        }

        protected override int ElementAt(object source, int index)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.ElementAt(index);
        }

        protected override int ElementAtOrDefault(object source, int index)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return (index >= 0 && index < span.Length) ? span[index] : default;
        }

        protected override int ElementAtOrDefault(object source, int index, int @default)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.ElementAtOrDefault(index, @default);
        }

        protected override int ElementAtOrDefault(object source, int index, Func<int> defaultFactory)
        {
            source.ThrowIfNull(nameof(source));
            ReadOnlySpan<int> span = (int[])source;
            return span.ElementAtOrDefault(index, () =>
            {
                var @default = new RefHolder { Value = defaultFactory() };
                return ref @default.Value;
            });
        }
    }
}
