using System;
using System.Linq;

namespace THNETII.Common.Collections.Generic.Test
{
    public class IndexedArrayExtensionsTest : IndexedCollectionExtensionsTest<int[], int>
    {
        protected override int[] GetEmpty() => Array.Empty<int>();
        protected override int[] GetNonEmpty() => Enumerable.Range(0, 10).ToArray();

        protected override int GetNonDefaultT() => 42;

        protected override int FirstIndexed(int[] array) =>
            array.FirstIndexed();

        protected override int FirstIndexedOrDefault(int[] array) =>
            array.FirstIndexedOrDefault();

        protected override int FirstIndexedOrDefault(int[] array, int @default) =>
            array.FirstIndexedOrDefault(@default);

        protected override int FirstIndexedOrDefault(int[] array, Func<int> defaultFactory) =>
            array.FirstIndexedOrDefault(defaultFactory);

        protected override int LastIndexed(int[] array) =>
            array.LastIndexed();

        protected override int LastIndexedOrDefault(int[] array) =>
            array.LastIndexedOrDefault();

        protected override int LastIndexedOrDefault(int[] array, int @default) =>
            array.LastIndexedOrDefault(@default);

        protected override int LastIndexedOrDefault(int[] array, Func<int> defaultFactory) =>
            array.LastIndexedOrDefault(defaultFactory);

        protected override int ElementAtIndex(int[] array, int index) =>
            array.ElementAtIndex(index);

        protected override int ElementAtIndexOrDefault(int[] array, int index) =>
            array.ElementAtIndexOrDefault(index);

        protected override int ElementAtIndexOrDefault(int[] array, int index, int @default) =>
            array.ElementAtIndexOrDefault(index, @default);

        protected override int ElementAtIndexOrDefault(int[] array, int index, Func<int> defaultFactory) =>
            array.ElementAtIndexOrDefault(index, defaultFactory);
    }
}
