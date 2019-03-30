namespace System
{
    public static class CharTestExtensions
    {
        public static int IndexOfSurrogatePair(this string s, int startIdx = 0)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));

            for (int i = startIdx; i < s.Length; i++)
            {
                if (char.IsSurrogatePair(s, i))
                    return i;
            }

            return -1;
        }

        public static int LastIndexOfSurrogatePair(this string s, int startIdx = -1)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (startIdx < 0)
                startIdx = s.Length - 1;

            for (int i = startIdx; i >= 0; i--)
            {
                if (char.IsSurrogatePair(s, i))
                    return i;
            }

            return -1;
        }
    }
}
