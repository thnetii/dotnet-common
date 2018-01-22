using System;
using System.Collections.Generic;

namespace THNETII.Common.Collections.Generic
{
    /// <summary>
    /// Defines methods to support the comparison of weak references for equality.
    /// </summary>
    /// <typeparam name="T">The type of the object that is referenced by the weak reference.</typeparam>
    public class WeakReferenceEqualityComparer<T> : IEqualityComparer<WeakReference<T>> where T : class
    {
        private static readonly WeakReferenceEqualityComparer<T> @default = new WeakReferenceEqualityComparer<T>();

#pragma warning disable CA1000 // Do not declare static members on generic types
        /// <summary>
        /// Returns a default weak reference equality comparer instance for the type referenced by the generic argument.
        /// </summary>
        public static WeakReferenceEqualityComparer<T> Default => @default;
#pragma warning restore CA1000 // Do not declare static members on generic types

        /// <inheritdoc />
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

        /// <inheritdoc />
        public int GetHashCode(WeakReference<T> obj)
        {
            if (!(obj is null) && obj.TryGetTarget(out var target))
                return ReferenceEqualityComparer<T>.Default.GetHashCode(target);
            return default;
        }
    }
}
