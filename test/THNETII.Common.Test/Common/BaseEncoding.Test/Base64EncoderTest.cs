using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
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
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public static void GetCharCount_with_null_bytes_throws(string name, Action<Base64Encoder> action)
        {
            var encoder = new Base64Encoder();

            var argExcept = Assert.ThrowsAny<ArgumentNullException>(() => action(encoder));
            Assert.Equal("bytes", argExcept.ParamName);
        }

        public static IEnumerable<object[]> GetCharCount_empty_invocations()
        {
            byte[] bytes = Array.Empty<byte>();
            return new (string name, Func<Base64Encoder, int> action)[]
            {
                ("pointer", encoder => {
                    unsafe
                    {
                        byte* pBytes = stackalloc byte[1];
                        return encoder.GetCharCount(pBytes, 0, flush: true);
                    }
                }),
                ("array", encoder => encoder.GetCharCount(bytes, 0, 0)),
                ("arrayFlushTrue", encoder => encoder.GetCharCount(bytes, 0, 0, flush: true))
            }.Select(a => new object[] { a.name, a.action });
        }

        [Theory]
        [MemberData(nameof(GetCharCount_empty_invocations))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public static void GetCharCount_with_empty_bytes_returns_0(string name, Func<Base64Encoder, int> action)
        {
            var encoder = new Base64Encoder();
            Assert.Equal(0, action(encoder));
        }

        public static IEnumerable<object[]> GetCharCount_negative_length_invocations()
        {
            byte[] bytes = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            return new (string name, Action<Base64Encoder> action)[]
            {
                ("pointer", encoder => {
                    unsafe
                    {
                        byte* pBytes = stackalloc byte[bytes.Length];
                        encoder.GetCharCount(pBytes, -1, flush: true);
                    }
                }),
                ("array", encoder => encoder.GetCharCount(bytes, index: 0, count: -1)),
                ("arrayFlushTrue", encoder => encoder.GetCharCount(bytes, index: 0, count: -1, flush: true))
            }.Select(a => new object[] { a.name, a.action });
        }

        [SkippableTheory]
        [MemberData(nameof(GetCharCount_negative_length_invocations))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public void GetCharCount_with_negative_length_throws(string name, Action<Base64Encoder> action)
        {
            var encoder = new Base64Encoder();
            var argExcept = Assert.ThrowsAny<ArgumentOutOfRangeException>(() => action(encoder));
            Skip.If(argExcept.ParamName == "start", "https://github.com/dotnet/corefx/issues/36014");
            Skip.If(argExcept.ParamName == "index", "https://github.com/dotnet/corefx/issues/36014");
            Assert.Equal("count", argExcept.ParamName);
        }

        public static IEnumerable<object[]> GetCharCount_toolarge_length_invocations()
        {
            byte[] bytes = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            return new (string name, Action<Base64Encoder> action)[]
            {
                ("array", encoder => encoder.GetCharCount(bytes, index: 0, count: bytes.Length + 10)),
                ("arrayFlushTrue", encoder => encoder.GetCharCount(bytes, index: 0, count: bytes.Length + 10, flush: true))
            }.Select(a => new object[] { a.name, a.action });
        }

        [SkippableTheory]
        [MemberData(nameof(GetCharCount_toolarge_length_invocations))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public void GetCharCount_with_too_large_length_throws(string name, Action<Base64Encoder> action)
        {
            var encoder = new Base64Encoder();
            var argExcept = Assert.ThrowsAny<ArgumentOutOfRangeException>(() => action(encoder));
            Skip.If(argExcept.ParamName == "index", "https://github.com/dotnet/corefx/issues/36014");
            Assert.Equal("count", argExcept.ParamName);
        }

        public static IEnumerable<object[]> GetCharCount_negative_index_invocations()
        {
            byte[] bytes = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            return new (string name, Action<Base64Encoder> action)[]
            {
                ("array", encoder => encoder.GetCharCount(bytes, index: -1, count: bytes.Length)),
                ("arrayFlushTrue", encoder => encoder.GetCharCount(bytes, index: -1, count: bytes.Length, flush: true))
            }.Select(a => new object[] { a.name, a.action });
        }

        [Theory]
        [MemberData(nameof(GetCharCount_negative_index_invocations))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public void GetCharCount_with_negative_index_throws(string name, Action<Base64Encoder> action)
        {
            var encoder = new Base64Encoder();
            var argExcept = Assert.ThrowsAny<ArgumentOutOfRangeException>(() => action(encoder));
            Assert.Equal("index", argExcept.ParamName);
        }

        public static IEnumerable<object[]> GetCharCount_too_large_index_invocations()
        {
            byte[] bytes = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray();
            return new (string name, Action<Base64Encoder> action)[]
            {
                ("array", encoder => encoder.GetCharCount(bytes, index: bytes.Length + 10, count: bytes.Length)),
                ("arrayFlushTrue", encoder => encoder.GetCharCount(bytes, index: bytes.Length + 10, count: bytes.Length, flush: true))
            }.Select(a => new object[] { a.name, a.action });
        }

        [Theory]
        [MemberData(nameof(GetCharCount_too_large_index_invocations))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public void GetCharCount_with_too_large_index_throws(string name, Action<Base64Encoder> action)
        {
            var encoder = new Base64Encoder();
            var argExcept = Assert.ThrowsAny<ArgumentOutOfRangeException>(() => action(encoder));
            Assert.Equal("index", argExcept.ParamName);
        }

        public static IEnumerable<object[]> GetChars_null_bytes_invocations()
        {
            const int charCount = 10;
            return new (string name, Action<Base64Encoder> action)[]
            {
                ("pointer", encoder =>
                {
                    unsafe
                    {
                        char* chars = stackalloc char[charCount];
                        encoder.GetChars(null, 0, chars, charCount, flush: true);
                    }
                }),
                ("array", encoder => encoder.GetChars(null, 0, 0, new char[charCount], charCount)),
                ("arrayFlushTrue", encoder => encoder.GetChars(null, 0, 0, new char[charCount], charCount, flush: true))
            }.Select(a => new object[] { a.name, a.action });
        }

        [Theory]
        [MemberData(nameof(GetChars_null_bytes_invocations))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public void GetChars_with_null_bytes_throws(string name, Action<Base64Encoder> action)
        {
            var encoder = new Base64Encoder();
            var argExcept = Assert.ThrowsAny<ArgumentNullException>(() => action(encoder));
            Assert.Equal("bytes", argExcept.ParamName);
        }

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
        public void GetCharCount_returns_equal_to_test_vector_length_using_array_index_length(string category, string asciiText, string base64Text)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(asciiText);
            var encoder = new Base64Encoder();

            int base64Count = encoder.GetCharCount(asciiBytes, index: 0, count: asciiBytes.Length);

            Assert.Equal(base64Text.Length, base64Count);
        }

        [Theory]
        [MemberData(nameof(GetIetfTestVectors))]
        [MemberData(nameof(GetLeviathanQuote))]
        [MemberData(nameof(GetWikipediaTestVectors))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public void GetCharCount_returns_equal_to_test_vector_length_using_array_index_length_flush_true(string category, string asciiText, string base64Text)
        {
            var asciiBytes = Encoding.ASCII.GetBytes(asciiText);
            var encoder = new Base64Encoder();

            int base64Count = encoder.GetCharCount(asciiBytes, index: 0, count: asciiBytes.Length, flush: true);

            Assert.Equal(base64Text.Length, base64Count);
        }

        [Theory]
        [MemberData(nameof(GetIetfTestVectors))]
        [MemberData(nameof(GetLeviathanQuote))]
        [MemberData(nameof(GetWikipediaTestVectors))]
        [SuppressMessage("Style", "IDE0060: Remove unused parameter")]
        [SuppressMessage("Usage", "CA1801: Unused formal parameter")]
        [SuppressMessage("Usage", "xUnit1026: Theory methods should use all of their parameters")]
        public unsafe void GetCharCount_returns_equal_to_test_vector_length_using_pointer(string category, string asciiText, string base64Text)
        {
            var asciiCount = Encoding.ASCII.GetByteCount(asciiText);
            byte* asciiBytes = stackalloc byte[asciiCount != 0 ? asciiCount : 1];
            fixed (char* asciiChars = asciiText)
            {
                asciiCount = Encoding.ASCII.GetBytes(asciiChars, asciiText.Length, asciiBytes, asciiCount);
            }
            var base64encoder = new Base64Encoder();

            int base64Count = base64encoder.GetCharCount(asciiBytes, count: asciiCount, flush: true);

            Assert.Equal(base64Text.Length, base64Count);
        }
    }
}
