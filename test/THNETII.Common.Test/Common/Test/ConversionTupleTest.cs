using System;
using Xunit;

namespace THNETII.Common.Test
{
    public class ConversionTupleTest
    {
        [Fact]
        public void NewConversionTupleWithNullConverterThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConversionTuple<int, string>(rawConvert: null));
        }

        [Fact]
        public void NewConversionTupleWithNullComparerThrowsArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new ConversionTuple<int, string>(i => i.ToString(), rawEqualityComparer: null));
        }
    }
}
