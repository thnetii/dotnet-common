using System.Collections.Generic;

namespace THNETII.Common.Collections.Generic
{
    public class ReferenceEqualityComparer<T> : IEqualityComparer<T>
    {
        private static readonly ReferenceEqualityComparer<T> @default = new ReferenceEqualityComparer<T>();

        public static ReferenceEqualityComparer<T> Default => @default;

        public bool Equals(T x, T y) => StaticEquals(x, y);

        public static bool StaticEquals(T x, T y) => ReferenceEquals(x, y);

        public int GetHashCode(T obj) => obj.GetHashCode();
    }
}
