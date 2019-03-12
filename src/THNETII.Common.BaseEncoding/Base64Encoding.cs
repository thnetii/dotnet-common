using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.BaseEncoding
{
    public static class Base64Encoding
    {
        public const string Alphabet = @"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
        internal const byte SextetMask = 0b11111100;

        private static readonly Dictionary<char, int> ReverseAlphabet =
            Alphabet.Select((ch, i) => (ch, i)).ToDictionary(t => t.ch, t => t.i);

        public static char GetDigit(int sextet) => Alphabet[sextet];

        public static int GetSextet(char digit) => ReverseAlphabet[digit];
    }
}
