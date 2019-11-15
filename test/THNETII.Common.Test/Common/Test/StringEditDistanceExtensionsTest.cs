using Xunit;

namespace THNETII.Common.Test
{
    public class StringEditDistanceExtensionsTest
    {
        [Theory]
        [InlineData("", "", 0)]
        [InlineData("kitten", "sitting", 3)] // from Wikipedia
        [InlineData("flaw", "lawn", 2)] // from Wikipedia
        [InlineData("привет", "превет", 1)]
        [InlineData("Åge", "Age", 1)]
        [InlineData("aaaa", "×××", 4)]
        public void EditDistanceReturnsCorrectDistance(string a, string b, int distance)
            => Assert.Equal(distance, a.EditDistance(b));

        [Fact]
        public void EditDistanceAcceptsNullFirstArgument()
        {
            const string? first = null;
            const string other = "abc";
            Assert.Equal(other.Length, first.EditDistance(other));
        }

        [Fact]
        public void EditDistanceAcceptsNullSecondsArgument()
        {
            const string other = "abc";
            Assert.Equal(other.Length, other.EditDistance(null));
        }

        [Fact]
        public void EditDistanceAcceptsNullBothArguments()
            => Assert.Equal(0, StringEditDistanceExtensions.EditDistance(null, null));
    }
}
