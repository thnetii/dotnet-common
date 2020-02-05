using System.Text;

using Xunit;

namespace THNETII.Common.Collections.Generic.Test
{
    public class ReferenceEqualityComparerTest
    {
        [Fact]
        public void StringReferenceEqualityComparerDefaultIsNotNull() =>
            Assert.NotNull(ReferenceEqualityComparer.Instance);

        [Fact]
        public void SameInstanceStringStaticReferenceEqualityComparisonReturnsTrue()
        {
            const string src = "Test";
            var p1 = src;
            var p2 = src;

            Assert.True(ReferenceEqualityComparer.StaticEquals(p1, p2));
        }

        [Fact]
        public void DifferentInstanceSameValueStringStaticReferenceEqualityComparisonReturnsFalse()
        {
            const string src = "Test";
            var p1 = new StringBuilder(src).ToString();
            var p2 = new StringBuilder(src).ToString();

            Assert.False(ReferenceEqualityComparer.StaticEquals(p1, p2));
        }

        [Fact]
        public void DifferentValueStringStaticReferenceEqualityComparisonReturnsFalse()
        {
            var p1 = "Test1";
            var p2 = "Test2";

            Assert.False(ReferenceEqualityComparer.StaticEquals(p1, p2));
        }
    }
}
