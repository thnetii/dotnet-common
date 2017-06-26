using System.Collections.Generic;

namespace THNETII.Common.Collections.Generic
{
    /// <summary>
    /// Provides equality comparison for reference type instances by determining equality through Reference Equality.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <remarks>
    /// Although a value type can be specified for <typeparamref name="T"/>, two values cannot evaluate to reference equal, as both values are boxed and placed in different memory locations when being compared to each other.
    /// <para>In oder to use equality comparison for value types, the <see cref="EqualityComparer{T}"/> type should be used.</para>
    /// </remarks>
    public class ReferenceEqualityComparer<T> : IEqualityComparer<T>
    {
        private static readonly ReferenceEqualityComparer<T> @default = new ReferenceEqualityComparer<T>();

        /// <summary>
        /// Returns a default reference equality comparer for the type specified by the generic argument.
        /// </summary>
        public static ReferenceEqualityComparer<T> Default => @default;

        /// <summary>
        /// Determines whether two instances of type <typeparamref name="T"/> refer to the same object instance.
        /// </summary>
        /// <param name="x">The first object instance.</param>
        /// <param name="y">The second object reference.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is the same instance as <paramref name="y"/> or if both are <c>null</c>; otherwise, <c>false</c>.</returns>
        public bool Equals(T x, T y) => StaticEquals(x, y);

        /// <summary>
        /// Determines whether two instances of type <typeparamref name="T"/> refer to the same object instance.
        /// </summary>
        /// <param name="x">The first object instance.</param>
        /// <param name="y">The second object reference.</param>
        /// <returns><c>true</c> if <paramref name="x"/> is the same instance as <paramref name="y"/> or if both are <c>null</c>; otherwise, <c>false</c>.</returns>
        public static bool StaticEquals(T x, T y) => ReferenceEquals(x, y);

        /// <summary>
        /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object, or <c>0</c> (zero) if <paramref name="obj"/> is <c>null</c>.</returns>
        public int GetHashCode(T obj) => obj?.GetHashCode() ?? 0;
    }
}
