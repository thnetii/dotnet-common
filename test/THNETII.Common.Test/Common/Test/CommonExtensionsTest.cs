using Xunit;

namespace THNETII.Common.Test
{
    public class CommonExtensionsTest
    {
        [Fact]
        public void ReturnsSameObjectInstanceIfNotNull()
        {
            var instance = new object();
            var magic = new object();

            Assert.Same(instance, instance.IfNotNull(otherwise: magic));
        }

        [Fact]
        public void IfNotNullAcceptsOtherwiseNull()
        {
            var instance = new object();
            Assert.Same(instance, instance.IfNotNull(otherwise: null));
        }

        [Fact]
        public void ValueTypeIfNotNullReturnsValue()
        {
            int test = 42;
            Assert.StrictEqual(test, test.IfNotNull(default(int)));
        }

        [Fact]
        public void ReturnsSameStringInstanceIfNeitherNullNorEmpty()
        {
            string test = "TEST";
            Assert.Same(test, test.IfNotNullOrEmpty("magic"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnsOtherwiseIfStringEitherNullOrEmpty(string input)
        {
            string alternate = "ALTERNATE";
            Assert.Same(alternate, input.IfNotNullOrEmpty(otherwise: alternate));
        }

        [Fact]
        public void ReturnsSameStringInstanceIfNeitherNullNorEmptyNorWhiteSpace()
        {
            string test = "TEST";
            Assert.Same(test, test.IfNotNullOrWhiteSpace("magic"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r\n")]
        public void ReturnsOtherwiseIfStringEitherNullOrEmptyOrWhiteSpace(string input)
        {
            string alternate = "ALTERNATE";
            Assert.Same(alternate, input.IfNotNullOrWhiteSpace(otherwise: alternate));
        }
    }
}
