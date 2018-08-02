using System;
using System.Collections.Generic;
using System.Text;

namespace THNETII.Common
{
    public static class StringCommonExtensions
    {
        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            var v = value.AsSpan();
            for (var remaining = s.AsSpan(); remaining.Length >= value.Length;
                remaining = remaining.Slice(1))
            {
                if (remaining.StartsWith(v, comparisonType))
                    return true;
            }
            return false;
        }
    }
}
