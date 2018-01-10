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
            Assert.Same(test, test.IfNotNull(otherwiseFactory: otherwiseFactory));
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
            Assert.Same(test, test.IfNotNullOrEmpty(otherwiseFactory: otherwiseFactory));
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
            Assert.Same(test, test.IfNotNullOrWhiteSpace(otherwiseFactory: otherwiseFactory));
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
            Assert.NotSame(test, test.IfNotNull(otherwiseFactory: otherwiseFactory));
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
            Assert.Equal(otherwiseValue, test.IfNotNullOrEmpty(otherwiseFactory: otherwiseFactory));
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
            Assert.Equal(otherwiseValue, test.IfNotNullOrWhiteSpace(otherwiseFactory: otherwiseFactory));
            Assert.True(invoked);
        }

        [Fact]
        public void ThrowsIfInstanceAndFactoryNull()
        {
            object test = null;
            Assert.ThrowsAny<ArgumentNullException>(() => test.IfNotNull(otherwiseFactory: null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ThrowsIfInstanceNullOrEmptyAndFactoryNull(string test)
        {
            Assert.ThrowsAny<ArgumentNullException>(() => test.IfNotNullOrEmpty(otherwiseFactory: null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r\n")]
        public void ThrowsIfInstanceNullOrWhitespaceAndFactoryNull(string test)
        {
            Assert.ThrowsAny<ArgumentNullException>(() => test.IfNotNullOrEmpty(otherwiseFactory: null));
        }
    }
}
