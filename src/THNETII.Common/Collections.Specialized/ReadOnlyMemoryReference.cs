using System;

namespace THNETII.Common.Collections.Specialized
{
    /// <summary>
    /// A read-only reference to a Read-Only Memory block and the index into
    /// that memory.
    /// </summary>
    /// <typeparam name="T">The type of the values stored within the memory.</typeparam>
    public struct ReadOnlyMemoryReference<T> : IEquatable<ReadOnlyMemoryReference<T>>
    {
        /// <summary>
        /// Gets the read-only memory that stores the referenced value at
        /// position <see cref="Index"/>.
        /// </summary>
        /// <value>
        /// A <see cref="ReadOnlyMemory{T}"/> value. If the
        /// <see cref="ReadOnlyMemory{T}.IsEmpty"/> property of
        /// <see cref="Memory"/> is <c>true</c>, <see cref="Index"/> is <c>0</c>
        /// (zero).
        /// </value>
        public ReadOnlyMemory<T> Memory { get; }

        /// <summary>
        /// Gets the index to the value that is referenced within
        /// <see cref="Memory"/>.
        /// </summary>
        /// <value>
        /// A non-negative index into the <see cref="ReadOnlySpan{T}"/>
        /// returned by the <see cref="ReadOnlyMemory{T}.Span"/> property of
        /// <see cref="Memory"/>. If the <see cref="ReadOnlyMemory{T}.IsEmpty"/>
        /// property of <see cref="Memory"/> returns <c>true</c>, the value is
        /// <c>0</c> (zero).
        /// </value>
        public int Index { get; }

        /// <summary>
        /// Creates a new reference to a value within a read-only memory block
        /// at the specified position.
        /// </summary>
        /// <param name="memory">The read-only memory within which the value is stored.</param>
        /// <param name="index">
        /// The index into the <see cref="ReadOnlySpan{T}"/> returned by the
        /// <see cref="ReadOnlyMemory{T}.Span"/> property of <paramref name="memory"/>.
        /// Ignored if <paramref name="memory"/> is empty, otherwise must be a
        /// valid index into <paramref name="memory"/>.
        /// </param>
        /// <remarks>
        /// If <paramref name="memory"/> is empty, the value of <paramref name="index"/>
        /// is ignored and the <see cref="Index"/> property is set to
        /// <c>0</c> (zero).
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="memory"/> is not empty and <paramref name="index"/>
        /// is negative or greater than or equal to the
        /// <see cref="ReadOnlyMemory{T}.Length"/> of <paramref name="memory"/>.
        /// </exception>
        public ReadOnlyMemoryReference(ReadOnlyMemory<T> memory, int index)
        {
            if (memory.IsEmpty)
                index = 0;
            else if (index < 0 || index >= memory.Length)
                throw new ArgumentOutOfRangeException(paramName: nameof(index),
                    index, message: new IndexOutOfRangeException().Message);
            (Memory, Index) = (memory, index);
        }

        /// <inheritdoc cref="ValueType.GetHashCode" />
        public override int GetHashCode() => Index.GetHashCode();

        /// <inheritdoc cref="ValueType.Equals" />
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case ReadOnlyMemoryReference<T> otherReference:
                    return Equals(otherReference);
                case null:
                default:
                    return false;
            }
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(ReadOnlyMemoryReference<T> otherReference) =>
            Memory.Equals(otherReference) && Index == otherReference.Index;

        /// <summary />
        /// <seealso cref="Equals(ReadOnlyMemoryReference{T})"/>
        public static bool operator ==(ReadOnlyMemoryReference<T> left, ReadOnlyMemoryReference<T> right) =>
            left.Equals(right);

        /// <summary />
        public static bool operator !=(ReadOnlyMemoryReference<T> left, ReadOnlyMemoryReference<T> right) =>
            !left.Memory.Equals(right.Memory) || left.Index != right.Index;
    }
}
