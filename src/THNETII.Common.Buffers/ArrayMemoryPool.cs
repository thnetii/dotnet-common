using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.ObjectPool;

namespace THNETII.Common.Buffers
{
    /// <summary>
    /// Provides a resource pool that enables re-using array instances of type
    /// <typeparamref name="T"/> that are wrapped in disposable
    /// <see cref="IMemoryOwner{T}"/> instances.
    /// </summary>
    /// <typeparam name="T">The type of the objects that are in the resource pool.</typeparam>
    public partial class ArrayMemoryPool<T> : MemoryPool<T>
    {
        private const int MaximumBufferSize = int.MaxValue;

        /// <summary>
        /// Gets a shared <see cref="ArrayMemoryPool{T}"/> instance.
        /// </summary>
        /// <value>A shared <see cref="ArrayMemoryPool{T}"/> instance that uses the <see cref="ArrayPool{T}.Shared"/> array pool.</value>
        [SuppressMessage("Design", "CA1000: Do not declare static members on generic types", Justification = nameof(MemoryPool<T>.Shared))]
        public new static ArrayMemoryPool<T> Shared { get; } =
            new ArrayMemoryPool<T>(ArrayPool<T>.Shared);

        /// <summary>
        /// Gets a shared <see cref="ArrayMemoryPool{T}"/> instance that
        /// clears rented array instances as they are disposed.
        /// </summary>
        /// <remarks>
        /// This instance instructs the underlying <see cref="ArrayPool{T}"/> to
        /// clear the rented array when the <see cref="IDisposable.Dispose"/>
        /// method is called on the rented <see cref="IMemoryOwner{T}"/> instance.
        /// Subsequent callers to the <see cref="Rent(int)"/> method will not
        /// see the content of the previous caller. This is useful when the rented
        /// memory contains user sensitive or security critical data.
        /// </remarks>
        /// <value>A shared <see cref="ArrayMemoryPool{T}"/> instance that uses the <see cref="ArrayPool{T}.Shared"/> array pool.</value>
        [SuppressMessage("Design", "CA1000: Do not declare static members on generic types", Justification = nameof(Shared))]
        public static ArrayMemoryPool<T> Secure { get; } =
            new ArrayMemoryPool<T>(ArrayPool<T>.Shared, clearOnReturn: true);

        private readonly ArrayPool<T> arrayPool;
        private readonly ObjectPool<ArrayMemoryPoolBuffer> bufferPool;
        private readonly bool clearOnReturn;

        /// <inheritdoc cref="MemoryPool{T}.MaxBufferSize" />
        public override int MaxBufferSize => MaximumBufferSize;

        internal ArrayMemoryPool() : base()
        {
            bufferPool = new DefaultObjectPool<ArrayMemoryPoolBuffer>(new ArrayMemoryPoolBufferPolicy(this));
        }

        public ArrayMemoryPool(ArrayPool<T> arrayPool, bool clearOnReturn = false) : this()
        {
            if (arrayPool is null)
                throw new ArgumentNullException(nameof(arrayPool));
            (this.arrayPool, this.clearOnReturn) = (arrayPool, clearOnReturn);
        }


        public override IMemoryOwner<T> Rent(int minimumBufferSize = -1)
        {
            if (minimumBufferSize == -1)
                minimumBufferSize = 1 + (4095 / Unsafe.SizeOf<T>());

            T[] array;
            try { array = arrayPool.Rent(minimumBufferSize); }
            catch (ArgumentOutOfRangeException argExcept)
            { throw new ArgumentOutOfRangeException(nameof(minimumBufferSize), minimumBufferSize, argExcept.Message); }

            var buffer = bufferPool.Get();
            buffer.Array = array;

            return buffer;
        }

        /// <summary>
        /// This method has no effect as there are no native resources to be
        /// cleaned up the pool itself.
        /// </summary>
        protected override void Dispose(bool disposing) { }
    }
}
