using System;
using Xunit;

namespace THNETII.Common.Test
{
    public class StringCommonExtensionsTest
    {
        [Fact]
        public void ContainsThrowsWithNullSource()
        {
            const string test = null;
            Assert.Throws<ArgumentNullException>(() => test.Contains("test", default));
        }

        [Fact]
        public void ContainsThrowsWithNullValue()
        {
            const string test = "test";
            Assert.Throws<ArgumentNullException>(() => test.Contains(null, default));
        }

        [Fact]
        public void ContainsUsingOrdinalIgnoreCase()
        {
            const string source = "test";
            string contains = source.ToUpperInvariant();

            Assert.True(source.Contains(contains, StringComparison.OrdinalIgnoreCase));
        }
    }
}
