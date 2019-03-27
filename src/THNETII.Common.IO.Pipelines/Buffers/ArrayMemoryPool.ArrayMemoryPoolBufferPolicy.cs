
using Microsoft.Extensions.ObjectPool;

namespace THNETII.Common.Buffers
{
    public partial class ArrayMemoryPool<T>
    {
        private class ArrayMemoryPoolBufferPolicy : IPooledObjectPolicy<ArrayMemoryPoolBuffer>
        {
            private readonly ArrayMemoryPool<T> arrayMemoryPool;

            public ArrayMemoryPoolBufferPolicy(ArrayMemoryPool<T> arrayMemoryPool)
            {
                this.arrayMemoryPool = arrayMemoryPool;
            }

            public ArrayMemoryPoolBuffer Create()
            {
                return new ArrayMemoryPoolBuffer(arrayMemoryPool.bufferPool);
            }

            public bool Return(ArrayMemoryPoolBuffer buffer)
            {
                if (buffer is null)
                    return false;
                var array = buffer.Array;
                buffer.Array = null;
                if (array is T[])
                    arrayMemoryPool.arrayPool.Return(array, arrayMemoryPool.clearOnReturn);
                return true;
            }
        }
    }
}
