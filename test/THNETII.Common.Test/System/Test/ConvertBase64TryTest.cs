using System.Diagnostics.CodeAnalysis;
using System.Text;

using Xunit;

namespace System.Test
{
    public static class ConvertBase64TryTest
    {
        [Fact]
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static void TryFromBase64String_returns_false_if_missing_padding()
        {
            const string test = "TWFuIG";
            const string text = "Man";
            Span<byte> bytes = stackalloc byte[test.Length];

            bool success = Convert.TryFromBase64String(test, bytes, out int bytesWritten);

            Assert.False(success);
            Assert.Equal(3, bytesWritten);
            Assert.Equal(text, Encoding.ASCII.GetString(bytes.Slice(bytesWritten)));
        }
    }
}
