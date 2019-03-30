using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace System.Text.Test
{
    public static class SurrogatePairUtf8EncodingRoundTripTest
    {
        [Theory]
        [InlineData("🌉")]
        [InlineData("🌐")]
        [InlineData("Hello, 😀! How are you today?")]
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static void Split_SurrogatePair_is_reassembled_on_Utf8_encoder_roundtrip(string s)
        {
            var encoder = Encoding.UTF8.GetEncoder();
            byte[] utf8Buffer = new byte[Encoding.UTF8.GetByteCount(s)];
            int surrogateIdx = s.IndexOfSurrogatePair() + 1;

            ReadOnlySpan<char> firstPart = s.AsSpan().Slice(0, surrogateIdx);
            ReadOnlySpan<char> secondPart = s.AsSpan().Slice(surrogateIdx);
            Span<byte> firstUtf8 = utf8Buffer;
            int bytesWritten = encoder.GetBytes(firstPart, utf8Buffer, flush: false);
            Span<byte> secondUtf8 = firstUtf8.Slice(bytesWritten);
            bytesWritten += encoder.GetBytes(secondPart, secondUtf8, flush: true);

            string reassembled = Encoding.UTF8.GetString(utf8Buffer, 0, bytesWritten);

            Assert.Equal(s, reassembled, StringComparer.Ordinal);
        }
    }
}
