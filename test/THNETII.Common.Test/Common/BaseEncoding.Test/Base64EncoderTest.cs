using System.Text;
using Xunit;

namespace THNETII.Common.BaseEncoding.Test
{
    public class Base64EncoderTest
    {
        private static readonly Base64Encoder base64Encoder = new Base64Encoder();

        [Theory]
        // From https://tools.ietf.org/html/rfc4648#section-10
        [InlineData("", "")]
        [InlineData("f", "Zg==")]
        [InlineData("fo", "Zm8=")]
        [InlineData("foo", "Zm9v")]
        [InlineData("foob", "Zm9vYg==")]
        [InlineData("fooba", "Zm9vYmE=")]
        [InlineData("foobar", "Zm9vYmFy")]
        // From https://en.wikipedia.org/wiki/Base64
        [InlineData("Man is distinguished, not only by his reason, but by this singular passion from other animals, which is a lust of the mind, that by a perseverance of delight in the continued and indefatigable generation of knowledge, exceeds the short vehemence of any carnal pleasure.", "TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlzIHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2YgdGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGludWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRoZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4=")]
        [InlineData("Man", "TWFu")]
        [InlineData("Ma", "TWE=")]
        [InlineData("M", "TQ==")]
        public static void WellKnownAsciiBytesEncodeCorrectly(string text, string base64Expect)
        {
            var ascii = Encoding.ASCII.GetBytes(text);

            int base64Length = base64Encoder.GetCharCount(ascii, index: 0, count: ascii.Length);
            Assert.Equal(base64Expect.Length, base64Length);
            char[] base64Chars = new char[base64Length];
            base64Length = base64Encoder.GetChars(ascii, byteIndex: 0, byteCount: ascii.Length, base64Chars, charIndex: 0);
            Assert.Equal(base64Expect.Length, base64Length);

            Assert.Equal(base64Expect, base64Chars);
        }
    }
}
