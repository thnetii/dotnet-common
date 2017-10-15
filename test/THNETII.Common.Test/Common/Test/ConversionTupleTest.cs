using System;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("Microsoft.Design", "CA1305", Justification = "int.ToString() return value not relevant for test case.")]
        public void NewConversionTupleWithNullComparer()
        {
            var conv = new ConversionTuple<int, string>(i => i.ToString(), rawEqualityComparer: null);
        }
    }
}
