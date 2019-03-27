using System;
using System.Buffers;

using Microsoft.Extensions.ObjectPool;

namespace THNETII.Common.Buffers
{
    public static class MemoryOwnerExtensions
    {
        internal sealed class WrappingMemoryOwner<T> : IMemoryOwner<T>
        {
            public static ObjectPool<WrappingMemoryOwner<T>> ObjectPool { get; } =
                new DefaultObjectPool<WrappingMemoryOwner<T>>(new DefaultPooledObjectPolicy<WrappingMemoryOwner<T>>());

            internal ObjectPool<WrappingMemoryOwner<T>> objectPool;

            internal IMemoryOwner<T> OriginalOwner { get; set; }

            public Memory<T> Memory { get; internal set; }

            public void Dispose()
            {
                var originalOwner = OriginalOwner;
                if (originalOwner is IMemoryOwner<T>)
                {
                    OriginalOwner = null;
                    originalOwner.Dispose();
                }
                var objectPool = this.objectPool;
                if (objectPool is ObjectPool<WrappingMemoryOwner<T>>)
                {
                    this.objectPool = null;
                    objectPool.Return(this);
                }
            }
        }

        public static IMemoryOwner<T> Slice<T>(this IMemoryOwner<T> memoryOwner, int start)
        {
            if (memoryOwner is null)
                throw new ArgumentNullException(nameof(memoryOwner));

            var slicePool = WrappingMemoryOwner<T>.ObjectPool;
            var sliceOwner = slicePool.Get();
            sliceOwner.objectPool = slicePool;
            sliceOwner.OriginalOwner = memoryOwner;

            try { sliceOwner.Memory = memoryOwner.Memory.Slice(start); }
            catch (Exception) when (ReturnPooledObject()) { throw; }

            return sliceOwner;

            bool ReturnPooledObject()
            {
                slicePool.Return(sliceOwner);
                return false; // Return false to not catch exception
            }
        }

        public static IMemoryOwner<T> Slice<T>(this IMemoryOwner<T> memoryOwner, int start, int length)
        {
            if (memoryOwner is null)
                throw new ArgumentNullException(nameof(memoryOwner));

            var slicePool = WrappingMemoryOwner<T>.ObjectPool;
            var sliceOwner = slicePool.Get();
            sliceOwner.objectPool = slicePool;
            sliceOwner.OriginalOwner = memoryOwner;

            try { sliceOwner.Memory = memoryOwner.Memory.Slice(start, length); }
            catch (Exception) when (ReturnPooledObject()) { throw; }

            return sliceOwner;

            bool ReturnPooledObject()
            {
                slicePool.Return(sliceOwner);
                return false; // Return false to not catch exception
            }
        }
    }
}
