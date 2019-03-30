using System;
using System.Buffers;
using System.Threading;

namespace THNETII.Common.Buffers
{
    public struct ArrayDisposableSegment<T> : IDisposable, IEquatable<ArrayDisposableSegment<T>>, IEquatable<ArraySegment<T>>
    {
        private ArraySegment<T> segment;
        private IMemoryOwner<T> memoryOwner;
        private Memory<T> originalMemory;

        public ArraySegment<T> Segment => memoryOwner is null
            ? throw new ObjectDisposedException(nameof(memoryOwner))
            : segment;

        public bool IsDisposable => memoryOwner is IDisposable;

        internal ArrayDisposableSegment(T[] array,
            IMemoryOwner<T> memoryOwner = null, Memory<T> originalMemory = default)
            : this(new ArraySegment<T>(array), memoryOwner, originalMemory) { }

        internal ArrayDisposableSegment(T[] array, int offset, int length,
            IMemoryOwner<T> memoryOwner = null, Memory<T> originalMemory = default)
            : this(new ArraySegment<T>(array, offset, length), memoryOwner, originalMemory) { }

        internal ArrayDisposableSegment(ArraySegment<T> segment,
            IMemoryOwner<T> memoryOwner = null, Memory<T> originalMemory = default)
            : this()
        {
            (this.segment, this.memoryOwner, this.originalMemory) =
                (segment, memoryOwner, originalMemory);
        }

        public void Dispose()
        {
            var memoryOwner = Interlocked.Exchange(ref this.memoryOwner, null);
            segment = default;
            Memory<T> originalMemory;
            (originalMemory, this.originalMemory) = (this.originalMemory, default);
            _ = memoryOwner?.Memory.TryCopyTo(originalMemory);
            memoryOwner?.Dispose();
        }

        public override int GetHashCode() => segment.GetHashCode();

        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case ArrayDisposableSegment<T> other:
                    return Equals(other);
                case ArraySegment<T> other:
                    return Equals(other);
                default:
                    return false;
            }
        }

        public bool Equals(ArrayDisposableSegment<T> other)
        {
            return Equals(other.segment)
                && memoryOwner.Equals(other.memoryOwner)
                && originalMemory.Equals(other.originalMemory);
        }

        public bool Equals(ArraySegment<T> other) => segment.Equals(other);

        public static bool operator ==(ArrayDisposableSegment<T> left, ArrayDisposableSegment<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ArrayDisposableSegment<T> left, ArrayDisposableSegment<T> right)
        {
            return !(left == right);
        }

        public static bool operator ==(ArrayDisposableSegment<T> left, ArraySegment<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ArrayDisposableSegment<T> left, ArraySegment<T> right)
        {
            return !(left == right);
        }

        public static bool operator ==(ArraySegment<T> left, ArrayDisposableSegment<T> right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(ArraySegment<T> left, ArrayDisposableSegment<T> right)
        {
            return !(right == left);
        }
    }
}
