using Xunit;

namespace System.Test
{
    public static class ReadOnlySpanConstructorTest
    {
        [SkippableFact]
        public static void Span_ctor_with_array_and_negative_length_throws()
        {
            var bytes = new byte[10];

            var argExcept = Assert.ThrowsAny<ArgumentOutOfRangeException>(() =>
            {
                _ = new ReadOnlySpan<byte>(bytes, start: 0, length: -1);
            });
            Skip.IfNot("length" == argExcept.ParamName, "https://github.com/dotnet/corefx/issues/36014");
            Assert.Equal("length", argExcept.ParamName);
        }

        [SkippableFact]
        public static unsafe void Span_ctor_with_pointer_and_negative_length_throws()
        {
            var bytes = stackalloc byte[10];

            var argExcept = Assert.ThrowsAny<ArgumentOutOfRangeException>(() =>
            {
                _ = new ReadOnlySpan<byte>(bytes, length: -1);
            });
            Skip.IfNot("length" == argExcept.ParamName, "https://github.com/dotnet/corefx/issues/36014");
            Assert.Equal("length", argExcept.ParamName);
        }
    }
}
