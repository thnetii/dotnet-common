using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.Common.Collections.Generic
{
    /// <summary>
    /// Defines methods to support the comparison of weak references for equality.
    /// </summary>
    /// <typeparam name="T">The type of the object that is referenced by the weak reference.</typeparam>
    public class WeakReferenceEqualityComparer<T> : IEqualityComparer<WeakReference<T>> where T : class
    {
        /// <summary>
        /// Returns a default weak reference equality comparer instance for the type referenced by the generic argument.
        /// </summary>
        /// <value>A singleton <see cref="WeakReferenceEqualityComparer{T}"/> instance that can be shared by all parts of the application.</value>
        [SuppressMessage("Design", "CA1000: Do not declare static members on generic types")]
        public static WeakReferenceEqualityComparer<T> Default { get; } =
            new WeakReferenceEqualityComparer<T>();

        /// <summary>
        /// Determines whether two instances of type <typeparamref name="T"/> refer to the same object instance.
        /// </summary>
        /// <param name="x">The first object reference.</param>
        /// <param name="y">The second object reference.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> is the same instance as <paramref name="y"/> or if both are <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// Equality between two weak references is determined by reference equality of the two instances or by satisfying all of the following criteria:
        /// <list type="number">
        /// <item><term>Neither <see cref="WeakReference{T}"/> instance is <see langword="null"/>. (If both were <see langword="null"/>, they would be reference equal.)</term></item>
        /// <item><term>Getting the target reference (by invoking <see cref="WeakReference{T}.TryGetTarget(out T)"/>) of both <see cref="WeakReference{T}"/> instances succeeds.</term></item>
        /// <item><term>The targets of both <see cref="WeakReference{T}"/> instances are reference equal.</term></item>
        /// </list>
        /// </remarks>
        public bool Equals(WeakReference<T>? x, WeakReference<T>? y)
        {
            if (ReferenceEquals(x, y))
                return true;
            else if (x is null || y is null)
                return false;
            return x.TryGetTarget(out var xRef)
                && y.TryGetTarget(out var yRef)
                && ReferenceEqualityComparer.Instance.Equals(xRef, yRef);
        }

        /// <summary>
        /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object, or <c>0</c> (zero) if <paramref name="obj"/> is <see langword="null"/>.</returns>
        public int GetHashCode(WeakReference<T>? obj)
        {
            if (!(obj is null) && obj.TryGetTarget(out var target))
                return ReferenceEqualityComparer.Instance.GetHashCode(target);
            return default;
        }
    }
}
