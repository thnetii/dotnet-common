using System;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.Common
{
    /// <summary>
    /// Provides URL-safe Base64 conversion methods to convert regular Base64-characters
    /// to and from URL-safe Base64-characters.
    /// </summary>
    public static class Base64UrlSafeConvert
    {
        internal const string base64First = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        internal const char urlsafe62 = '-';
        internal const char urlsafe63 = '_';
        internal static readonly string urlsafeAlphabet = base64First + urlsafe62 + urlsafe63;
        internal const char regular62 = '+';
        internal const char regular63 = '/';
        internal const char padding = '=';

        /// <summary>
        /// In-Place Transcodes a span of Unicode characters that contains
        /// regular base-64 digits to the URL-safe base-64 equivalent.
        /// </summary>
        /// <param name="chars">The span of regular base-64 characters to convert.</param>
        /// <returns>
        /// The length of the URL-safe span. It may be shorter than then length
        /// of <paramref name="chars"/>.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The regular base-64 alphabet is equal to the URL-safe Base-64
        /// alphabet, but for the 62<sup>nd</sup> and 63<sup>rd</sup> characters.
        /// All occurances of <c>'+'</c> are replaced by <c>'-'</c> and all
        /// occurances of <c>'/'</c> are replaced by <c>'_'</c>.
        /// </para>
        /// <para>
        /// Because URL-safe Base64 encoding does not use trailing padding
        /// characters the returned span may be shorter than <paramref name="chars"/>.
        /// The returned span will only span up to, but exclude the first encountered
        /// padding character (<c>'='</c>).
        /// </para>
        /// </remarks>
        public static int MakeUrlSafe(Span<char> chars) =>
            MakeUrlSafe(chars, regular62, regular63, padding, urlsafe62, urlsafe63);

        /// <summary>
        /// In-Place transcodes a span of URL-safe Base-64 digits to its regular
        /// base-64 equivalent.
        /// </summary>
        /// <param name="chars">The span of URL-safe base-64 characters to convert.</param>
        /// <param name="requiredPadding">Receives the number of required padding characters that have to be added to create a proper Base64 sequence.</param>
        /// <returns>
        /// The length of the Base64-span excluding additional padding.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The regular base-64 alphabet is equal to the URL-safe Base-64
        /// alphabet, but for the 62<sup>nd</sup> and 63<sup>rd</sup> characters.
        /// All occurances of <c>'+'</c> are replaced by <c>'-'</c> and all
        /// occurances of <c>'/'</c> are replaced by <c>'_'</c>.
        /// </para>
        /// <para>
        /// The method will count the number of found Base64-digits while converting
        /// the URL-safe digits to non-safe digits. If the number of Base64-digits
        /// is not a multiple of 4, padding characters (<c>=</c>) have to be added
        /// to make the number of digits a multiple of 4.
        /// </para>
        /// </remarks>
        public static int RevertUrlSafe(Span<char> chars, out int requiredPadding) =>
            ToRegularBase64Chars(chars, urlsafeAlphabet.AsSpan(), regular62, regular63, out requiredPadding);


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
