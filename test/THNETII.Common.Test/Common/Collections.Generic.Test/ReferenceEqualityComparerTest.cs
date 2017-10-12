using System.Text;
using Xunit;

namespace THNETII.Common.Collections.Generic.Test
{
    public class ReferenceEqualityComparerTest
    {
        [Fact]
        public void StringReferenceEqualityComparerDefaultIsNotNull() =>
            Assert.NotNull(ReferenceEqualityComparer<string>.Default);

        [Fact]
        public void SameInstanceStringReferenceEqualityComparisonReturnsTrue()
        {
            const string src = "Test";
            var p1 = src;
            var p2 = src;
            var cmp = new ReferenceEqualityComparer<string>();

            Assert.True(cmp.Equals(p1, p2));
        }

        [Fact]
        public void DifferentInstanceSameValueStringReferenceEqualityComparisonReturnsFalse()
        {
            const string src = "Test";
            var p1 = new StringBuilder(src).ToString();
            var p2 = new StringBuilder(src).ToString();
            var cmp = new ReferenceEqualityComparer<string>();

            Assert.False(cmp.Equals(p1, p2));
        }

        [Fact]
        public void DifferentValueStringReferenceEqualityComparisonReturnsFalse()
        {
            var p1 = "Test1";
            var p2 = "Test2";
            var cmp = new ReferenceEqualityComparer<string>();

            Assert.False(cmp.Equals(p1, p2));
        }

        [Fact]
        public void SameInstanceStringStaticReferenceEqualityComparisonReturnsTrue()
        {
            const string src = "Test";
            var p1 = src;
            var p2 = src;

            Assert.True(ReferenceEqualityComparer<string>.StaticEquals(p1, p2));
        }

        [Fact]
        public void DifferentInstanceSameValueStringStaticReferenceEqualityComparisonReturnsFalse()
        {
            const string src = "Test";
            var p1 = new StringBuilder(src).ToString();
            var p2 = new StringBuilder(src).ToString();

            Assert.False(ReferenceEqualityComparer<string>.StaticEquals(p1, p2));
        }

        [Fact]
        public void DifferentValueStringStaticReferenceEqualityComparisonReturnsFalse()
        {
            var p1 = "Test1";
            var p2 = "Test2";

            Assert.False(ReferenceEqualityComparer<string>.StaticEquals(p1, p2));
        }

        [Fact]
        public void SameValueIntNewEqualsReturnsFalse()
        {
            var cmp = new ReferenceEqualityComparer<int>();

            Assert.False(cmp.Equals(42, 42));
        }

        [Fact]
        public void SameValueIntDefaultEqualsReturnsFalse()
        {
            var cmp = ReferenceEqualityComparer<int>.Default;

            Assert.False(cmp.Equals(42, 42));
        }

        [Fact]
        public void SameValueIntStaticEqualsReturnsFalse() => Assert.False(ReferenceEqualityComparer<int>.StaticEquals(42, 42));
    }
}