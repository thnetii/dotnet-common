using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace THNETII.Common.DataContractSerializer.Test
{
    public static class BooleanStringConverterTest
    {
        [Fact]
        public static void ParseStaticTrueString()
            => Assert.True(BooleanStringConverter.Parse(bool.TrueString));

        [Theory]
        [InlineData("TRUE")]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("trUe")]
        public static void ParseTrueString(string s)
            => Assert.True(BooleanStringConverter.Parse(s));

        [Fact]
        public static void ParseStaticFalseString()
            => Assert.False(BooleanStringConverter.Parse(bool.FalseString));

        [Theory]
        [InlineData("FALSE")]
        [InlineData("false")]
        [InlineData("False")]
        [InlineData("faLSe")]
        public static void ParseFalseString(string s)
            => Assert.False(BooleanStringConverter.Parse(s));

        [Fact]
        public static void ParseUppercaseTrueString() =>
            Assert.True(BooleanStringConverter.Parse(bool.TrueString.ToUpperInvariant()));

        [Fact]
        public static void ParseUppercaseFalseString() =>
            Assert.False(BooleanStringConverter.Parse(bool.FalseString.ToUpperInvariant()));

        [Theory]
        [InlineData(null)]
        public static void ParseFalsyStringAsFalse(string s) =>
            Assert.False(BooleanStringConverter.Parse(s));

        [Theory]
        [InlineData("y")]
        [InlineData("Y")]
        [InlineData("yes")]
        [InlineData("YES")]
        [InlineData("Yes")]
        public static void ParsePositiveAnswerStringAsTrue(string s) =>
            Assert.True(BooleanStringConverter.Parse(s));

        [Theory]
        [InlineData("n")]
        [InlineData("N")]
        [InlineData("no")]
        [InlineData("NO")]
        [InlineData("No")]
        public static void ParseNegativeAnswerStringAsFalse(string s) =>
            Assert.False(BooleanStringConverter.Parse(s));

        [Theory]
        [InlineData("0")]
        [InlineData("0x0")]
        [InlineData("0x000000")]
        public static void ParseZeroStringAsFalse(string s) =>
            Assert.False(BooleanStringConverter.Parse(s));

        [Theory]
        [InlineData("1")]
        [InlineData("-1")]
        [InlineData("42")]
        [InlineData("-42")]
        [InlineData("0x1")]
        [InlineData("0xFFFFFFFF")]
        [InlineData("0xDEADBEEF")]
        [InlineData("FFFFFFFF")]
        [InlineData("DEADBEEF")]
        public static void ParseNonZeroStringAsTrue(string s) =>
            Assert.True(BooleanStringConverter.Parse(s));

        private static readonly string[] whiteSpace = new[] { " ", "\t", "\n", "\r\n" };

        private static IEnumerable<string> GetWhiteSpaceSurrounded(string ws, string s)
        {
            yield return ws + s;
            yield return s + ws;
            yield return ws + s + ws;
        }

        [Theory]
        [InlineData("TRUE")]
        [InlineData("true")]
        [InlineData("True")]
        [InlineData("trUe")]
        [InlineData("y")]
        [InlineData("Y")]
        [InlineData("yes")]
        [InlineData("YES")]
        [InlineData("Yes")]
        [InlineData("1")]
        [InlineData("-1")]
        [InlineData("42")]
        [InlineData("-42")]
        [InlineData("0x1")]
        [InlineData("0xFFFFFFFF")]
        [InlineData("0xDEADBEEF")]
        [InlineData("FFFFFFFF")]
        [InlineData("DEADBEEF")]
        public static void ParseWhiteSpaceSurroundedStringAsTrue(string s)
        {
            Assert.All(whiteSpace.SelectMany(ws => GetWhiteSpaceSurrounded(ws, s)),
                t => Assert.True(BooleanStringConverter.Parse(t)));
        }

        [Theory]
        [InlineData("FALSE")]
        [InlineData("false")]
        [InlineData("False")]
        [InlineData("faLSe")]
        [InlineData("n")]
        [InlineData("N")]
        [InlineData("no")]
        [InlineData("NO")]
        [InlineData("No")]
        [InlineData("0")]
        [InlineData("0x0")]
        [InlineData("0x000000")]
        public static void ParseWhiteSpaceSurroundedStringAsFalse(string s)
        {
            Assert.All(whiteSpace.SelectMany(ws => GetWhiteSpaceSurrounded(ws, s)),
                t => Assert.False(BooleanStringConverter.Parse(t)));
        }

        [Fact]
        public static void ParseNullStringAsFalse() => Assert.False(
            BooleanStringConverter.Parse(null));

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        [InlineData("\t")]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("Test")]
        public static void ThrowsArgumentExceptionAnyUnrecognizedString(string s)
            => Assert.Throws<FormatException>(() => BooleanStringConverter.Parse(s));
    }
}
