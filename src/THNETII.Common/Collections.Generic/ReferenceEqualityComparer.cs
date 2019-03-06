using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.Common.Collections.Generic
{
    /// <summary>
    /// Provides equality comparison for reference type instances by determining equality through Reference Equality.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    /// <remarks>
    /// In oder to use equality comparison for value types, the <see cref="EqualityComparer{T}"/> type should be used.
    /// </remarks>
    public class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
    {
        /// <summary>
        /// Returns a default reference equality comparer for the type specified by the generic argument.
        /// </summary>
        /// <value>A singleton <see cref="ReferenceEqualityComparer{T}"/> instance that can be shared by all parts of the application.</value>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static ReferenceEqualityComparer<T> Default { get; } =
            new ReferenceEqualityComparer<T>();

        /// <summary>
        /// Determines whether two instances of type <typeparamref name="T"/> refer to the same object instance.
        /// </summary>
        /// <param name="x">The first object reference.</param>
        /// <param name="y">The second object reference.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> is the same instance as <paramref name="y"/> or if both are <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
        public bool Equals(T x, T y) => StaticEquals(x, y);

        /// <summary>
        /// Determines whether two instances of type <typeparamref name="T"/> refer to the same object instance.
        /// </summary>
        /// <param name="x">The first object reference.</param>
        /// <param name="y">The second object reference.</param>
        /// <returns><see langword="true"/> if <paramref name="x"/> is the same instance as <paramref name="y"/> or if both are <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1000")]
        public static bool StaticEquals(T x, T y) => ReferenceEquals(x, y);

        /// <summary>
        /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
        /// </summary>
        /// <param name="obj">The object for which to get a hash code.</param>
        /// <returns>A hash code for the specified object, or <c>0</c> (zero) if <paramref name="obj"/> is <see langword="null"/>.</returns>
        public int GetHashCode(T obj) => obj?.GetHashCode() ?? 0;
    }
}
