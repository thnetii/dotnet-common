using System;
using System.Text;

namespace THNETII.Common.Buffers.Text
{
    public static class Base64UrlSafe
    {
        private static readonly byte urlsafe62 = EncodeUtf8(Base64UrlSafeConvert.urlsafe62);
        private static readonly byte urlsafe63 = EncodeUtf8(Base64UrlSafeConvert.urlsafe63);
        private static readonly ReadOnlyMemory<byte> urlsafeAlphabet = Encoding.UTF8.GetBytes(Base64UrlSafeConvert.urlsafeAlphabet);
        private static readonly byte regular62 = EncodeUtf8(Base64UrlSafeConvert.regular62);
        private static readonly byte regular63 = EncodeUtf8(Base64UrlSafeConvert.regular63);
        private static readonly byte padding = EncodeUtf8(Base64UrlSafeConvert.padding);

        public static int MakeUrlSafeUtf8(Span<byte> regularUtf8) =>
            Base64UrlSafeConvert.MakeUrlSafe(regularUtf8,
                regular62, regular63, padding, urlsafe62, urlsafe63);

        public static int RevertUrlSafeUtf8(Span<byte> urlsafeUtf8,
            out int requiredPadding) =>
            Base64UrlSafeConvert.ToRegularBase64Chars(urlsafeUtf8,
                urlsafeAlphabet.Span, regular62, regular63, out requiredPadding);

        private unsafe static byte EncodeUtf8(char ch)
        {
            byte b;
            Encoding.UTF8.GetBytes(&ch, 1, &b, 1);
            return b;
        }
    }
}
