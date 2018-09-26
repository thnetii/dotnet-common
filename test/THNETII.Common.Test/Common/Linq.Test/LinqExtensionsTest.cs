using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace THNETII.Common.Linq.Test
{
    public abstract class LinqExtensionsTest<T>
    {
        protected abstract object GetEmpty();
        protected abstract object GetMoreThan5ButLessThan100();

        protected abstract T GetNonDefaultT();

        protected virtual T PrimitiveFirst(object source) =>
            (source as IEnumerable<T>).First();
        protected virtual T PrimitiveLast(object source) =>
            (source as IEnumerable<T>).Last();
        protected virtual T PrimitiveElementAt(object source, int index) =>
            (source as IEnumerable<T>).ElementAt(index);

        protected abstract T First(object source);
        protected abstract T FirstOrDefault(object source);
        protected abstract T FirstOrDefault(object source, T @default);
        protected abstract T FirstOrDefault(object source, Func<T> @defaultFactory);

        protected abstract T Last(object source);
        protected abstract T LastOrDefault(object source);
        protected abstract T LastOrDefault(object source, T @default);
        protected abstract T LastOrDefault(object source, Func<T> defaultFactory);

        protected abstract T ElementAt(object source, int index);
        protected abstract T ElementAtOrDefault(object source, int index);
        protected abstract T ElementAtOrDefault(object source, int index, T @default);
        protected abstract T ElementAtOrDefault(object source, int index, Func<T> defaultFactory);

        #region First
        [SkippableFact]
        public void FirstOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => First(null));

        [SkippableFact]
        public void FirstOfEmptyThrows() =>
            Assert.ThrowsAny<Exception>(() => First(GetEmpty()));

        [SkippableFact]
        public void FirstReturnsFirstT()
        {
            var test = GetMoreThan5ButLessThan100();
            Assert.Equal(PrimitiveFirst(test), First(test));
        }

        [SkippableFact]
        public void FirstOrDefaultOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => FirstOrDefault(null));

        [SkippableFact]
        public void FirstOrDefaultOfEmptyReturnsDefaultT() =>
            Assert.Equal(default, FirstOrDefault(GetEmpty()));

        [SkippableFact]
        public void FirstOrDefaultOfEmptyReturnsArgument()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, FirstOrDefault(GetEmpty(), expected));
        }

        [SkippableFact]
        public void FirstOrDefaultOfEmptyReturnsFactoryValue()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, FirstOrDefault(GetEmpty(), () => expected));
        }

        [SkippableFact]
        public void FirstOrDefaultReturnsFirstT()
        {
            var test = GetMoreThan5ButLessThan100();
            Assert.Equal(PrimitiveFirst(test), FirstOrDefault(test));
        }

        [SkippableFact]
        public void FirstOrDefaultIgnoresArgumentIfNonEmpty()
        {
            var test = GetMoreThan5ButLessThan100();
            var ignore = GetNonDefaultT();
            Assert.Equal(PrimitiveFirst(test), FirstOrDefault(test, ignore));
        }

        [SkippableFact]
        public void FirstOrDefaultIgnoresFactoryIfNonEmpty()
        {
            var test = GetMoreThan5ButLessThan100();
            T ignoredFactory() => throw new InvalidOperationException("The factory should never be invoked");
            Assert.Equal(PrimitiveFirst(test), FirstOrDefault(test, ignoredFactory));
        }
        #endregion

        #region Last
        [SkippableFact]
        public void LastOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => Last(null));

        [SkippableFact]
        public void LastOfEmptyThrows() =>
            Assert.ThrowsAny<Exception>(() => Last(GetEmpty()));

        [SkippableFact]
        public void LastReturnsLastT()
        {
            var test = GetMoreThan5ButLessThan100();
            Assert.Equal(PrimitiveLast(test), Last(test));
        }

        [SkippableFact]
        public void LastOrDefaultOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => LastOrDefault(null));

        [SkippableFact]
        public void LastOrDefaultOfEmptyReturnsDefaultT() =>
            Assert.Equal(default, LastOrDefault(GetEmpty()));

        [SkippableFact]
        public void LastOrDefaultOfEmptyReturnsArgument()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, LastOrDefault(GetEmpty(), expected));
        }

        [SkippableFact]
        public void LastOrDefaultOfEmptyReturnsFactoryValue()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, LastOrDefault(GetEmpty(), () => expected));
        }

        [SkippableFact]
        public void LastOrDefaultReturnsLastT()
        {
            var test = GetMoreThan5ButLessThan100();
            Assert.Equal(PrimitiveLast(test), LastOrDefault(test));
        }

        [SkippableFact]
        public void LastOrDefaultIgnoresArgumentIfNonEmpty()
        {
            var test = GetMoreThan5ButLessThan100();
            var ignore = GetNonDefaultT();
            Assert.Equal(PrimitiveLast(test), LastOrDefault(test, ignore));
        }

        [SkippableFact]
        public void LastOrDefaultIgnoresFactoryIfNonEmpty()
        {
            var test = GetMoreThan5ButLessThan100();
            T ignoredFactory() => throw new InvalidOperationException("The factory should never be invoked");
            Assert.Equal(PrimitiveLast(test), LastOrDefault(test, ignoredFactory));
        }
        #endregion

        #region At
        [SkippableFact]
        public void ElementAtOfNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                try { ElementAt(null, default); }
                catch (SkipException)
                { throw new ArgumentNullException(); }
            });
        }

        [SkippableFact]
        public void ElementAtOfEmptyThrows() =>
            Assert.ThrowsAny<Exception>(() => ElementAt(GetEmpty(), default));

        [SkippableFact]
        public void ElementAtReturnsElementAtT()
        {
            const int index = 5;
            var test = GetMoreThan5ButLessThan100();
            Assert.Equal(PrimitiveElementAt(test, index), ElementAt(test, index));
        }

        [SkippableFact]
        public void ElementAtOrDefaultOfNullThrows() =>
            Assert.Throws<ArgumentNullException>(() => ElementAtOrDefault(null, default));

        [SkippableFact]
        public void ElementAtOrDefaultOfEmptyReturnsDefaultT() =>
            Assert.Equal(default, ElementAtOrDefault(GetEmpty(), default));

        [SkippableFact]
        public void ElementAtOrDefaultOfEmptyReturnsArgument()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, ElementAtOrDefault(GetEmpty(), default, expected));
        }

        [SkippableFact]
        public void ElementAtOrDefaultOfEmptyReturnsFactoryValue()
        {
            var expected = GetNonDefaultT();
            Assert.Equal(expected, ElementAtOrDefault(GetEmpty(), default, () => expected));
        }

        [SkippableFact]
        public void ElementAtOrDefaultReturnsElementAtT()
        {
            const int index = 5;
            var test = GetMoreThan5ButLessThan100();
            Assert.Equal(PrimitiveElementAt(test, index), ElementAtOrDefault(test, index));
        }

        [SkippableFact]
        public void ElementAtWithNegativeThrows()
        {
            var test = GetMoreThan5ButLessThan100();
            Assert.ThrowsAny<Exception>(() => ElementAt(test, -1));
        }

        [SkippableFact]
        public void ElementAtOrDefaultWithNegativeReturnsDefault()
        {
            var test = GetMoreThan5ButLessThan100();
            T expected = default;
            Assert.Equal(expected, ElementAtOrDefault(test, -1));
        }

        [SkippableFact]
        public void ElementAtOrDefaultWithNegativeReturnsArgument()
        {
            var test = GetMoreThan5ButLessThan100();
            T expected = GetNonDefaultT();
            Assert.Equal(expected, ElementAtOrDefault(test, -1, expected));
        }

        [SkippableFact]
        public void ElementAtOrDefaultWithNegativeReturnsFactoryValue()
        {
            var test = GetMoreThan5ButLessThan100();
            T expected = GetNonDefaultT();
            Assert.Equal(expected, ElementAtOrDefault(test, -1, () => expected));
        }

        [SkippableFact]
        public void ElementAtOrDefaultIgnoresArgumentIfNonEmpty()
        {
            const int index = 5;
            var test = GetMoreThan5ButLessThan100();
            var ignore = GetNonDefaultT();
            Assert.Equal(PrimitiveElementAt(test, index), ElementAtOrDefault(test, index, ignore));
        }

        [SkippableFact]
        public void ElementAtOrDefaultIgnoresFactoryIfNonEmpty()
        {
            const int index = 5;
            var test = GetMoreThan5ButLessThan100();
            T ignoredFactory() => throw new InvalidOperationException("The factory should never be invoked");
            Assert.Equal(PrimitiveElementAt(test, index), ElementAtOrDefault(test, index, ignoredFactory));
        }

        [SkippableFact]
        public void ElementAtWithTooBigIndexThrows()
        {
            const int index = 100;
            var test = GetMoreThan5ButLessThan100();
            Assert.ThrowsAny<Exception>(() => ElementAt(test, index));
        }

        [SkippableFact]
        public void ElementAtOrDefaultWithTooBigIndexReturnsDefault()
        {
            const int index = 100;
            var test = GetMoreThan5ButLessThan100();
            Assert.Equal(default, ElementAtOrDefault(test, index));
        }

        [SkippableFact]
        public void ElementAtOrDefaultWithTooBigIndexReturnsArgument()
        {
            const int index = 100;
            var test = GetMoreThan5ButLessThan100();
            var expected = GetNonDefaultT();
            Assert.Equal(expected, ElementAtOrDefault(test, index, expected));
        }

        [SkippableFact]
        public void ElementAtOrDefaultWithTooBigIndexReturnsFactoryValue()
        {
            const int index = 100;
            var test = GetMoreThan5ButLessThan100();
            var expected = GetNonDefaultT();
            Assert.Equal(expected, ElementAtOrDefault(test, index, () => expected));
        }
        #endregion
    }

    public abstract class LinqExtensionsWithIntsTest : LinqExtensionsTest<int>
    {
        protected override int GetNonDefaultT() => 42;
    }

    public abstract class LinqExtensionsWithCharsTest : LinqExtensionsTest<char>
    {
        protected override char GetNonDefaultT() => 'Z';
    }
}
