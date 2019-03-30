using System;
using System.Runtime.InteropServices;

namespace THNETII.Common.Buffers
{
    public static class ArrayMemoryMarshal
    {
        public static ArrayDisposableSegment<T> GetMemoryArray<T>(
            Memory<T> memory, bool noCopy = false, bool noCopyBack = true)
        {
            if (memory.IsEmpty)
            {
                return new ArrayDisposableSegment<T>(Array.Empty<T>(), 0, 0);
            }
            else if (MemoryMarshal.TryGetArray((ReadOnlyMemory<T>)memory, out var segment))
            {
                return new ArrayDisposableSegment<T>(segment);
            }

            var owner = ArrayMemoryPool<T>.Shared.Rent(memory.Length);
            try
            {
                if (!noCopy)
                    memory.CopyTo(owner.Memory);
                Memory<T> copyMemory = owner.Memory.Slice(0, memory.Length);
                bool isArray = MemoryMarshal.TryGetArray((ReadOnlyMemory<T>)copyMemory, out var segment);
                if (!isArray) throw new InvalidOperationException("Memory returned from ArrayMemoryPool is not an array.");
                return new ArrayDisposableSegment<T>(segment, owner,
                    noCopyBack ? Memory<T>.Empty : memory);
            }
            catch (Exception) when (DisposeWithoutUnwind(owner)) { throw; }

            bool DisposeWithoutUnwind(IDisposable disposable)
            {
                disposable?.Dispose();
                return false; // Return false to not catch exception
            }
        }

        public static ArrayDisposableSegment<T> GetMemoryArray<T>(
            ReadOnlyMemory<T> memory, bool noCopy = false)
        {
            if (memory.IsEmpty)
            {
                return new ArrayDisposableSegment<T>(Array.Empty<T>(), 0, 0);
            }
            else if (MemoryMarshal.TryGetArray(memory, out var segment))
            {
                return new ArrayDisposableSegment<T>(segment);
            }

            var owner = ArrayMemoryPool<T>.Shared.Rent(memory.Length);
            try
            {
                if (!noCopy)
                    memory.CopyTo(owner.Memory);
                Memory<T> copyMemory = owner.Memory.Slice(0, memory.Length);
                bool isArray = MemoryMarshal.TryGetArray((ReadOnlyMemory<T>)copyMemory, out var segment);
                if (!isArray) throw new InvalidOperationException("Memory returned from ArrayMemoryPool is not an array.");
                return new ArrayDisposableSegment<T>(segment, owner);
            }
            catch (Exception) when (DisposeWithoutUnwind(owner)) { throw; }

            bool DisposeWithoutUnwind(IDisposable disposable)
            {
                disposable?.Dispose();
                return false; // Return false to not catch exception
            }
        }
    }
}
