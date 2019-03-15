using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace THNETII.Common.Buffers.Text
{
    public static class Base64UrlSafe
    {
        private static readonly byte regular62 = Encoding.UTF8.GetBytes(Base64UrlSafeConvert.regular62.ToString(CultureInfo.InvariantCulture))[0];
        private static readonly byte regular63 = Encoding.UTF8.GetBytes(Base64UrlSafeConvert.regular63.ToString(CultureInfo.InvariantCulture))[0];
        private static readonly byte urlsafe62 = Encoding.UTF8.GetBytes(Base64UrlSafeConvert.urlsafe62.ToString(CultureInfo.InvariantCulture))[0];
        private static readonly byte urlsafe63 = Encoding.UTF8.GetBytes(Base64UrlSafeConvert.urlsafe63.ToString(CultureInfo.InvariantCulture))[0];
        private static readonly byte padding = Encoding.UTF8.GetBytes(Base64UrlSafeConvert.padding.ToString(CultureInfo.InvariantCulture))[0];

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static OperationStatus DecodeFromUtf8(ReadOnlySpan<byte> utf8,
            Span<byte> bytes, out int bytesConsumed, out int bytesWritten,
            bool isFinalBlock = true)
        {
            if (isFinalBlock)
            {
                int utf8Count = utf8.Length & ~3; // 4-byte aligned length
                int pad4Count = (utf8.Length % 4) != 0 ? 4 : 0; // 4 more bytes if not originally 4-byte aligned
                if (bytes.Length < utf8Count + pad4Count)
                {
                    // Not able to do final block
                    return DecodeFromUtf8(utf8, bytes, out bytesConsumed, out bytesWritten, isFinalBlock: false);
                }

                utf8.CopyTo(bytes);
                Span<byte> pad4Span = bytes.Slice(utf8Count, pad4Count);
                ToRegularUtf8(bytes.Slice(0, utf8.Length), pad4Span, out _);
                var opStatus = Base64.DecodeFromUtf8InPlace(bytes.Slice(0, utf8Count + pad4Count), out bytesWritten);
                bytesConsumed = GetBase64BytesConsumedFromRawBytesWritten(bytesWritten);
                return opStatus;
            }
            else
            {
                // Determine 4-byte aligned lengths of both buffers
                (int utf8Count, int safeCount) = (utf8.Length & ~3, bytes.Length & ~3);
                safeCount = Math.Min(utf8Count, safeCount);
                utf8.Slice(0, safeCount).CopyTo(bytes);
                Span<byte> safeSpan = ToRegularUtf8(bytes.Slice(0, safeCount),
                    endChars: Span<byte>.Empty, out _);
                var opStatus = Base64.DecodeFromUtf8InPlace(safeSpan, out bytesWritten);
                bytesConsumed = GetBase64BytesConsumedFromRawBytesWritten(bytesWritten);
                return opStatus;
            }
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static OperationStatus DecodeFromUtf8InPlace(Span<byte> buffer, out int bytesWritten)
        {
            Span<byte> pad4Span = stackalloc byte[4];
            Span<byte> utf8Span = ToRegularUtf8(buffer, pad4Span, out bool hasEndChars);

            var opStatus = Base64.DecodeFromUtf8InPlace(utf8Span, out bytesWritten);
            if (hasEndChars && opStatus == OperationStatus.Done)
            {
                Base64.DecodeFromUtf8InPlace(pad4Span, out int pad4Written);
                pad4Span.Slice(0, pad4Written).CopyTo(buffer.Slice(bytesWritten));
                bytesWritten += pad4Written;
            }
            return opStatus;
        }

        private static int GetBase64BytesConsumedFromRawBytesWritten(int bytesWritten)
        {
            int groupsWritten = Math.DivRem(bytesWritten, 3, out int bytesEnd);
            int bytesConsumed = groupsWritten * 4;
            if (bytesEnd > 0) // Modulo 3 -> 1 or 2 possible
                bytesConsumed += 2;
            if (bytesEnd > 1) // Modulo 3 -> 2 possible
                bytesConsumed += 1;
            return bytesConsumed;
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes,
            Span<byte> utf8, out int bytesConsumed, out int bytesWritten,
            bool isFinalBlock = true)
        {
            var opStatus = Base64.EncodeToUtf8(bytes, utf8, out bytesConsumed,
                out bytesWritten, isFinalBlock);
            Span<byte> safe = FromRegularUtf8(utf8.Slice(0, bytesWritten));
            bytesWritten = safe.Length;
            return opStatus;
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static OperationStatus EncodeToUtf8InPlace(Span<byte> buffer,
            int dataLength, out int bytesWritten)
        {
            var opStatus = Base64.EncodeToUtf8InPlace(buffer, dataLength,
                out bytesWritten);
            Span<byte> safe = FromRegularUtf8(buffer.Slice(0, bytesWritten));
            bytesWritten = safe.Length;
            return opStatus;
        }

        public static int GetMaxDecodedFromUtf8Length(int length)
        {
            var rem = length % 4;
            if (rem != 0)
                length += rem;
            return Base64.GetMaxDecodedFromUtf8Length(length);
        }

        public static int GetMaxEncodedToUtf8Length(int length) =>
            Base64.GetMaxEncodedToUtf8Length(length);

        public static Span<byte> FromRegularUtf8(Span<byte> chars) =>
            Base64UrlSafeConvert.FromRegularBase64Chars(chars,
                regular62, regular63, padding,
                urlsafe62, urlsafe63);

        public static Span<byte> ToRegularUtf8(Span<byte> chars, Span<byte> endChars, out bool hasEndChars) =>
            Base64UrlSafeConvert.ToRegularBase64Chars(chars,
                urlsafe62, urlsafe63,
                regular62, regular63, padding,
                endChars, out hasEndChars);
    }
}
