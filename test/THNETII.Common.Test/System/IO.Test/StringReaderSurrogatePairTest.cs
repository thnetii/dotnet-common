using System.Diagnostics.CodeAnalysis;

using Xunit;

namespace System.IO.Test
{
    public static class StringReaderSurrogatePairTest
    {
        [Theory]
        [InlineData("🌉")]
        [InlineData("🌐")]
        [InlineData("Hello, 😀! How are you today?")]
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static void Read_can_split_into_low_and_high(string s)
        {
            int splitIdx = s.IndexOfSurrogatePair();
            Span<char> span = stackalloc char[s.Length];
            using (var reader = new StringReader(s))
            {

                Span<char> buffer1 = span.Slice(0, splitIdx);
                int read1 = reader.Read(buffer1);

                Span<char> buffer2 = span.Slice(splitIdx);
                int read2 = reader.Read(buffer2);

                Assert.True(read1 > 0);
                Assert.True(read2 > 0);

                char last1 = buffer1[read1 - 1];
                Assert.True(char.IsHighSurrogate(last1));

                char first2 = buffer2[0];
                Assert.True(char.IsLowSurrogate(first2));
            }
            Assert.Equal(s, span.ToString(), StringComparer.Ordinal);
        }
    }
}
