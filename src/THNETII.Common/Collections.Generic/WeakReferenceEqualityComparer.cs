using System;
using System.Collections.Generic;

namespace THNETII.Common.Collections.Generic
{
    public class WeakReferenceEqualityComparer<T> : IEqualityComparer<WeakReference<T>> where T : class
    {
        private static readonly WeakReferenceEqualityComparer<T> @default = new WeakReferenceEqualityComparer<T>();

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static WeakReferenceEqualityComparer<T> Default => @default;
#pragma warning restore CA1000 // Do not declare static members on generic types

        public bool Equals(WeakReference<T> x, WeakReference<T> y)
        {
            if (ReferenceEquals(x, y))
                return true;
            else if (x is null || y is null)
                return false;
            return x.TryGetTarget(out var xRef)
                && y.TryGetTarget(out var yRef)
                && ReferenceEqualityComparer<T>.Default.Equals(xRef, yRef);
        }

        public int GetHashCode(WeakReference<T> obj)
        {
            if (!(obj is null) && obj.TryGetTarget(out var target))
                return ReferenceEqualityComparer<T>.Default.GetHashCode(target);
            return default;
        }
    }
}
