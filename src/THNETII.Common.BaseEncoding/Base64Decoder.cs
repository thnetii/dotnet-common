using System;
using System.Text;

namespace THNETII.Common.BaseEncoding
{
    public class Base64Decoder : Encoder
    {
        public Base64Decoder() : base()
        { }

        public override int GetByteCount(char[] chars, int index, int count, bool flush)
        {
            throw new NotImplementedException();
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
        {
            throw new NotImplementedException();
        }

        public override void Convert(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
        {
            base.Convert(chars, charIndex, charCount, bytes, byteIndex, byteCount, flush, out charsUsed, out bytesUsed, out completed);
        }
    }
}
