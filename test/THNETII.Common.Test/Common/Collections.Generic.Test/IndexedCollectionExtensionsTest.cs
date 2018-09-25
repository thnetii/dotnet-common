using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace THNETII.Common.Collections.Generic.Test
{
    public abstract class IndexedCollectionExtensionsTest<TEnumerable, T>
        where TEnumerable : class, IEnumerable<T>
    {
        protected abstract TEnumerable GetEmpty();
        protected abstract TEnumerable GetNonEmpty();

        protected abstract T GetNonDefaultT();

        protected abstract T FirstIndexed(TEnumerable enumerable);
        protected abstract T FirstIndexedOrDefault(TEnumerable enumerable);
        protected abstract T FirstIndexedOrDefault(TEnumerable enumerable, T @default);
        protected abstract T FirstIndexedOrDefault(TEnumerable enumerable, Func<T> @defaultFactory);

        protected abstract T LastIndexed(TEnumerable enumerable);
        protected abstract T LastIndexedOrDefault(TEnumerable enumerable);
        protected abstract T LastIndexedOrDefault(TEnumerable enumerable, T @default);
        protected abstract T LastIndexedOrDefault(TEnumerable enumerable, Func<T> defaultFactory);

        protected abstract T ElementAtIndex(TEnumerable enumerable, int index);
        protected abstract T ElementAtIndexOrDefault(TEnumerable enumerable, int index);
        protected abstract T ElementAtIndexOrDefault(TEnumerable enumerable, int index, T @default);
        protected abstract T ElementAtIndexOrDefault(TEnumerable enumerable, int index, Func<T> defaultFactory);

        protected virtual (TEnumerable enumerable, int index) GetNonEmptyWithMiddleIndex()
        {
            var e = GetNonEmpty();
            int c = e.Count();
            return (e, (c / 2) - 1);
        }

        #region First
        [Fact]
        public void FirstIndexedOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => FirstIndexed(null));

        [Fact]
        public void FirstIndexedOfEmptyThrows() =>
            Assert.Throws<InvalidOperationException>(() => FirstIndexed(GetEmpty()));

        [Fact]
        public void FirstIndexedReturnsFirstT()
        {
            var test = GetNonEmpty();
            Assert.Equal(test.First(), FirstIndexed(test));
        }

        [Fact]
        public void FirstIndexOrDefaultOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => FirstIndexedOrDefault(null));

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
            Assert.Equal(expected, FirstIndexedOrDefault(GetEmpty(), () => expected));
        }

        [Fact]
        public void FirstIndexedOrDefaultReturnsFirstT()
        {
            var test = GetNonEmpty();
            Assert.Equal(test.First(), FirstIndexedOrDefault(test));
        } 
        #endregion

        #region Last
        [Fact]
        public void LastIndexedOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => LastIndexed(null));

        [Fact]
        public void LastIndexedOfEmptyThrows() =>
            Assert.Throws<InvalidOperationException>(() => LastIndexed(GetEmpty()));

        [Fact]
        public void LastIndexedReturnsLastT()
        {
            var test = GetNonEmpty();
            Assert.Equal(test.Last(), LastIndexed(test));
        }

        [Fact]
        public void LastIndexOrDefaultOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => LastIndexedOrDefault(null));

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
            Assert.Equal(expected, LastIndexedOrDefault(GetEmpty(), () => expected));
        }

        [Fact]
        public void LastIndexedOrDefaultReturnsLastT()
        {
            var test = GetNonEmpty();
            Assert.Equal(test.Last(), LastIndexedOrDefault(test));
        }
        #endregion

        #region AtIndex
        [Fact]
        public void ElementAtIndexOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => ElementAtIndex(null, default));

        [Fact]
        public void ElementAtIndexOfEmptyThrows() =>
            Assert.Throws<InvalidOperationException>(() => ElementAtIndex(GetEmpty(), default));

        [Fact]
        public void ElementAtIndexReturnsElementAtT()
        {
            var (test, index) = GetNonEmptyWithMiddleIndex();
            Assert.Equal(test.ElementAt(index), ElementAtIndex(test, index));
        }

        [Fact]
        public void ElementAtIndexOrDefaultOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => ElementAtIndexOrDefault(null, default));

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
            Assert.Equal(expected, ElementAtIndexOrDefault(GetEmpty(), default, () => expected));
        }

        [Fact]
        public void ElementAtIndexOrDefaultReturnsElementAtT()
        {
            var (test, index) = GetNonEmptyWithMiddleIndex();
            Assert.Equal(test.ElementAt(index), ElementAtIndexOrDefault(test, index));
        }
        #endregion
    }
}
