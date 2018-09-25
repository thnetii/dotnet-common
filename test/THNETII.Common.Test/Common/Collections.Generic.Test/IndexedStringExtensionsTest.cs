using System;

namespace THNETII.Common.Collections.Generic.Test
{
    public class IndexedStringExtensionsTest : IndexedCollectionExtensionsTest<string, char>
    {
        protected override string GetEmpty() => string.Empty;
        protected override string GetNonEmpty() => "abcdefghijklmnopqrstuvwxyz";

        protected override char GetNonDefaultT() => 'Z';

        protected override char FirstIndexed(string enumerable) =>
            enumerable.FirstIndexed();

        protected override char FirstIndexedOrDefault(string enumerable) =>
            enumerable.FirstIndexedOrDefault();

        protected override char FirstIndexedOrDefault(string enumerable, char @default) =>
            enumerable.FirstIndexedOrDefault(@default);

        protected override char FirstIndexedOrDefault(string enumerable, Func<char> defaultFactory) =>
            enumerable.FirstIndexedOrDefault(defaultFactory);

        protected override char LastIndexed(string enumerable) =>
            enumerable.LastIndexed();

        protected override char LastIndexedOrDefault(string enumerable) =>
            enumerable.LastIndexedOrDefault();

        protected override char LastIndexedOrDefault(string enumerable, char @default) =>
            enumerable.LastIndexedOrDefault(@default);

        protected override char LastIndexedOrDefault(string enumerable, Func<char> defaultFactory) =>
            enumerable.LastIndexedOrDefault(defaultFactory);

        protected override char ElementAtIndex(string enumerable, int index) =>
            enumerable.ElementAtIndex(index);

        protected override char ElementAtIndexOrDefault(string enumerable, int index) =>
            enumerable.ElementAtIndexOrDefault(index);

        protected override char ElementAtIndexOrDefault(string enumerable, int index, char @default) =>
            enumerable.ElementAtIndexOrDefault(index, @default);

        protected override char ElementAtIndexOrDefault(string enumerable, int index, Func<char> defaultFactory) =>
            enumerable.ElementAtIndexOrDefault(index, defaultFactory);
    }
}
