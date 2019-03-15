using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace THNETII.Common.Buffers.Text
{
    public static class Base64Utf16
    {
        private static readonly ArrayPool<byte> bytePool = ArrayPool<byte>.Shared;

        // Can use ASCII as optimization here, since Base64-Alphabet is equal in
        // UTF-8 and ASCII.
        private static readonly ObjectPool<Encoder> utf8EncoderPool =
            AsciiEncodingPool.EncoderPool;
        private static readonly ObjectPool<Decoder> utf8DecoderPool =
            AsciiEncodingPool.DecoderPool;

        public static OperationStatus DecodeFromChars(
            char[] chars, int charIndex, int charCount,
            Span<byte> bytes, out int charsConsumed, out int bytesWritten,
            bool isFinalBlock = true, bool scrubBuffer = true)
        {
            var encoder = utf8EncoderPool.Get();
            try
            {
                int utf8Count = encoder.GetByteCount(chars, charIndex, charCount, isFinalBlock);
                byte[] utf8Bytes = bytePool.Rent(utf8Count);
                try
                {
                    utf8Count = encoder.GetBytes(
                        chars, charIndex, charCount,
                        utf8Bytes, byteIndex: 0,
                        isFinalBlock);
                    return Base64.DecodeFromUtf8(
                        new ReadOnlySpan<byte>(utf8Bytes, 0, utf8Count),
                        bytes, out charsConsumed, out bytesWritten, isFinalBlock
                        );
                }
                finally { bytePool.Return(utf8Bytes, scrubBuffer); }
            }
            finally { utf8EncoderPool.Return(encoder); }
        }

        public static unsafe OperationStatus DecodeFromChars(
            char* chars, int charCount,
            Span<byte> bytes, out int charsConsumed, out int bytesWritten,
            bool isFinalBlock = true, bool scrubBuffer = true)
        {
            var encoder = utf8EncoderPool.Get();
            try
            {
                int utf8Count = encoder.GetByteCount(chars, charCount, isFinalBlock);
                byte[] utf8Bytes = bytePool.Rent(utf8Count);
                try
                {
                    fixed (byte* utf8Ptr = utf8Bytes)
                    {
                        utf8Count = encoder.GetBytes(
                            chars, charCount,
                            utf8Ptr, utf8Bytes.Length,
                            isFinalBlock);
                    }
                    return Base64.DecodeFromUtf8(
                        new ReadOnlySpan<byte>(utf8Bytes, 0, utf8Count),
                        bytes, out charsConsumed, out bytesWritten, isFinalBlock
                        );
                }
                finally { bytePool.Return(utf8Bytes, scrubBuffer); }
            }
            finally { utf8EncoderPool.Return(encoder); }
        }

        public static int GetMaxDecodedFromCharCount(int charCount) =>
            Base64.GetMaxDecodedFromUtf8Length(charCount);

        public static OperationStatus EncodeToChars(
            ReadOnlySpan<byte> bytes, char[] chars, int charIndex,
            out int bytesConsumed, out int charsWritten,
            bool isFinalBlock = true, bool scrubBuffer = true)
        {
            var decoder = utf8DecoderPool.Get();
            try
            {
                var utf8Count = Base64.GetMaxEncodedToUtf8Length(bytes.Length);
                byte[] utf8Bytes = bytePool.Rent(utf8Count);
                try
                {
                    var opStatus = Base64.EncodeToUtf8(bytes, utf8Bytes,
                        out bytesConsumed, out charsWritten, isFinalBlock);
                    decoder.GetChars(
                        utf8Bytes, 0, charsWritten,
                        chars, charIndex, isFinalBlock);
                    return opStatus;
                }
                finally { bytePool.Return(utf8Bytes, scrubBuffer); }
            }
            finally { utf8DecoderPool.Return(decoder); }
        }

        public static unsafe OperationStatus EncodeToChars(
            ReadOnlySpan<byte> bytes, char* chars, int charCount,
            out int bytesConsumed, out int charsWritten,
            bool isFinalBlock = true, bool scrubBuffer = true)
        {
            var decoder = utf8DecoderPool.Get();
            try
            {
                var utf8Count = Base64.GetMaxEncodedToUtf8Length(bytes.Length);
                byte[] utf8Bytes = bytePool.Rent(utf8Count);
                try
                {
                    var opStatus = Base64.EncodeToUtf8(bytes, utf8Bytes,
                        out bytesConsumed, out charsWritten, isFinalBlock);
                    fixed (byte* utf8Ptr = utf8Bytes)
                    {
                        if (utf8Ptr != null)
                        {
                            decoder.GetChars(
                                utf8Ptr, charsWritten,
                                chars, charCount, isFinalBlock); 
                        }
                    }
                    return opStatus;
                }
                finally { bytePool.Return(utf8Bytes, scrubBuffer); }
            }
            finally { utf8DecoderPool.Return(decoder); }
        }

        public static int GetMaxEncodedCharCount(int length) =>
            Base64.GetMaxEncodedToUtf8Length(length);
    }
}
