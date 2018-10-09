using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace THNETII.Common.Test
{
    public class CommonExtensionsTest
    {
        public static IEnumerable<object[]> GetWhiteSpaceStrings()
        {
            return new[]
            {
                " ",
                "\t",
                "\r\n"
            }.Select(s => new object[] { s });
    }

        #region NotNull<T>(T, T) : T

        [Fact]
        public void NotNullReturnsSameObjectInstanceIfNotNull()
        {
            var instance = new object();
            var magic = new object();

            Assert.Same(instance, instance.NotNull(otherwise: magic));
        }

        [Fact]
        public void NotNullAcceptsOtherwiseNull()
        {
            var instance = new object();
            Assert.Same(instance, instance.NotNull(otherwise: null));
        }

        [Fact]
        public void NotNullReturnsOtherwiseIfNull()
        {
            object instance = null;
            var magic = new object();

            Assert.Same(magic, instance.NotNull(magic));
        }

        #endregion

        #region NotNull<T>(T, Func<T>) : T

        [Fact]
        public void NotNullFactoryReturnsSameObjectInstanceIfNotNull()
        {
            var instance = new object();

            Assert.Same(instance, instance.NotNull(() => new object()));
        }

        [Fact]
        public void NotNullFactoryAcceptsNullFactoryIfNotNull()
        {
            var instance = new object();

            Assert.Same(instance, instance.NotNull(otherwiseFactory: null));
        }

        [Fact]
        public void NotNullFactoryDoesNotInvokeFactoryIfNotNull()
        {
            var instance = new object();
            bool invoked = false;
            object otherwiseFactory()
            {
                invoked = true;
                return new object();
            }

            _ = instance.NotNull(otherwiseFactory);
            Assert.False(invoked);
        }

        [Fact]
        public void NotNullFactoryReturnsFactoryValueIfNull()
        {
            object instance = null;
            bool invoked = false;
            object expected = default;
            object otherwiseFactory()
            {
                invoked = true;
                expected = new object();
                return expected;
            }

            var notNull = instance.NotNull(otherwiseFactory);
            Assert.True(invoked);
            Assert.Same(expected, notNull);
        }

        #endregion

        #region NotNullOrEmpty(string, string) : string

        [Fact]
        public void NotNullOrEmptyReturnsSameStringIfNotNullString()
        {
            string str = nameof(NotNullOrEmptyReturnsSameStringIfNotNullString);

            Assert.Same(str, str.NotNullOrEmpty(nameof(str)));
        }

        [Fact]
        public void NotNullOrEmptyReturnsOtherwiseIfNullString()
        {
            string str = null;
            string otherwise = nameof(str);

            Assert.Same(otherwise, str.NotNullOrEmpty(otherwise));
        }

        [Fact]
        public void NotNullOrEmptyReturnsOtherwiseIfEmptyString()
        {
            string str = string.Empty;
            string otherwise = nameof(str);

            Assert.Same(otherwise, str.NotNullOrEmpty(otherwise));
        }

        [Fact]
        public void NotNullOrEmptyAcceptsOtherwiseNullString()
        {
            string str = nameof(NotNullOrEmptyAcceptsOtherwiseNullString);

            _ = str.NotNullOrEmpty(otherwise: null);
        }

        #endregion

        #region NotNullOrEmpty(string, Func<string>) : string

        [Fact]
        public void NotNullOrEmptyFactoryReturnsSameObjectInstanceIfNotNullString()
        {
            string str = nameof(NotNullOrEmptyFactoryReturnsSameObjectInstanceIfNotNullString);

            Assert.Same(str, str.NotNullOrEmpty(() => nameof(str)));
        }

        [Fact]
        public void NotNullOrEmptyFactoryAcceptsNullFactoryIfNotNullString()
        {
            string str = nameof(NotNullOrEmptyFactoryAcceptsNullFactoryIfNotNullString);

            Assert.Same(str, str.NotNullOrEmpty(otherwiseFactory: null));
        }

        [Fact]
        public void NotNullOrEmptyFactoryDoesNotInvokeFactoryIfNotNullString()
        {
            string str = nameof(NotNullOrEmptyFactoryDoesNotInvokeFactoryIfNotNullString);
            bool invoked = false;
            string otherwiseFactory()
            {
                invoked = true;
                return nameof(otherwiseFactory);
            }

            _ = str.NotNullOrEmpty(otherwiseFactory);
            Assert.False(invoked);
        }

        [Fact]
        public void NotNullOrEmptyFactoryReturnsFactoryValueIfNullString()
        {
            string str = null;
            bool invoked = false;
            string expected = default;
            string otherwiseFactory()
            {
                invoked = true;
                expected = nameof(otherwiseFactory);
                return expected;
            }

            var notNull = str.NotNullOrEmpty(otherwiseFactory);
            Assert.True(invoked);
            Assert.Same(expected, notNull);
        }

        #endregion

        #region NotNullOrEmpty<T>(T[], T[]) : T[]

        [Fact]
        public void NotNullOrEmptyReturnsSameStringIfNotNullArray()
        {
            int[] array = Enumerable.Range(0, 10).ToArray();

            Assert.Same(array, array.NotNullOrEmpty(Array.Empty<int>()));
        }

        [Fact]
        public void NotNullOrEmptyReturnsOtherwiseIfNullArray()
        {
            int[] array = null;
            int[] otherwise = Enumerable.Range(0, 10).ToArray();

            Assert.Same(otherwise, array.NotNullOrEmpty(otherwise));
        }

        [Fact]
        public void NotNullOrEmptyReturnsOtherwiseIfEmptyArray()
        {
            int[] array = Array.Empty<int>();
            int[] otherwise = Enumerable.Range(0, 10).ToArray();

            Assert.Same(otherwise, array.NotNullOrEmpty(otherwise));
        }

        [Fact]
        public void NotNullOrEmptyAcceptsOtherwiseNullArray()
        {
            int[] array = Enumerable.Range(0, 10).ToArray();

            _ = array.NotNullOrEmpty(otherwise: null);
        }

        #endregion

        #region NotNullOrEmpty<T>(T[], Func<T[]>) : T[]

        [Fact]
        public void NotNullOrEmptyFactoryReturnsSameObjectInstanceIfNotNullArray()
        {
            int[] array = Enumerable.Range(0, 10).ToArray();

            Assert.Same(array, array.NotNullOrEmpty(() => Array.Empty<int>()));
        }

        [Fact]
        public void NotNullOrEmptyFactoryAcceptsNullFactoryIfNotNullArray()
        {
            int[] array = Enumerable.Range(0, 10).ToArray();

            Assert.Same(array, array.NotNullOrEmpty(otherwiseFactory: null));
        }

        [Fact]
        public void NotNullOrEmptyFactoryDoesNotInvokeFactoryIfNotNullArray()
        {
            int[] array = Enumerable.Range(0, 10).ToArray();
            bool invoked = false;
            int[] otherwiseFactory()
            {
                invoked = true;
                return Array.Empty<int>();
            }

            _ = array.NotNullOrEmpty(otherwiseFactory);
            Assert.False(invoked);
        }

        [Fact]
        public void NotNullOrEmptyFactoryReturnsFactoryValueIfNullArray()
        {
            int[] array = null;
            bool invoked = false;
            int[] expected = default;
            int[] otherwiseFactory()
            {
                invoked = true;
                expected = Enumerable.Range(0, 10).ToArray();
                return expected;
            }

            var notNull = array.NotNullOrEmpty(otherwiseFactory);
            Assert.True(invoked);
            Assert.Same(expected, notNull);
        }

        #endregion

        #region NotNullOrEmpty<T>(IEnumerable<T>, IEnumerable<T>) : IEnumerable<T>

        [Fact]
        public void NotNullOrEmptyReturnsSameStringIfNotNullEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Range(0, 10);

            Assert.Equal(enumerable, enumerable.NotNullOrEmpty(Enumerable.Empty<int>()));
        }

        [Fact]
        public void NotNullOrEmptyReturnsOtherwiseIfNullEnumerable()
        {
            IEnumerable<int> enumerable = null;
            IEnumerable<int> otherwise = Enumerable.Empty<int>();

            Assert.Same(otherwise, enumerable.NotNullOrEmpty(otherwise));
        }

        [Fact]
        public void NotNullOrEmptyReturnsOtherwiseIfEmptyEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Empty<int>();
            IEnumerable<int> otherwise = Enumerable.Range(0, 10);

            Assert.Same(otherwise, enumerable.NotNullOrEmpty(otherwise));
        }

        [Fact]
        public void NotNullOrEmptyAcceptsOtherwiseNullEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Range(0, 10);

            _ = enumerable.NotNullOrEmpty(otherwise: null);
        }

        #endregion

        #region NotNullOrEmpty<T>(IEnumerable<T>, Func<IEnumerable<T>>) : IEnumerable<T>

        [Fact]
        public void NotNullOrEmptyFactoryReturnsSameObjectInstanceIfNotNullEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Range(0, 10);

            Assert.Equal(enumerable, enumerable.NotNullOrEmpty(() => Enumerable.Empty<int>()));
        }

        [Fact]
        public void NotNullOrEmptyFactoryAcceptsNullFactoryIfNotNullEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Range(0, 10);

            Assert.Equal(enumerable, enumerable.NotNullOrEmpty(otherwiseFactory: null));
        }

        [Fact]
        public void NotNullOrEmptyFactoryDoesNotInvokeFactoryIfNotNullEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Range(0, 10);
            bool invoked = false;
            IEnumerable<int> otherwiseFactory()
            {
                invoked = true;
                return Enumerable.Empty<int>();
            }

            _ = enumerable.NotNullOrEmpty(otherwiseFactory);
            Assert.False(invoked);
        }

        [Fact]
        public void NotNullOrEmptyFactoryReturnsFactoryValueIfNullEnumerable()
        {
            IEnumerable<int> enumerable = null;
            bool invoked = false;
            IEnumerable<int> expected = default;
            IEnumerable<int> otherwiseFactory()
            {
                invoked = true;
                expected = Enumerable.Range(0, 10);
                return expected;
            }

            var notNull = enumerable.NotNullOrEmpty(otherwiseFactory);
            Assert.True(invoked);
            Assert.Same(expected, notNull);
        }

        #endregion

        #region NotNullOrWhiteSpace(string, string) : string

        [Fact]
        public void NotNullOrWhiteSpaceReturnsSameStringIfNotNullString()
        {
            string str = nameof(NotNullOrWhiteSpaceReturnsSameStringIfNotNullString);

            Assert.Same(str, str.NotNullOrWhiteSpace(nameof(str)));
        }

        [Fact]
        public void NotNullOrWhiteSpaceReturnsOtherwiseIfNullString()
        {
            string str = null;
            string otherwise = nameof(str);

            Assert.Same(otherwise, str.NotNullOrWhiteSpace(otherwise));
        }

        [Fact]
        public void NotNullOrWhiteSpaceReturnsOtherwiseIfEmptyString()
        {
            string str = string.Empty;
            string otherwise = nameof(str);

            Assert.Same(otherwise, str.NotNullOrWhiteSpace(otherwise));
        }

        [Theory, MemberData(nameof(GetWhiteSpaceStrings))]
        public void NotNullOrWhiteSpaceReturnsOtherwiseIfWhiteSpaceString(string whitespace)
        {
            string otherwise = nameof(whitespace);

            Assert.Same(otherwise, whitespace.NotNullOrWhiteSpace(otherwise));
        }

        [Fact]
        public void NotNullOrWhiteSpaceAcceptsOtherwiseNullString()
        {
            string str = nameof(NotNullOrWhiteSpaceAcceptsOtherwiseNullString);

            _ = str.NotNullOrWhiteSpace(otherwise: null);
        }

        #endregion

        #region NotNullOrWhiteSpace(string, Func<string>) : string

        [Fact]
        public void NotNullOrWhiteSpaceFactoryReturnsSameObjectInstanceIfNotNullString()
        {
            string str = nameof(NotNullOrWhiteSpaceFactoryReturnsSameObjectInstanceIfNotNullString);

            Assert.Same(str, str.NotNullOrWhiteSpace(() => nameof(str)));
        }

        [Fact]
        public void NotNullOrWhiteSpaceFactoryAcceptsNullFactoryIfNotNullString()
        {
            string str = nameof(NotNullOrWhiteSpaceFactoryAcceptsNullFactoryIfNotNullString);

            Assert.Same(str, str.NotNullOrWhiteSpace(otherwiseFactory: null));
        }

        [Fact]
        public void NotNullOrWhiteSpaceFactoryDoesNotInvokeFactoryIfNotNullString()
        {
            string str = nameof(NotNullOrWhiteSpaceFactoryDoesNotInvokeFactoryIfNotNullString);
            bool invoked = false;
            string otherwiseFactory()
            {
                invoked = true;
                return nameof(otherwiseFactory);
            }

            _ = str.NotNullOrWhiteSpace(otherwiseFactory);
            Assert.False(invoked);
        }

        [Fact]
        public void NotNullOrWhiteSpaceFactoryReturnsFactoryValueIfNullString()
        {
            string str = null;
            bool invoked = false;
            string expected = default;
            string otherwiseFactory()
            {
                invoked = true;
                expected = nameof(otherwiseFactory);
                return expected;
            }

            var notNull = str.NotNullOrWhiteSpace(otherwiseFactory);
            Assert.True(invoked);
            Assert.Same(expected, notNull);
        }

        #endregion

        #region TryNotNull<T>(T, out T) : bool

        [Fact]
        public void TryNotNullReturnsTrueIfNotNull()
        {
            var instance = new object();

            Assert.True(instance.TryNotNull(out var notNull));
            Assert.NotNull(notNull);
            Assert.Same(instance, notNull);
        }

        [Fact]
        public void TryNotNullReturnsFalseIfNull()
        {
            object instance = null;

            Assert.False(instance.TryNotNull(out var _));
        }

        #endregion

        #region TryNotNullOrEmpty(string, out string) : bool

        [Fact]
        public void TryNotNullOrEmptyReturnsTrueIfNotNullString()
        {
            string str = nameof(str);

            Assert.True(str.TryNotNullOrEmpty(out var notNull));
            Assert.NotNull(notNull);
            Assert.Same(str, notNull);
        }

        [Fact]
        public void TryNotNullOrEmptyReturnsFalseIfNullString()
        {
            string str = null;

            Assert.False(str.TryNotNullOrEmpty(out var _));
        }

        [Fact]
        public void TryNotNullOrEmptyReturnsFalseIfEmptyString()
        {
            string str = string.Empty;

            Assert.False(str.TryNotNullOrEmpty(out var _));
        }

        #endregion

        #region TryNotNullOrEmpty<T>(T[], out T[]) : bool

        [Fact]
        public void TryNotNullOrEmptyReturnsTrueIfNotNullArray()
        {
            int[] array = Enumerable.Range(0, 10).ToArray();

            Assert.True(array.TryNotNullOrEmpty(out var notNull));
            Assert.NotNull(notNull);
            Assert.Same(array, notNull);
        }

        [Fact]
        public void TryNotNullOrEmptyReturnsFalseIfNullArray()
        {
            int[] array = null;

            Assert.False(array.TryNotNullOrEmpty(out var _));
        }

        [Fact]
        public void TryNotNullOrEmptyReturnsFalseIfEmptyArray()
        {
            int[] array = Array.Empty<int>();

            Assert.False(array.TryNotNullOrEmpty(out var _));
        }

        #endregion

        #region TryNotNullOrEmpty<T>(IEnumerable<T>, out IEnumerable<T>) : bool

        [Fact]
        public void TryNotNullOrEmptyReturnsTrueIfNotNullEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Range(0, 10);

            Assert.True(enumerable.TryNotNullOrEmpty(out var notNull));
            Assert.NotNull(notNull);
            Assert.Equal(enumerable, notNull);
        }

        [Fact]
        public void TryNotNullOrEmptyReturnsFalseIfNullEnumerable()
        {
            IEnumerable<int> enumerable = null;

            Assert.False(enumerable.TryNotNullOrEmpty(out var _));
        }

        [Fact]
        public void TryNotNullOrEmptyReturnsFalseIfEmptyEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Empty<int>();

            Assert.False(enumerable.TryNotNullOrEmpty(out var _));
        }

        #endregion

        #region TryNotNullOrWhiteSpace(string, out string) : bool

        [Fact]
        public void TryNotNullOrWhiteSpaceReturnsTrueIfNotNullString()
        {
            string str = nameof(str);

            Assert.True(str.TryNotNullOrWhiteSpace(out var notNull));
            Assert.NotNull(notNull);
            Assert.Same(str, notNull);
        }

        [Fact]
        public void TryNotNullOrWhiteSpaceReturnsFalseIfNullString()
        {
            string str = null;

            Assert.False(str.TryNotNullOrWhiteSpace(out var _));
        }

        [Theory, MemberData(nameof(GetWhiteSpaceStrings))]
        public void TryNotNullOrWhiteSpaceReturnsFalseIfWhiteSpaceString(string whitespace)
        {
            Assert.False(whitespace.TryNotNullOrWhiteSpace(out var _));
        }

        #endregion

        #region ZeroLengthIfNull<T>(T[]) : T[]

        [Fact]
        public void ZeroLengthIfNullReturnsSameInstanceIfNotNull()
        {
            int[] array = Enumerable.Range(0, 10).ToArray();

            Assert.Same(array, array.ZeroLengthIfNull());
        }

        [Fact]
        public void ZeroLengthIfNullReturnsEmptyIfNull()
        {
            int[] array = null;

            Assert.Empty(array.ZeroLengthIfNull());
        }

        #endregion

        #region EmptyIfNull<T>(IEnumerable<T>) : IEnumerable<T>

        [Fact]
        public void EmptyIfNullReturnsSameInstanceIfNotNull()
        {
            var enumerable = Enumerable.Range(0, 10);

            Assert.Same(enumerable, enumerable.EmptyIfNull());
        }

        [Fact]
        public void EmptyIfNullReturnsEmptyIfNull()
        {
            IEnumerable<int> enumerable = null;

            Assert.Empty(enumerable.EmptyIfNull());
        }

        #endregion
    }
}
