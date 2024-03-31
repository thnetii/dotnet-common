using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace THNETII.Common.Collections.Generic;

/// <summary>
/// Provides equality comparison for reference type instances by determining equality through Reference Equality.
/// </summary>
/// <remarks>
/// In oder to use equality comparison for value types, the <see cref="EqualityComparer{T}"/> type should be used.
/// </remarks>
public class ReferenceEqualityComparer : IEqualityComparer, IEqualityComparer<object>
{
    /// <summary>
    /// Returns a default reference equality comparer for the type specified by the generic argument.
    /// </summary>
    /// <value>A singleton <see cref="ReferenceEqualityComparer"/> instance that can be shared by all parts of the application.</value>
    public static ReferenceEqualityComparer Instance { get; } =
        new ReferenceEqualityComparer();

    private ReferenceEqualityComparer() { }

    /// <summary>
    /// Determines whether two instances refer to the same object instance.
    /// </summary>
    /// <param name="x">The first object reference.</param>
    /// <param name="y">The second object reference.</param>
    /// <returns><see langword="true"/> if <paramref name="x"/> is the same instance as <paramref name="y"/> or if both are <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
    public new bool Equals(object x, object y) => StaticEquals(x, y);

    /// <summary>
    /// Determines whether two instances refer to the same object instance.
    /// </summary>
    /// <param name="x">The first object reference.</param>
    /// <param name="y">The second object reference.</param>
    /// <returns><see langword="true"/> if <paramref name="x"/> is the same instance as <paramref name="y"/> or if both are <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
    public static bool StaticEquals(object? x, object? y) => ReferenceEquals(x, y);

    /// <summary>
    /// Serves as a hash function for the specified object for hashing algorithms and data structures, such as a hash table.
    /// </summary>
    /// <param name="obj">The object for which to get a hash code.</param>
    /// <returns>A hash code for the specified object, or <c>0</c> (zero) if <paramref name="obj"/> is <see langword="null"/>.</returns>
    public int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
}
