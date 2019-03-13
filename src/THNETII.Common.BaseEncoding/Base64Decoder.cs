using System;
using System.Text;

namespace THNETII.Common.BaseEncoding
{
    public class Base64Decoder : Encoder
    {
        public Base64Decoder() : base()
        { }

#if !NETSTANDARD1_3
        public override unsafe int GetByteCount(char* chars, int count, bool flush)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            return GetByteCount(new ReadOnlySpan<char>(chars, count), flush);
        }
#endif // !NETSTANDARD1_3

        public override int GetByteCount(char[] chars, int index, int count, bool flush)
        {
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            return GetByteCount(new ReadOnlySpan<char>(chars, index, count), flush);
        }

        public int GetByteCount(ReadOnlySpan<char> chars, bool flush)
        {
            throw new NotImplementedException();
        }

#if !NETSTANDARD1_3
        public override unsafe int GetBytes(char* chars, int charCount,
            byte* bytes, int byteCount, bool flush)
        {
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            return GetBytes(
                new ReadOnlySpan<char>(chars, charCount),
                new Span<byte>(bytes, byteCount),
                flush);
        }
#endif // !NETSTANDARD1_3

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
        {
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            return GetBytes(
                new ReadOnlySpan<char>(chars, charIndex, charCount),
                new Span<byte>(bytes, byteIndex, bytes.Length - byteIndex),
                flush);
        }

        public int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes, bool flush)
        {
            throw new NotImplementedException();
        }
    }
}
