using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using THNETII.Common.Buffers.Text;
using Xunit;

namespace THNETII.Common.BaseEncoding.Test
{
    public class Base64Utf16Test
    {
        public static IEnumerable<object[]> GetIetfTestVectors()
        {
            // From https://tools.ietf.org/html/rfc4648#section-10
            return new (string category, string asciiText, string base64Text)[]
            {
                ("IETF", "", ""),
                ("IETF", "f", "Zg=="),
                ("IETF", "fo", "Zm8="),
                ("IETF", "foo", "Zm9v"),
                ("IETF", "foob", "Zm9vYg=="),
                ("IETF", "fooba", "Zm9vYmE="),
                ("IETF", "foobar", "Zm9vYmFy")
            }.Select(t => new object[] { t.category, t.asciiText, t.base64Text });
        }

        public static IEnumerable<object[]> GetLeviathanQuote()
        {
            // From https://en.wikipedia.org/wiki/Base64#Examples
            yield return new object[]
            {
                "Leviathan",

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
            return new (string category, string asciiText, string base64Text)[]
            {
                ("Wikipedia", "Man", "TWFu"),
                ("Wikipedia", "Ma", "TWE="),
                ("Wikipedia", "M", "TQ=="),
            }.Select(t => new object[] { t.category, t.asciiText, t.base64Text });
        }

        [Theory]
        [MemberData(nameof(GetIetfTestVectors))]
        [MemberData(nameof(GetLeviathanQuote))]
        [MemberData(nameof(GetWikipediaTestVectors))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public void EncodeToChars_returns_equal_to_test_vector_length_using_array(string category, string asciiText, string base64Text)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(asciiText);
            int base64Count = Base64Utf16.GetMaxEncodedCharCount(asciiBytes.Length);
            var base64Chars = new char[base64Count];

            var opStatus = Base64Utf16.EncodeToChars(
                asciiBytes, base64Chars, charIndex: 0,
                out int asciiConsumed, out base64Count);

            Assert.Equal(OperationStatus.Done, opStatus);
            Assert.Equal(base64Text.Length, base64Count);
            Assert.Equal(base64Text, base64Chars);
        }

        [Theory]
        [MemberData(nameof(GetIetfTestVectors))]
        [MemberData(nameof(GetLeviathanQuote))]
        [MemberData(nameof(GetWikipediaTestVectors))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public unsafe void EncodeToChars_returns_equal_to_test_vector_length_using_pointer(string category, string asciiText, string base64Text)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(asciiText);
            int base64Count = Base64Utf16.GetMaxEncodedCharCount(asciiBytes.Length);
            char[] base64Chars = new char[base64Count];

            OperationStatus opStatus;
            fixed (char* base64Ptr = base64Chars)
            {
                opStatus = Base64Utf16.EncodeToChars(
                    asciiBytes, base64Ptr, base64Count,
                    out int asciiConsumed, out base64Count);
            }

            Assert.Equal(OperationStatus.Done, opStatus);
            Assert.Equal(base64Text.Length, base64Count);
            Assert.Equal(base64Text, base64Chars);
        }
    }
}
