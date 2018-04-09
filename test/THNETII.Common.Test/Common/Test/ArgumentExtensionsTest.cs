using System;
using Xunit;

namespace THNETII.Common.Test
{
    public class ArgumentExtensionsTest
    {
        [Fact]
        public void ThrowIfNullReturnsSameReferenceIfArgumentNotNull()
        {
            var obj = new object();
            Assert.Same(obj, obj.ThrowIfNull(nameof(obj)));
        }

        [Fact]
        public void ThrowIfNullThrowsArgumentNullExceptionIfArgumentNull()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(nameof(obj), () => obj.ThrowIfNull(nameof(obj)));
        }

        [Fact]
        public void ThrowIfNullAcceptsNullArgumentName()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNull(null));
        }

        [Fact]
        public void ThrowIfNullAcceptsEmptyArgumentName()
        {
            object obj = null;
            Assert.Throws<ArgumentNullException>(() => obj.ThrowIfNull(string.Empty));
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpaceReturnsSameInstanceIfStringWithContent()
        {
            var str = "test";
            Assert.Same(str, str.ThrowIfNullOrWhiteSpace(nameof(str)));
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpaceThrowsArgumentNullExceptionIfArgumentNull()
        {
            string str = null;
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
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ThrowIfNullOrWhiteSpace(null));
        }

        [Fact]
        public void ThrowIfNullOrWhiteSpaceAcceptsEmptyArgumentName()
        {
            string str = null;
            Assert.Throws<ArgumentNullException>(() => str.ThrowIfNullOrWhiteSpace(string.Empty));
        }
    }
}
