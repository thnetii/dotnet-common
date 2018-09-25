using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Collections.Generic.Test
{
    public class IndexedListExtensionsTest : IndexedCollectionExtensionsTest<List<int>, int>
    {
        protected override List<int> GetEmpty() => new List<int>();
        protected override List<int> GetNonEmpty() => Enumerable.Range(0, 10).ToList();

        protected override int GetNonDefaultT() => 42;

        protected override int FirstIndexed(List<int> list) =>
            list.FirstIndexed();

        protected override int FirstIndexedOrDefault(List<int> list) =>
            list.FirstIndexedOrDefault();

        protected override int FirstIndexedOrDefault(List<int> list, int @default) =>
            list.FirstIndexedOrDefault(@default);

        protected override int FirstIndexedOrDefault(List<int> list, Func<int> defaultFactory) =>
            list.FirstIndexedOrDefault(defaultFactory);

        protected override int LastIndexed(List<int> list) =>
            list.LastIndexed();

        protected override int LastIndexedOrDefault(List<int> list) =>
            list.LastIndexedOrDefault();

        protected override int LastIndexedOrDefault(List<int> list, int @default) =>
            list.LastIndexedOrDefault(@default);

        protected override int LastIndexedOrDefault(List<int> list, Func<int> defaultFactory) =>
            list.LastIndexedOrDefault(defaultFactory);

        protected override int ElementAtIndex(List<int> list, int index) =>
            list.ElementAtIndex(index);

        protected override int ElementAtIndexOrDefault(List<int> list, int index) =>
            list.ElementAtIndexOrDefault(index);

        protected override int ElementAtIndexOrDefault(List<int> list, int index, int @default) =>
            list.ElementAtIndexOrDefault(index, @default);

        protected override int ElementAtIndexOrDefault(List<int> list, int index, Func<int> defaultFactory) =>
            list.ElementAtIndexOrDefault(index, defaultFactory);
    }
}
