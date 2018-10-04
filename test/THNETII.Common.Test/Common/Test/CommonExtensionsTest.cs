using System;
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

            Assert.Same(instance, instance.NotNull(otherwise: magic));
        }

        [Fact]
        public void IfNotNullAcceptsOtherwiseNull()
        {
            var instance = new object();
            Assert.Same(instance, instance.NotNull(otherwise: null));
        }

        [Fact]
        public void ReturnsSameStringInstanceIfNeitherNullNorEmpty()
        {
            string test = "TEST";
            Assert.Same(test, test.NotNullOrEmpty("magic"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ReturnsOtherwiseIfStringEitherNullOrEmpty(string input)
        {
            string alternate = "ALTERNATE";
            Assert.Same(alternate, input.NotNullOrEmpty(otherwise: alternate));
        }

        [Fact]
        public void ReturnsSameStringInstanceIfNeitherNullNorEmptyNorWhiteSpace()
        {
            string test = "TEST";
            Assert.Same(test, test.NotNullOrWhiteSpace("magic"));
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
            Assert.Same(alternate, input.NotNullOrWhiteSpace(otherwise: alternate));
        }

        [Fact]
        public void DoesNotInvokeOtherwiseIfInstanceNotNull()
        {
            object test = new object();
            bool invoked = false;
            object otherwiseFactory()
            {
                invoked = true;
                return new object();
            }
            Assert.Same(test, test.NotNull(otherwiseFactory: otherwiseFactory));
            Assert.False(invoked);
        }

        [Fact]
        public void DoesNotInvokeOtherwiseIfStringNotEmpty()
        {
            string test = "TEST";
            bool invoked = false;
            string otherwiseFactory()
            {
                invoked = true;
                return "OTHERWISE";
            }
            Assert.Same(test, test.NotNullOrEmpty(otherwiseFactory: otherwiseFactory));
            Assert.False(invoked);
        }

        [Fact]
        public void DoesNotInvokeOtherwiseIfStringNotEmptyOrWhiteSpace()
        {
            string test = "TEST";
            bool invoked = false;
            string otherwiseFactory()
            {
                invoked = true;
                return "OTHERWISE";
            }
            Assert.Same(test, test.NotNullOrWhiteSpace(otherwiseFactory: otherwiseFactory));
            Assert.False(invoked);
        }

        [Fact]
        public void InvokesOtherwiseIfInstanceNull()
        {
            bool invoked = false;
            object otherwiseFactory()
            {
                invoked = true;
                return new object();
            }
            object test = null;
            Assert.NotSame(test, test.NotNull(otherwiseFactory: otherwiseFactory));
            Assert.True(invoked);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void InvokesOtherwiseIfStringNullOrEmpty(string test)
        {
            bool invoked = false;
            const string otherwiseValue = "OTHERWISE";
            string otherwiseFactory()
            {
                invoked = true;
                return otherwiseValue;
            }
            Assert.Equal(otherwiseValue, test.NotNullOrEmpty(otherwiseFactory: otherwiseFactory));
            Assert.True(invoked);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r\n")]
        public void InvokesOtherwiseIfStringNullOrWhiteSpace(string test)
        {
            bool invoked = false;
            const string otherwiseValue = "OTHERWISE";
            string otherwiseFactory()
            {
                invoked = true;
                return otherwiseValue;
            }
            Assert.Equal(otherwiseValue, test.NotNullOrWhiteSpace(otherwiseFactory: otherwiseFactory));
            Assert.True(invoked);
        }

        [Fact]
        public void ThrowsIfInstanceAndFactoryNull()
        {
            object test = null;
            Assert.ThrowsAny<ArgumentNullException>(() => test.NotNull(otherwiseFactory: null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ThrowsIfInstanceNullOrEmptyAndFactoryNull(string test)
        {
            Assert.ThrowsAny<ArgumentNullException>(() => test.NotNullOrEmpty(otherwiseFactory: null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r\n")]
        public void ThrowsIfInstanceNullOrWhitespaceAndFactoryNull(string test)
        {
            Assert.ThrowsAny<ArgumentNullException>(() => test.NotNullOrWhiteSpace(otherwiseFactory: null));
        }
    }
}
