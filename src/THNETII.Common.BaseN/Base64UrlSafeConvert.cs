using System;
using THNETII.Common.Buffers.Text;

namespace THNETII.Common
{
    /// <summary>
    /// Provides URL-safe Base64 conversion methods to convert regular Base64-characters
    /// to and from URL-safe Base64-characters.
    /// </summary>
    public static class Base64UrlSafeConvert
    {
        internal const string base64First = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "abcdefghijklmnopqrstuvwxyz" + "0123456789";
        internal const char urlsafe62 = '-';
        internal const char urlsafe63 = '_';
        internal static readonly string urlsafeAlphabet = base64First + urlsafe62 + urlsafe63;
        internal const char regular62 = '+';
        internal const char regular63 = '/';
        internal const char padding = '=';

        /// <summary>
        /// Gets the characters in the URL-safe base-64 alphabet in order of
        /// their sextet value.
        /// </summary>
        [SuppressMessage("Design", "CA1056: Uri properties should not be strings")]
        public static string UrlSafeAlphabet => urlsafeAlphabet;

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
            Base64UrlSafe.MakeUrlSafe(chars, regular62, regular63, padding, urlsafe62, urlsafe63);

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
            Base64UrlSafe.ToRegularBase64Chars(chars, urlsafeAlphabet.AsSpan(), regular62, regular63, out requiredPadding);
    }
}
