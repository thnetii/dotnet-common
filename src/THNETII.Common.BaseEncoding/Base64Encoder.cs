using System;
using System.Text;

namespace THNETII.Common.BaseEncoding
{
    public class Base64Encoder : Decoder
    {
        private readonly bool noPadding;
        private readonly bool urlSafeAlphabet;
        private byte buffer;
        private int bitsRemaining;

        public Base64Encoder(bool urlSafeAlphabet = false, bool noPadding = false)
            : base()
        {
            (this.urlSafeAlphabet, this.noPadding) = (urlSafeAlphabet, noPadding);
        }

        public override void Reset()
        {
            buffer = 0;
            bitsRemaining = 0;
        }

#if !NETSTANDARD1_3
        /// <inheritdoc />
        public override unsafe int GetCharCount(byte* bytes, int count, bool flush)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            return GetCharCount(new Span<byte>(bytes, count), flush);
        }
#endif // !NETSTANDARD1_3

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count) =>
            GetCharCount(bytes, index, count, flush: true);

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count,
            bool flush)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            return GetCharCount(new Span<byte>(bytes, index, count), flush);
        }

        public int GetCharCount(Span<byte> bytes, bool flush)
        {
            int totalBits = bytes.Length * 8 + bitsRemaining;
#if NETSTANDARD1_3
            (int sextets, int overlapBits) = (totalBits / 6, totalBits % 6);
#else // !NETSTANDARD1_3
            int sextets = Math.DivRem(totalBits, 6, out int overlapBits);
#endif // !NETSTANDARD1_3
            if (overlapBits != 0 && flush)
            {
                int padding = 0;
                if (!noPadding)
                {
                    switch (overlapBits)
                    {
                        case 2:
                            padding = 2;
                            break;
                        case 4:
                            padding = 1;
                            break;
                    }
                }
                sextets += 1 + padding;
            }
            return sextets;
        }

        /// <inheritdoc />
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount,
            char[] chars, int charIndex) =>
            GetChars(bytes, byteIndex, byteCount, chars, charIndex, flush: true);

        /// <inheritdoc />
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount,
            char[] chars, int charIndex, bool flush)
        {

            return base.GetChars(bytes, byteIndex, byteCount, chars, charIndex, flush);
        }

        /// <inheritdoc />
        public override void Convert(byte[] bytes, int byteIndex, int byteCount,
            char[] chars, int charIndex, int charCount, bool flush,
            out int bytesUsed, out int charsUsed, out bool completed)
        {
            base.Convert(bytes, byteIndex, byteCount, chars, charIndex, charCount, flush, out bytesUsed, out charsUsed, out completed);
        }
    }
}
