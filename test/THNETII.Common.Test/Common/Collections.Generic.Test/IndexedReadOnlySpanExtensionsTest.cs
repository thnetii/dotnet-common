﻿using System;
using System.Linq;
using Xunit;

namespace THNETII.Common.Collections.Generic.Test
{
    public class IndexedReadOnlySpanExtensionsTest
    {
        protected ReadOnlySpan<int> GetEmpty() => ReadOnlySpan<int>.Empty;
        protected ReadOnlySpan<int> GetNonEmpty() => Enumerable.Range(0, 10).ToArray();

        protected readonly int NonDefaultValue = 42;

        protected int GetNonDefaultT() => NonDefaultValue;

        protected int FirstIndexed(ReadOnlySpan<int> span) =>
            span.FirstIndexed();

        protected int FirstIndexedOrDefault(ReadOnlySpan<int> span) =>
            span.FirstIndexedOrDefault();

        protected int FirstIndexedOrDefault(ReadOnlySpan<int> span, int @default) =>
            span.FirstIndexedOrDefault(@default);

        protected int FirstIndexedOrDefault(ReadOnlySpan<int> span, RefReadOnlyFunc<int> defaultFactory) =>
            span.FirstIndexedOrDefault(defaultFactory);

        protected int LastIndexed(ReadOnlySpan<int> span) =>
            span.LastIndexed();

        protected int LastIndexedOrDefault(ReadOnlySpan<int> span) =>
            span.LastIndexedOrDefault();

        protected int LastIndexedOrDefault(ReadOnlySpan<int> span, int @default) =>
            span.LastIndexedOrDefault(@default);

        protected int LastIndexedOrDefault(ReadOnlySpan<int> span, RefReadOnlyFunc<int> defaultFactory) =>
            span.LastIndexedOrDefault(defaultFactory);

        protected int ElementAtIndex(ReadOnlySpan<int> span, int index) =>
            span.ElementAtIndex(index);

        protected int ElementAtIndexOrDefault(ReadOnlySpan<int> span, int index) =>
            span.ElementAtIndexOrDefault(index);

        protected int ElementAtIndexOrDefault(ReadOnlySpan<int> span, int index, int @default) =>
            span.ElementAtIndexOrDefault(index, @default);

        protected int ElementAtIndexOrDefault(ReadOnlySpan<int> span, int index, RefReadOnlyFunc<int> defaultFactory) =>
            span.ElementAtIndexOrDefault(index, defaultFactory);

        protected virtual int GetNonEmptyWithMiddleIndex(ReadOnlySpan<int> span) =>
            (span.Length / 2) - 1;

        #region First
        [Fact]
        public void FirstIndexedOfEmptyThrows() =>
            Assert.ThrowsAny<Exception>(() => FirstIndexed(GetEmpty()));

        [Fact]
        public void FirstIndexedReturnsFirstT()
        {
            var test = GetNonEmpty();
            Assert.Equal(test[0], FirstIndexed(test));
        }

        [Fact]
        public void FirstIndexOrDefaultOfEmptyReturnsDefaultT() =>
            Assert.Equal(default, FirstIndexedOrDefault(GetEmpty()));

        [Fact]
        public void FirstIndexOrDefaultOfEmptyReturnsArgument()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, FirstIndexedOrDefault(GetEmpty(), expected));
        }

        [Fact]
        public void FirstIndexOrDefaultOfEmptyReturnsFactoryValue()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, FirstIndexedOrDefault(GetEmpty(), () => ref NonDefaultValue));
        }

        [Fact]
        public void FirstIndexedOrDefaultReturnsFirstT()
        {
            var test = GetNonEmpty();
            Assert.Equal(test[0], FirstIndexedOrDefault(test));
        }
        #endregion

        #region Last
        [Fact]
        public void LastIndexedOfEmptyThrows() =>
            Assert.ThrowsAny<Exception>(() => LastIndexed(GetEmpty()));

        [Fact]
        public void LastIndexedReturnsLastT()
        {
            var test = GetNonEmpty();
            Assert.Equal(test[test.Length - 1], LastIndexed(test));
        }

        [Fact]
        public void LastIndexOrDefaultOfEmptyReturnsDefaultT() =>
            Assert.Equal(default, LastIndexedOrDefault(GetEmpty()));

        [Fact]
        public void LastIndexOrDefaultOfEmptyReturnsArgument()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, LastIndexedOrDefault(GetEmpty(), expected));
        }

        [Fact]
        public void LastIndexOrDefaultOfEmptyReturnsFactoryValue()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, LastIndexedOrDefault(GetEmpty(), () => ref NonDefaultValue));
        }

        [Fact]
        public void LastIndexedOrDefaultReturnsLastT()
        {
            var test = GetNonEmpty();
            Assert.Equal(test[test.Length - 1], LastIndexedOrDefault(test));
        }
        #endregion

        #region AtIndex
        [Fact]
        public void ElementAtIndexOfEmptyThrows() =>
            Assert.ThrowsAny<Exception>(() => ElementAtIndex(GetEmpty(), default));

        [Fact]
        public void ElementAtIndexReturnsElementAtT()
        {
            var test = GetNonEmpty();
            var index = GetNonEmptyWithMiddleIndex(test);
            Assert.Equal(test[index], ElementAtIndex(test, index));
        }

        [Fact]
        public void ElementAtIndexOrDefaultOfEmptyReturnsDefaultT() =>
            Assert.Equal(default, ElementAtIndexOrDefault(GetEmpty(), default));

        [Fact]
        public void ElementAtIndexOrDefaultOfEmptyReturnsArgument()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, ElementAtIndexOrDefault(GetEmpty(), default, expected));
        }

        [Fact]
        public void ElementAtIndexOrDefaultOfEmptyReturnsFactoryValue()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, ElementAtIndexOrDefault(GetEmpty(), default, () => ref NonDefaultValue));
        }

        [Fact]
        public void ElementAtIndexOrDefaultReturnsElementAtT()
        {
            var test = GetNonEmpty();
            var index = GetNonEmptyWithMiddleIndex(test);
            Assert.Equal(test[index], ElementAtIndexOrDefault(test, index));
        }
        #endregion
    }
}
