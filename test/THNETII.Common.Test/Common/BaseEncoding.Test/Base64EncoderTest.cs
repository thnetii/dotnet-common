using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace THNETII.Common.BaseEncoding.Test
{
    public class Base64EncoderTest
    {
        public static IEnumerable<object[]> GetCharCount_null_invocations()
        {
            return new (string name, Action<Base64Encoder> action)[]
            {
                ("pointer", encoder => { unsafe { encoder.GetCharCount(null, 0, flush: true); } }),
                ("array", encoder => encoder.GetCharCount(null, 0, 0)),
                ("arrayFlushTrue", encoder => encoder.GetCharCount(null, 0, 0, flush: true))
            }.Select(a => new object[] { a.name, a.action });
        }

        [Theory]
        [MemberData(nameof(GetCharCount_null_invocations))]
        public static void GetCharCount_with_null_bytes_throws(string name, Action<Base64Encoder> action)
        {
            var encoder = new Base64Encoder();

            Assert.ThrowsAny<ArgumentNullException>(() => action(encoder));
        }

        public static IEnumerable<object[]> GetCharCount_empty_invocations()
        {
            byte[] bytes = Array.Empty<byte>();
            return new (string name, Action<Base64Encoder> action)[]
            {
                ("pointer", encoder => {
                    unsafe
                    {
                        byte* pBytes = stackalloc byte[1];
                        encoder.GetCharCount(pBytes, 0, flush: true);
                    }
                }),
                ("array", encoder => encoder.GetCharCount(bytes, 0, 0)),
                ("arrayFlushTrue", encoder => encoder.GetCharCount(bytes, 0, 0, flush: true))
            }.Select(a => new object[] { a.name, a.action });
        }

        [Theory]
        [MemberData(nameof(GetCharCount_empty_invocations))]
        public static void GetCharCount_with_empty_bytes_throws(string name, Action<Base64Encoder> action)
        {
            var encoder = new Base64Encoder();
            Assert.ThrowsAny<ArgumentOutOfRangeException>(() => action(encoder));
        }

        public static IEnumerable<object[]> GetIetfTestVectors()
        {
            // From https://tools.ietf.org/html/rfc4648#section-10
            return new[]
            {
                ("", ""),
                ("f", "Zg=="),
                ("fo", "Zm8="),
                ("foo", "Zm9v"),
                ("foob", "Zm9vYg=="),
                ("fooba", "Zm9vYmE="),
                ("foobar", "Zm9vYmFy")
            }.Select(t => new object[] { t.Item1, t.Item2 });
        }

        public static IEnumerable<object[]> GetLeviathanQuote()
        {
            // From https://en.wikipedia.org/wiki/Base64#Examples
            yield return new object[]
            {
                @"Man is distinguished, not only by his reason, but by this singular passion from other animals, " +
                @"which is a lust of the mind, that by a perseverance of delight in the continued and indefatigable " +
                @"generation of knowledge, exceeds the short vehemence of any carnal pleasure.",

                @"TWFuIGlzIGRpc3Rpbmd1aXNoZWQsIG5vdCBvbmx5IGJ5IGhpcyByZWFzb24sIGJ1dCBieSB0aGlz" +
                @"IHNpbmd1bGFyIHBhc3Npb24gZnJvbSBvdGhlciBhbmltYWxzLCB3aGljaCBpcyBhIGx1c3Qgb2Yg" +
                @"dGhlIG1pbmQsIHRoYXQgYnkgYSBwZXJzZXZlcmFuY2Ugb2YgZGVsaWdodCBpbiB0aGUgY29udGlu" +
                @"dWVkIGFuZCBpbmRlZmF0aWdhYmxlIGdlbmVyYXRpb24gb2Yga25vd2xlZGdlLCBleGNlZWRzIHRo" +
                @"ZSBzaG9ydCB2ZWhlbWVuY2Ugb2YgYW55IGNhcm5hbCBwbGVhc3VyZS4="
            };
        }

        public static IEnumerable<object[]> GetWikipediaTestVectors()
        {
            // From https://en.wikipedia.org/wiki/Base64#Examples
            return new[]
            {
                ("Man", "TWFu"),
                ("Ma", "TWE="),
                ("M", "TQ=="),
            }.Select(t => new object[] { t.Item1, t.Item2 });
        }
    }
}
