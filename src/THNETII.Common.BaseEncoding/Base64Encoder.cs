using System;
using System.Text;

namespace THNETII.Common.BaseEncoding
{
    public class Base64Encoder : Decoder
    {
        private readonly bool noPadding;
        private readonly bool urlSafeAlphabet;

        public Base64Encoder(bool urlSafeAlphabet = false, bool noPadding = false)
            : base()
        {
            (this.urlSafeAlphabet, this.noPadding) = (urlSafeAlphabet, noPadding);
        }

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count, bool flush)
        {
            return base.GetCharCount(bytes, index, count, flush);
        }

        /// <inheritdoc />
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, bool flush)
        {
            return base.GetChars(bytes, byteIndex, byteCount, chars, charIndex, flush);
        }

        public override void Convert(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount, bool flush, out int bytesUsed, out int charsUsed, out bool completed)
        {
            base.Convert(bytes, byteIndex, byteCount, chars, charIndex, charCount, flush, out bytesUsed, out charsUsed, out completed);
        }
    }
}
