using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace THNETII.Common.Test
{
    public class ConversionTupleTest
    {
        [Fact]
        public void NullConverterThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConversionTuple<int, string>(rawConvert: null!));
        }

        [Fact]
        [SuppressMessage("Microsoft.Design", "CA1305", Justification = "int.ToString() return value not relevant for test case.")]
        public void NullComparerDoesNotThrow()
        {
            var conv = new ConversionTuple<int, string>(i => i.ToString(), rawEqualityComparer: null);
        }

        [Fact]
        [SuppressMessage("Microsoft.Design", "CA1305", Justification = "int.ToString() return value not relevant for test case.")]
        public void InstanceEquality()
        {
            const int testRawValue = 42;

            static string intToString(int i) => i.ToString();
            var l = new ConversionTuple<int, string>(intToString);
            var r = new ConversionTuple<int, string>(intToString);

            (l.RawValue, r.RawValue) = (testRawValue, testRawValue);

            Assert.Equal(l, r);
            Assert.True(l == r);
            Assert.False(l != r);
        }

        [Fact]
        [SuppressMessage("Microsoft.Design", "CA1305", Justification = "int.ToString() return value not relevant for test case.")]
        public void InstanceInequality()
        {
            const int testRawValueLeft = 42;
            const int testRawValueRight = ~testRawValueLeft;

            static string intToString(int i) => i.ToString();
            var l = new ConversionTuple<int, string>(intToString);
            var r = new ConversionTuple<int, string>(intToString);

            (l.RawValue, r.RawValue) = (testRawValueLeft, testRawValueRight);

            Assert.NotEqual(l, r);
            Assert.True(l != r);
            Assert.False(l == r);
        }
    }
}
