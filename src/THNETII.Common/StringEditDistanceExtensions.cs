using System;

namespace THNETII.Common
{
    /// <summary>
    /// Provides extension methods for strings to calculate the edit-distance between two strings.
    /// </summary>
    public static class StringEditDistanceExtensions
    {
        /// <summary>
        /// Returns the edit distance from the current string <paramref name="a"/> to the other string <paramref name="b"/>.
        /// <para>The edit distance between two strings, is the number of character edit operations needed in order to change one string to another.</para>
        /// </summary>
        /// <param name="a">The original string.</param>
        /// <param name="b">The other string to calculate the distance to.</param>
        /// <returns>The number of characters that need to be changed, inserted or deleted in <paramref name="a"/> to produce <paramref name="b"/>.</returns>
        /// <remarks>
        /// This uses the <a href="https://en.wikipedia.org/wiki/Levenshtein_distance">Levenshtein distance algorithm</a> with only a linear
        /// memory overhead.
        /// <para>This implementation is highly optimized!</para>
        /// <para>The original implementation is taken from the <c>editDistance</c> implementation found in the standard library for the Nim programming language.</para>
        /// </remarks>
        public static int EditDistance(this string a, string b)
        {
            int len_a = a?.Length ?? 0;
            int len_b = b?.Length ?? 0;

            // Make b the longer string
            if (len_a > len_b)
            {
                return EditDistance(b, a);
            }

            // Strip common prefix:
            int s, s_bound = len_a;
            for (s = 0; s < s_bound && a[s] == b[s]; s++)
            {
                len_a--;
                len_b--;
            }

            // Strip common suffix:
            while (len_a > 0 && len_b > 0 && a[s + len_a - 1] == b[s + len_b - 1])
            {
                len_a--;
                len_b--;
            }

            // Trivial cases:
            if (len_a == 0) return len_b;
            if (len_b == 0) return len_a;

            // another special case:
            if (len_a == 1)
            {
                int j_bound = s + len_b;
                for (int j = s; j < j_bound; j++)
                    if (a[s] == b[j]) return len_b - 1;
                return len_b;
            }

            len_a++;
            len_b++;
            var row = new int[len_b];
            for (int i = 0; i < len_b; i++)
                row[i] = i;
            for (int i = 1; i < len_a; i++)
            {
                char char1 = a[s + i - 1];
                int prev_cost = i - 1;
                int new_cost = i;

                for (int j = 1; j < len_b; j++)
                {
                    char char2 = b[s + j - 1];

                    if (char1 == char2)
                        new_cost = prev_cost;
                    else
                        new_cost = Math.Min(new_cost, Math.Min(prev_cost, row[j])) + 1;

                    prev_cost = row[j];
                    row[j] = new_cost;
                }
            }

            return row[len_b - 1];
        }
    }
}
