using System;
using System.Globalization;
using System.Linq;
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

        [Fact]
        public void EnumerateLinesOfNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).EnumerateLines());
        }

        [Fact]
        public void EnumerateLinesOfEmptyReturnsEmpty()
        {
            Assert.Empty(string.Empty.EnumerateLines());
        }

        [Theory]
        [InlineData("\r")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void EnumerateLinesUsesAnyLineSeparator(string newline)
        {
            var test = Enumerable.Range(0, 10)
                .Select(i => i.ToString(CultureInfo.InvariantCulture))
                .ToArray();
            var multiLine = string.Join(newline, test);
            Assert.Equal(test, multiLine.EnumerateLines());
        }
    }
}
