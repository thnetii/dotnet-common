using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace THNETII.Common.Test
{
    public class ArgumentExtensionsTest
    {
        #region ThrowIfNull<T>(T, string) : T

        [Fact]
        public void ThrowIfNullReturnsSameReferenceIfArgumentNotNull()
        {
            var obj = new object();
            Assert.Same(obj, obj.ThrowIfNull(nameof(obj)));
        }

        [Fact]
        public void ThrowIfNullThrowsArgumentNullExceptionIfArgumentNull()
        {
            object obj = null!;
            Assert.Throws<ArgumentNullException>(nameof(obj), () => obj.ThrowIfNull(nameof(obj)));
        }

        [Fact]
        public void ThrowIfNullAcceptsNullArgumentName()
        {
            object obj = null!;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNull(null!));
        }

        [Fact]
        public void ThrowIfNullAcceptsEmptyArgumentName()
        {
            object obj = null!;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNull(string.Empty));
        }

        #endregion

        #region ThrowIfNullOrEmpty(string, string) : string

        [Fact]
        public void ThrowIfNullOrEmptyReturnsSameReferenceIfArgumentNotNullOrEmptyString()
        {
            string @string = nameof(ThrowIfNullOrEmptyReturnsSameReferenceIfArgumentNotNullOrEmptyString);
            Assert.Same(@string, @string.ThrowIfNullOrEmpty(nameof(@string)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyThrowsArgumentNullExceptionIfArgumentNullString()
        {
            string? @string = null;
            Assert.Throws<ArgumentNullException>(nameof(@string), () => @string.ThrowIfNullOrEmpty(nameof(@string)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyThrowsArgumentExceptionIfArgumentEmptyString()
        {
            string @string = string.Empty;
            Assert.Throws<ArgumentException>(nameof(@string), () => @string.ThrowIfNullOrEmpty(nameof(@string)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyStringAcceptsNullArgumentName()
        {
            string? @string = null;
            Assert.Throws<ArgumentNullException>(() => @string.ThrowIfNullOrEmpty(null!));
        }

        [Fact]
        public void ThrowIfNullOrEmptyStringAcceptsEmptyArgumentName()
        {
            string? @string = null;
            Assert.Throws<ArgumentNullException>(() => @string.ThrowIfNullOrEmpty(string.Empty));
        }

        #endregion

        #region ThrowIfNullOrEmpty<T>(T[], string) : T[]

        [Fact]
        public void ThrowIfNullOrEmptyReturnsSameReferenceIfArgumentNotNullOrEmptyArray()
        {
            var array = new[] { 24, 42 };
            Assert.Same(array, array.ThrowIfNullOrEmpty(nameof(array)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyThrowsArgumentNullExceptionIfArgumentNullArray()
        {
            int[]? array = null;
            Assert.Throws<ArgumentNullException>(nameof(array), () => array.ThrowIfNullOrEmpty(nameof(array)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyThrowsArgumentExceptionIfArgumentEmptyArray()
        {
            int[] array = Array.Empty<int>();
            Assert.Throws<ArgumentException>(nameof(array), () => array.ThrowIfNullOrEmpty(nameof(array)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyArrayAcceptsNullArgumentName()
        {
            int[]? array = null;
            Assert.Throws<ArgumentNullException>(() => array.ThrowIfNullOrEmpty(null!));
        }

        [Fact]
        public void ThrowIfNullOrEmptyArrayAcceptsEmptyArgumentName()
        {
            int[]? array = null;
            Assert.Throws<ArgumentNullException>(() => array.ThrowIfNullOrEmpty(string.Empty));
        }

        #endregion

        #region ThrowIfNullOrEmpty<T>(IEnumerable<T>, string) : IEnumerable<T>

        [Fact]
        public void ThrowIfNullOrEmptyReturnsSameReferenceIfArgumentNotNullOrEmptyEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Range(0, 10);
            Assert.Equal(enumerable, enumerable.ThrowIfNullOrEmpty(nameof(enumerable)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyThrowsArgumentNullExceptionIfArgumentNullEnumerable()
        {
            IEnumerable<int>? enumerable = null;
            Assert.Throws<ArgumentNullException>(nameof(enumerable), () => enumerable.ThrowIfNullOrEmpty(nameof(enumerable)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyThrowsArgumentExceptionIfArgumentEmptyEnumerable()
        {
            IEnumerable<int> enumerable = Enumerable.Empty<int>();
            Assert.Throws<ArgumentException>(nameof(enumerable), () => enumerable.ThrowIfNullOrEmpty(nameof(enumerable)));
        }

        [Fact]
        public void ThrowIfNullOrEmptyEnumerableAcceptsNullArgumentName()
        {
            IEnumerable<int>? enumerable = null;
            Assert.Throws<ArgumentNullException>(() => enumerable.ThrowIfNullOrEmpty(null!));
        }

        [Fact]
        public void ThrowIfNullOrEmptyEnumerableAcceptsEmptyArgumentName()
        {
            IEnumerable<int>? enumerable = null;
            Assert.Throws<ArgumentNullException>(() => enumerable.ThrowIfNullOrEmpty(string.Empty));
        }

        #endregion

        #region ThrowIfNullOrWhiteSpace(string, string) : string

        [Fact]
        public void ThrowIfNullOrWhiteSpaceReturnsSameInstanceIfStringWithContent()
        {
            var str = "test";
            Assert.Same(str, str.ThrowIfNullOrWhiteSpace(nameof(str)));
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpaceThrowsArgumentNullExceptionIfArgumentNull()
        {
            string? str = null;
            Assert.Throws<ArgumentNullException>(() => str.ThrowIfNullOrWhiteSpace(nameof(str)));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void ThrowIfNullOrWhiteSpaceThrowsArgumentNullExceptionIfArgumentInvalid(string str)
        {
            Assert.Throws<ArgumentException>(nameof(str), () => str.ThrowIfNullOrWhiteSpace(nameof(str)));
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpaceAcceptsNullArgumentName()
        {
            string? str = null;
            Assert.Throws<ArgumentNullException>(() => str.ThrowIfNullOrWhiteSpace(null!));
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpaceAcceptsEmptyArgumentName()
        {
            string? str = null;
            Assert.Throws<ArgumentNullException>(() => str.ThrowIfNullOrWhiteSpace(string.Empty));
        }

        #endregion
    }
}
