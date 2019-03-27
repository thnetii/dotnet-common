using System;
using System.Buffers;

using Microsoft.Extensions.ObjectPool;

namespace THNETII.Common.Buffers
{
    public partial class ArrayMemoryPool<T>
    {
        private sealed class ArrayMemoryPoolBuffer : IMemoryOwner<T>
        {
            private T[] array;
            private Memory<T> memory;
            private ObjectPool<ArrayMemoryPoolBuffer> bufferPool;

            public ArrayMemoryPoolBuffer(ObjectPool<ArrayMemoryPoolBuffer> bufferPool)
            {
                this.bufferPool = bufferPool;
            }


            internal T[] Array
            {
                get => array;
                set
                {
                    array = value;
                    memory = value;
                }
            }

            public Memory<T> Memory
            {
                get => memory;
                internal set
                {
                    if (MemoryExtensions.Overlaps(memory.Span, value.Span))
                        memory = value;
                    else
                        throw new InvalidOperationException("Memory must overlap previously set value");
                }
            }

            public void Dispose()
            {
                var bufferPool = this.bufferPool;
                if (bufferPool is null)
                    return;
                this.bufferPool = null;

                bufferPool.Return(this);
            }
        }
    }
}
