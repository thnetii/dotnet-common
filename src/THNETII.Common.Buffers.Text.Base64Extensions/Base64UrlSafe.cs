using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.Common.Buffers.Text
{
    public static class Base64UrlSafe
    {
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static OperationStatus DecodeFromChars(
            char[] chars, int charIndex, int charCount,
            Span<byte> bytes, out int charsConsumed, out int bytesWritten,
            bool isFinalBlock = true, bool scrubBuffer = true)
        {
            RevertUrlSafe(
                new Span<char>(chars, charIndex, charCount),
                out int charsEnd, isFinalBlock);
            var opStatus = Base64Utf16.DecodeFromChars(
                chars, charIndex, charCount - charsEnd,
                bytes, out charsConsumed, out bytesWritten,
                isFinalBlock, scrubBuffer);
            if (opStatus != OperationStatus.Done)
                return opStatus;
            unsafe
            {
                char* lastGroup = stackalloc char[] { '=', '=', '=', '=' };
                for (int chIdx = charIndex + charsConsumed, gIdx = 0; gIdx < 4; gIdx++, chIdx++)
                {
                    lastGroup[gIdx] = chars[chIdx];
                }
                opStatus = Base64Utf16.DecodeFromChars(
                    lastGroup, 4, bytes.Slice(bytesWritten),
                    out int gConsumed, out int gWritten,
                    isFinalBlock, scrubBuffer);
                charsConsumed += charsEnd;
                bytesWritten += gWritten;
            }
            return opStatus;
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static unsafe OperationStatus DecodeFromChars(
            char* chars, int charCount,
            Span<byte> bytes, out int charsConsumed, out int bytesWritten,
            bool isFinalBlock = true, bool scrubBuffer = true)
        {
            RevertUrlSafe(
                new Span<char>(chars, charCount),
                out int charsEnd, isFinalBlock);
            var opStatus = Base64Utf16.DecodeFromChars(
                chars, charCount - charsEnd,
                bytes, out charsConsumed, out bytesWritten,
                isFinalBlock, scrubBuffer);
            if (opStatus != OperationStatus.Done)
                return opStatus;
            char* lastGroup = stackalloc char[] { '=', '=', '=', '=' };
            for (int chIdx = charsConsumed, gIdx = 0; gIdx < 4; gIdx++, chIdx++)
            {
                lastGroup[gIdx] = chars[chIdx];
            }
            opStatus = Base64Utf16.DecodeFromChars(
                lastGroup, 4, bytes.Slice(bytesWritten),
                out int gConsumed, out int gWritten,
                isFinalBlock, scrubBuffer);
            charsConsumed += charsEnd;
            bytesWritten += gWritten;
            return opStatus;
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        private static void RevertUrlSafe(Span<char> chars, out int endChars,
            bool isFinalBlock = true)
        {
            Span<char> tmp = chars;
            for (int idx = tmp.IndexOfAny('+', '/'); idx >= 0;
                tmp = tmp.Slice(idx + 1), idx = tmp.IndexOfAny('+', '/'))
            {
                switch (tmp[idx])
                {
                    case '-':
                        tmp[idx] = '+';
                        break;
                    case '_':
                        tmp[idx] = '/';
                        break;
                }
            }
            if (isFinalBlock)
                endChars = chars.Length % 4;
            else
                endChars = 0;
        }

        public static int GetMaxDecodedFromCharCount(int charCount) =>
            Base64.GetMaxDecodedFromUtf8Length(charCount);

        public static OperationStatus EncodeToChars(
            ReadOnlySpan<byte> bytes, char[] chars, int charIndex,
            out int bytesConsumed, out int charsWritten,
            bool isFinalBlock = true, bool scrubBuffer = true)
        {
            var opStatus = Base64Utf16.EncodeToChars(
                bytes, chars, charIndex,
                out bytesConsumed, out charsWritten,
                isFinalBlock, scrubBuffer);
            charsWritten = MakeUrlSafe(
                new Span<char>(chars, charIndex, charsWritten));
            return opStatus;
        }

        public static unsafe OperationStatus EncodeToChars(
            ReadOnlySpan<byte> bytes, char* chars, int charCount,
            out int bytesConsumed, out int charsWritten,
            bool isFinalBlock = true, bool scrubBuffer = true)
        {
            var opStatus = Base64Utf16.EncodeToChars(
                bytes, chars, charCount,
                out bytesConsumed, out charsWritten,
                isFinalBlock, scrubBuffer);
            charsWritten = MakeUrlSafe(
                new Span<char>(chars, charsWritten));
            return opStatus;
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        private static int MakeUrlSafe(Span<char> chars)
        {
            Span<char> tmp = chars;
            for (int idx = tmp.IndexOfAny('+', '/'); idx >= 0;
                tmp = tmp.Slice(idx + 1), idx = tmp.IndexOfAny('+', '/'))
            {
                switch (tmp[idx])
                {
                    case '+':
                        tmp[idx] = '-';
                        break;
                    case '/':
                        tmp[idx] = '_';
                        break;
                }
            }
            for (int idx = chars.Length - 1; idx >= 0; idx--)
            {
                if (tmp[idx] != '=')
                    return idx + 1;
            }
            return 0;
        }

        public static int GetMaxEncodedCharCount(int length) =>
            Base64.GetMaxEncodedToUtf8Length(length);
    }
}
