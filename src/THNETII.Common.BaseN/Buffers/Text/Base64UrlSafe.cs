using System;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace THNETII.Common.Buffers.Text
{
    /// <seealso cref="Base64"/>
    public static class Base64UrlSafe
    {
        private static readonly byte urlsafe62 = EncodeUtf8(Base64UrlSafeConvert.urlsafe62);
        private static readonly byte urlsafe63 = EncodeUtf8(Base64UrlSafeConvert.urlsafe63);
        private static readonly ReadOnlyMemory<byte> urlsafeAlphabet = Encoding.UTF8.GetBytes(Base64UrlSafeConvert.urlsafeAlphabet);
        private static readonly byte regular62 = EncodeUtf8(Base64UrlSafeConvert.regular62);
        private static readonly byte regular63 = EncodeUtf8(Base64UrlSafeConvert.regular63);
        private static readonly byte padding = EncodeUtf8(Base64UrlSafeConvert.padding);

        /// <seealso cref="Base64UrlSafeConvert.MakeUrlSafe(Span{char})"/>
        public static int MakeUrlSafeUtf8(Span<byte> regularUtf8) =>
            MakeUrlSafe(regularUtf8,
                regular62, regular63, padding, urlsafe62, urlsafe63);

        /// <seealso cref="Base64UrlSafeConvert.RevertUrlSafe(Span{char}, out int)"/>
        public static int RevertUrlSafeUtf8(Span<byte> urlsafeUtf8,
            out int requiredPadding) =>
            ToRegularBase64Chars(urlsafeUtf8,
                urlsafeAlphabet.Span, regular62, regular63, out requiredPadding);

        private unsafe static byte EncodeUtf8(char ch)
        {
            byte b;
            Encoding.UTF8.GetBytes(&ch, 1, &b, 1);
            return b;
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        internal static int MakeUrlSafe<T>(Span<T> chars,
            T regular62, T regular63, T padding,
            T urlsafe62, T urlsafe63)
            where T : IEquatable<T>
        {
            int pad = chars.IndexOf(padding);
            if (pad >= 0)
                chars = chars.Slice(0, pad);
            Span<T> tmp = chars;
            for (int idx = tmp.IndexOfAny(regular62, regular63); idx >= 0; idx = tmp.IndexOfAny(regular62, regular63))
            {
                T ch = tmp[idx];
                if (ch.Equals(regular62))
                    tmp[idx] = urlsafe62;
                else if (ch.Equals(regular63))
                    tmp[idx] = urlsafe63;
                tmp = tmp.Slice(idx + 1);
            }
            return chars.Length;
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        internal static int ToRegularBase64Chars<T>(Span<T> chars,
            ReadOnlySpan<T> urlsafeAlphabet, T regular62, T regular63,
            out int requiredPadding)
            where T : IEquatable<T>
        {
            Span<T> tmp = chars;
            int charLen = 0, charCount = 0;
            for (int idx = tmp.IndexOfAny(urlsafeAlphabet); idx >= 0; idx = tmp.IndexOfAny(urlsafeAlphabet))
            {
                charLen += idx;
                T ch = tmp[idx];
                if (ch.Equals(urlsafeAlphabet[62]))
                    tmp[idx] = regular62;
                else if (ch.Equals(urlsafeAlphabet[63]))
                    tmp[idx] = regular63;
                charCount++;
                charLen++;
                tmp = tmp.Slice(idx + 1);
            }
            var endCount = charCount % 4;
            switch (endCount)
            {
                default:
                case 0:
                    requiredPadding = 0;
                    break;
                case 1:
                    // Technically true, but can never happen if proper base-64
                    // input, as there cannot be 3 padding characters in base-64.
                    requiredPadding = 3;
                    break;
                case 2:
                    requiredPadding = 2;
                    break;
                case 3:
                    requiredPadding = 1;
                    break;
            }
            return charLen;
        }
    }
}
