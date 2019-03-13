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

        private bool UsePadding => !noPadding;

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

        /// <inheritdoc />
        public
#if !NETSTANDARD1_3
        override
#endif // !NETSTANDARD1_3
        unsafe int GetCharCount(byte* bytes, int count, bool flush)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            ReadOnlySpan<byte> byteSpan;
            try { byteSpan = new ReadOnlySpan<byte>(bytes, count); }
            catch (ArgumentOutOfRangeException lengthExcept) when (lengthExcept.ParamName == "length")
            { throw new ArgumentOutOfRangeException(nameof(count), count, lengthExcept.Message); }

            return GetCharCount(byteSpan, flush);
        }

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count) =>
            GetCharCount(bytes, index, count, flush: true);

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count,
            bool flush)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            ReadOnlySpan<byte> byteSpan;
            try { byteSpan = new ReadOnlySpan<byte>(bytes, index, count); }
            catch (ArgumentOutOfRangeException startExcept) when (startExcept.ParamName == "start")
            { throw new ArgumentOutOfRangeException(nameof(index), index, startExcept.Message); }
            catch (ArgumentOutOfRangeException lengthExcept) when (lengthExcept.ParamName == "length")
            { throw new ArgumentOutOfRangeException(nameof(index), index, lengthExcept.Message); }

            return GetCharCount(byteSpan, flush);
        }

        public int GetCharCount(ReadOnlySpan<byte> bytes, bool flush)
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
                if (UsePadding)
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
        public
#if !NETSTANDARD1_3
        override
#endif // !NETSTANDARD1_3
        unsafe int GetChars(byte* bytes, int byteCount,
            char* chars, int charCount, bool flush)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));

            ReadOnlySpan<byte> byteSpan;
            try { byteSpan = new ReadOnlySpan<byte>(bytes, byteCount); }
            catch (ArgumentOutOfRangeException lengthExcept) when (lengthExcept.ParamName == "length")
            { throw new ArgumentOutOfRangeException(nameof(byteCount), byteCount, lengthExcept.Message); }

            Span<char> charSpan;
            try { charSpan = new Span<char>(chars, charCount); }
            catch (ArgumentOutOfRangeException lengthExcept) when (lengthExcept.ParamName == "length")
            { throw new ArgumentOutOfRangeException(nameof(charCount), byteCount, lengthExcept.Message); }

            return GetChars(byteSpan, charSpan, flush);
        }

        /// <inheritdoc />
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount,
            char[] chars, int charIndex) =>
            GetChars(bytes, byteIndex, byteCount, chars, charIndex, flush: true);

        /// <inheritdoc />
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount,
            char[] chars, int charIndex, bool flush)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));

            ReadOnlySpan<byte> byteSpan;
            try { byteSpan = new ReadOnlySpan<byte>(bytes, byteIndex, byteCount); }
            catch (ArgumentOutOfRangeException startExcept) when (startExcept.ParamName == "start")
            { throw new ArgumentOutOfRangeException(nameof(byteIndex), byteIndex, startExcept.Message); }
            catch (ArgumentOutOfRangeException lengthExcept) when (lengthExcept.ParamName == "length")
            { throw new ArgumentOutOfRangeException(nameof(byteCount), byteCount, lengthExcept.Message); }

            Span<char> charSpan;
            try { charSpan = new Span<char>(chars, charIndex, chars.Length - charIndex); }
            catch (ArgumentOutOfRangeException startExcept) when (startExcept.ParamName == "start")
            { throw new ArgumentOutOfRangeException(nameof(charIndex), charIndex, startExcept.Message); }
            catch (ArgumentOutOfRangeException lengthExcept) when (lengthExcept.ParamName == "length")
            { throw new ArgumentOutOfRangeException(nameof(charIndex), byteCount, lengthExcept.Message); }

            return GetChars(byteSpan, charSpan, flush);
        }

        public int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars, bool flush)
        {
            throw new NotImplementedException();
        }
    }
}
