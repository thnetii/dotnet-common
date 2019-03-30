using System;
using System.Threading;

namespace THNETII.Common.Buffers
{
    public struct ArrayDisposableSegment<T> : IDisposable, IEquatable<ArrayDisposableSegment<T>>, IEquatable<ArraySegment<T>>
    {
        private ArraySegment<T> segment;
        private IDisposable disposable;
        private Memory<T> originalMemory;

        public ArraySegment<T> Segment => disposable is null
            ? throw new ObjectDisposedException(nameof(disposable))
            : segment;

        public ArrayDisposableSegment(T[] array,
            IDisposable disposable = null, Memory<T> originalMemory = default)
            : this(new ArraySegment<T>(array), disposable, originalMemory) { }

        public ArrayDisposableSegment(T[] array, int offset, int length,
            IDisposable disposable = null, Memory<T> originalMemory = default)
            : this(new ArraySegment<T>(array, offset, length), disposable, originalMemory) { }

        public ArrayDisposableSegment(ArraySegment<T> segment,
            IDisposable disposable = null, Memory<T> originalMemory = default)
            : this()
        {
            (this.segment, this.disposable, this.originalMemory) =
                (segment, disposable, originalMemory);
        }

        /// <summary>
        /// Disposes this instance. If set on construction, it will copy the
        /// data from <see cref="Segment"/> back to the original memory
        /// destination. Subsequent accesses to <see cref="Segment"/> will throw
        /// <see cref="ObjectDisposedException"/>.
        /// </summary>
        public void Dispose()
        {
            var disposable = Interlocked.Exchange(ref this.disposable, null);
            ArraySegment<T> segment;
            Memory<T> originalMemory;
            (segment, this.segment) = (this.segment, default);
            (originalMemory, this.originalMemory) = (this.originalMemory, default);
            _ = segment.AsMemory().TryCopyTo(originalMemory);
            disposable?.Dispose();
        }

        /// <summary />
        public override int GetHashCode() => segment.GetHashCode();

        /// <summary />
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

        /// <summary />
        public bool Equals(ArrayDisposableSegment<T> other)
        {
            return Equals(other.segment)
                && disposable.Equals(other.disposable)
                && originalMemory.Equals(other.originalMemory);
        }

        /// <summary />
        public bool Equals(ArraySegment<T> other) => segment.Equals(other);

        /// <summary />
        public static bool operator ==(ArrayDisposableSegment<T> left, ArrayDisposableSegment<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(ArrayDisposableSegment<T> left, ArrayDisposableSegment<T> right)
        {
            return !(left == right);
        }

        /// <summary />
        public static bool operator ==(ArrayDisposableSegment<T> left, ArraySegment<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(ArrayDisposableSegment<T> left, ArraySegment<T> right)
        {
            return !(left == right);
        }

        /// <summary />
        public static bool operator ==(ArraySegment<T> left, ArrayDisposableSegment<T> right)
        {
            return right.Equals(left);
        }

        /// <summary />
        public static bool operator !=(ArraySegment<T> left, ArrayDisposableSegment<T> right)
        {
            return !(right == left);
        }
    }
}
