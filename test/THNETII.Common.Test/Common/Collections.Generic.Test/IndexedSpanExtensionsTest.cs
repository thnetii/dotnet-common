using System;
using System.Linq;

namespace THNETII.Common.Collections.Generic.Test
{
    public class IndexedSpanExtensionsTest : IndexedCollectionExtensionsTest<Span<int>, int>
    {
        protected override Span<int> GetEmpty() => Span<int>.Empty;
        protected override Span<int> GetNonEmpty() => Enumerable.Range(0, 10).ToArray();

        protected override int GetNonDefaultT() => 42;

        protected override int FirstIndexed(Span<int> span) =>
            span.FirstIndexed();

        protected override int FirstIndexedOrDefault(Span<int> span) =>
            throw new InvalidOperationException();

        protected override int FirstIndexedOrDefault(Span<int> span, int @default) =>
            span.FirstIndexedOrDefault(@default);

        protected override int FirstIndexedOrDefault(Span<int> span, Func<int> defaultFactory) =>
            span.FirstIndexedOrDefault(defaultFactory);

        protected override int LastIndexed(Span<int> span) =>
            span.LastIndexed();

        protected override int LastIndexedOrDefault(Span<int> span) =>
            throw new InvalidOperationException();

        protected override int LastIndexedOrDefault(Span<int> span, int @default) =>
            span.LastIndexedOrDefault(@default);

        protected override int LastIndexedOrDefault(Span<int> span, Func<int> defaultFactory) =>
            span.LastIndexedOrDefault(defaultFactory);

        protected override int ElementAtIndex(Span<int> span, int index) =>
            span.ElementAtIndex(index);

        protected override int ElementAtIndexOrDefault(Span<int> span, int index) =>
            throw new InvalidOperationException();

        protected override int ElementAtIndexOrDefault(Span<int> span, int index, int @default) =>
            span.ElementAtIndexOrDefault(index, @default);

        protected override int ElementAtIndexOrDefault(Span<int> span, int index, Func<int> defaultFactory) =>
            span.ElementAtIndexOrDefault(index, defaultFactory);
    }
}
