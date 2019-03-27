using System;
using System.Buffers;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.ObjectPool;

namespace THNETII.Common.Buffers
{
    public partial class ArrayMemoryPool<T> : MemoryPool<T>
    {
        private const int MaximumBufferSize = int.MaxValue;

        public new static ArrayMemoryPool<T> Shared { get; } =
            new ArrayMemoryPool<T>(ArrayPool<T>.Shared);

        private readonly ArrayPool<T> arrayPool;
        private readonly ObjectPool<ArrayMemoryPoolBuffer> bufferPool;
        private readonly bool clearOnReturn;

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

        protected override void Dispose(bool disposing) { }
    }
}
