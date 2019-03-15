using System;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.Common
{
    /// <summary>
    /// Provides URL-safe Base64 conversion methods in addition those defined in
    /// <see cref="Convert"/>.
    /// </summary>
    public static class Base64UrlSafeConvert
    {
        internal const char regular62 = '+';
        internal const char regular63 = '/';
        internal const char urlsafe62 = '-';
        internal const char urlsafe63 = '_';
        internal const char padding = '=';
        private static readonly ArrayPool<char> charPool = ArrayPool<char>.Shared;

        /// <summary>
        /// Converts an 8-bit unsigned integer array to an equivalent subset of
        /// a Unicode character array encoded with URL-safe Base64 digits.
        /// Paramaters optionally specify the subset in the output array, and
        /// whether line breaks are inserted in the output array.
        /// </summary>
        /// <param name="inArray">An input array of 8-bit unsigned integers.</param>
        /// <param name="outArray">An output array of Unicode characters.</param>
        /// <param name="offsetOut">Optional. A position within <paramref name="outArray"/>. Defaults to the start.</param>
        /// <param name="options"><see cref="Base64FormattingOptions.InsertLineBreaks"/> to insert a line break every 76 characters, or <see cref="Base64FormattingOptions.None"/> to not insert line breaks.</param>
        /// <returns>
        /// A 32-bit integer containing the number of encoded URL-safe Base64
        /// written to <paramref name="outArray"/> starting at <paramref name="offsetOut"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="inArray"/> or <paramref name="outArray"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="offsetOut"/> is negative.</para>
        /// <para>-or-</para>
        /// <para><paramref name="offsetOut"/> plus the number of elements to return is greater than the length of <paramref name="outArray"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="options"/> is not a valid <see cref="Base64FormattingOptions"/> value.</exception>
        /// <remarks>
        /// <para>The elements of the <paramref name="inArray"/> parameter are taken as a numeric value and converted to a string representation in base 64.</para>
        /// <para>The URL-safe base-64 digits in ascending order from zero are the uppercase characters "A" to "Z", the lowercase characters "a" to "z", the numerals "0" to "9", and the symbols "-" and "_". No trailing padding is used.</para>
        /// </remarks>
        /// <seealso cref="Convert.ToBase64CharArray(byte[], int, int, char[], int, Base64FormattingOptions)"/>
        public static int ToCharArray(byte[] inArray,
            char[] outArray, int offsetOut = 0,
            Base64FormattingOptions options = Base64FormattingOptions.None) =>
            ToCharArray(inArray, offsetIn: 0, length: inArray?.Length ?? 0,
                outArray, offsetOut, options);

        public static int ToCharArray(byte[] inArray, int offsetIn, int length,
            char[] outArray, int offsetOut = 0,
            Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            int charCount = Convert.ToBase64CharArray(
                inArray, offsetIn, length,
                outArray, offsetOut, options);
            Span<char> outSpan = new Span<char>(outArray, offsetOut, charCount);
            outSpan = FromRegularBase64Chars(outSpan);
            return outSpan.Length;
        }

        public static (byte[] bytes, byte[] end) FromCharArray(char[] inArray) =>
            FromCharArray(inArray, offset: 0, length: inArray?.Length ?? 0);

        public static (byte[] bytes, byte[] end) FromCharArray(char[] inArray, int offset, int length)
        {
            char[] endChars = new char[4];
            Span<char> chars = ToRegularBase64Chars(
                new Span<char>(inArray, offset, length),
                endChars, out bool hasEndChars);
            return (
                Convert.FromBase64CharArray(inArray, offset, chars.Length),
                hasEndChars ? Convert.FromBase64CharArray(endChars, 0, endChars.Length) : Array.Empty<byte>()
                );
        }

        public static string ToString(byte[] inArray,
            Base64FormattingOptions options = Base64FormattingOptions.None) =>
            ToString(inArray, offsetIn: 0, length: inArray?.Length ?? 0, options);

        public static string ToString(byte[] inArray, int offsetIn, int length,
            Base64FormattingOptions options = Base64FormattingOptions.None)
        {
            var charCount = Base64.GetMaxEncodedToUtf8Length(length);
            if (options == Base64FormattingOptions.InsertLineBreaks)
                charCount += (length / 76 + 1) * Environment.NewLine.Length;
            var charArray = charPool.Rent(charCount);
            try
            {
                charCount = ToCharArray(inArray, offsetIn, length,
                    charArray, offsetOut: 0, options);
                return new string(charArray, startIndex: 0, charCount);
            }
            finally { charPool.Return(charArray); }
        }

        public static Span<char> FromRegularBase64Chars(Span<char> chars) =>
            FromRegularBase64Chars(chars, regular62, regular63, padding, urlsafe62, urlsafe63);

        public static Span<char> ToRegularBase64Chars(Span<char> chars,
            Span<char> endChars, out bool hasEndChars) =>
            ToRegularBase64Chars(chars, urlsafe62, urlsafe63, regular62, regular63, padding,
                endChars, out hasEndChars);

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        internal static Span<T> FromRegularBase64Chars<T>(Span<T> chars,
            T regular62, T regular63, T padding,
            T urlsafe62, T urlsafe63)
            where T : IEquatable<T>
        {
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
            for (int idx = chars.LastIndexOf(padding); idx >= 0; idx = chars.LastIndexOf(padding))
            {
                chars = chars.Slice(0, idx);
            }
            return chars;
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        internal static Span<T> ToRegularBase64Chars<T>(Span<T> chars,
            T urlsafe62, T urlsafe63,
            T regular62, T regular63, T padding,
            Span<T> endChars, out bool hasEndChars)
            where T : IEquatable<T>
        {
            Span<T> tmp = chars;
            for (int idx = tmp.IndexOfAny(urlsafe62, urlsafe63); idx >= 0; idx = tmp.IndexOfAny(urlsafe62, urlsafe63))
            {
                T ch = tmp[idx];
                if (ch.Equals(urlsafe62))
                    tmp[idx] = regular62;
                else if (ch.Equals(urlsafe63))
                    tmp[idx] = regular63;
                tmp = tmp.Slice(idx + 1);
            }
            var endCount = chars.Length % 4;
            if (endCount == 0)
            {
                hasEndChars = false;
                return chars;
            }
            int charsCount = chars.Length - endCount;
            chars.Slice(charsCount).CopyTo(endChars);
            for (int idx = endCount; idx < 4; idx++)
                endChars[idx] = padding;
            hasEndChars = true;
            return chars.Slice(0, charsCount);
        }
    }
}
