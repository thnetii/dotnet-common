using System;
using System.Linq;

namespace THNETII.Common.Collections.Generic.Test
{
    class IndexedReadOnlySpanExtensionsTest : IndexedCollectionExtensionsTest<ReadOnlySpan<int>, int>
    {
        protected override ReadOnlySpan<int> GetEmpty() => ReadOnlySpan<int>.Empty;
        protected override ReadOnlySpan<int> GetNonEmpty() => Enumerable.Range(0, 10).ToArray();

        protected override int GetNonDefaultT() => 42;

        protected override int FirstIndexed(ReadOnlySpan<int> span) =>
            span.FirstIndexed();

        protected override int FirstIndexedOrDefault(ReadOnlySpan<int> span) =>
            throw new InvalidOperationException();

        protected override int FirstIndexedOrDefault(ReadOnlySpan<int> span, int @default) =>
            span.FirstIndexedOrDefault(@default);

        protected override int FirstIndexedOrDefault(ReadOnlySpan<int> span, Func<int> defaultFactory) =>
            span.FirstIndexedOrDefault(defaultFactory);

        protected override int LastIndexed(ReadOnlySpan<int> span) =>
            span.LastIndexed();

        protected override int LastIndexedOrDefault(ReadOnlySpan<int> span) =>
            throw new InvalidOperationException();

        protected override int LastIndexedOrDefault(ReadOnlySpan<int> span, int @default) =>
            span.LastIndexedOrDefault(@default);

        protected override int LastIndexedOrDefault(ReadOnlySpan<int> span, Func<int> defaultFactory) =>
            span.LastIndexedOrDefault(defaultFactory);

        protected override int ElementAtIndex(ReadOnlySpan<int> span, int index) =>
            span.ElementAtIndex(index);

        protected override int ElementAtIndexOrDefault(ReadOnlySpan<int> span, int index) =>
            throw new InvalidOperationException();

        protected override int ElementAtIndexOrDefault(ReadOnlySpan<int> span, int index, int @default) =>
            span.ElementAtIndexOrDefault(index, @default);

        protected override int ElementAtIndexOrDefault(ReadOnlySpan<int> span, int index, Func<int> defaultFactory) =>
            span.ElementAtIndexOrDefault(index, defaultFactory);

    }
}
