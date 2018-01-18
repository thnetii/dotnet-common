using System;
using System.Collections.Generic;

namespace THNETII.Common
{
    public static class WeakReferenceComparer
    {
        public static WeakReferenceComparer<T> GetDefault<T>() where T : class => WeakReferenceComparer<T>.Default;
    }

    public class WeakReferenceComparer<T> : IEqualityComparer<WeakReference<T>> where T : class
    {
        private static readonly WeakReferenceComparer<T> @default = new WeakReferenceComparer<T>();

        internal static WeakReferenceComparer<T> Default => @default;

        public bool Equals(WeakReference<T> x, WeakReference<T> y)
        {
            if (ReferenceEquals(x, y))
                return true;
            else if (x is null || y is null)
                return false;
            return x.TryGetTarget(out var xRef)
                && y.TryGetTarget(out var yRef)
                && ReferenceEquals(xRef, yRef);
        }

        public int GetHashCode(WeakReference<T> obj)
        {
            if (!(obj is null) && obj.TryGetTarget(out var target))
                return target?.GetHashCode() ?? default;
            return default;
        }
    }
}
