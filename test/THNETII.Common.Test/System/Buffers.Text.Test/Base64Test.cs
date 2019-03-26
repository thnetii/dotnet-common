using Xunit;

namespace System.Buffers.Text.Test
{
    public static class Base64Test
    {
        [Fact]
        public static void Base64_DecodeFromUtf8_with_empty_input()
        {
            var status = Base64.DecodeFromUtf8(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out int bytesConsumed, out int bytesWritten);

            Assert.Equal(OperationStatus.Done, status);
            Assert.Equal(0, bytesConsumed);
            Assert.Equal(0, bytesWritten);
        }
    }
}
