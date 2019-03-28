#if !NETCOREAPP
using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.IO
{
    public static class TextReaderMemoryExtensions
    {
        public static Task<int> ReadAsync(this TextReader reader,
            Memory<char> buffer, CancellationToken cancelToken = default)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            cancelToken.ThrowIfCancellationRequested();
            bool bufferIsArray = MemoryMarshal.TryGetArray(buffer, out ArraySegment<char> segment);
            if (bufferIsArray)
            {
                return reader.ReadAsync(segment.Array, segment.Offset, segment.Count);
            }
            else
            {
                return reader.ReadThroughIntermediateBufferAsync(buffer, cancelToken);
            }
        }

        private static async Task<int> ReadThroughIntermediateBufferAsync(this TextReader reader,
            Memory<char> buffer, CancellationToken cancelToken = default)
        {
            ArrayPool<char> arrayPool = ArrayPool<char>.Shared;
            char[] array = arrayPool.Rent(buffer.Length);
            try
            {
                cancelToken.ThrowIfCancellationRequested();
                var charsRead = await reader.ReadAsync(array, 0, buffer.Length)
                    .ConfigureAwait(false);
                cancelToken.ThrowIfCancellationRequested();
                var readMemory = new Memory<char>(array, 0, charsRead);
                readMemory.CopyTo(buffer);
                return charsRead;
            }
            finally { arrayPool.Return(array); }
        }

        public static Task<int> ReadBlockAsync(this TextReader reader,
            Memory<char> buffer, CancellationToken cancelToken = default)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            cancelToken.ThrowIfCancellationRequested();
            bool bufferIsArray = MemoryMarshal.TryGetArray(buffer, out ArraySegment<char> segment);
            if (bufferIsArray)
            {
                return reader.ReadBlockAsync(segment.Array, segment.Offset, segment.Count);
            }
            else
            {
                return reader.ReadBlockThroughIntermediateBufferAsync(buffer, cancelToken);
            }
        }

        private static async Task<int> ReadBlockThroughIntermediateBufferAsync(
            this TextReader reader, Memory<char> buffer,
            CancellationToken cancelToken = default)
        {
            ArrayPool<char> arrayPool = ArrayPool<char>.Shared;
            char[] array = arrayPool.Rent(buffer.Length);
            try
            {
                cancelToken.ThrowIfCancellationRequested();

                var charsRead = await reader.ReadBlockAsync(array, 0, buffer.Length)
                    .ConfigureAwait(false);
                cancelToken.ThrowIfCancellationRequested();
                var readMemory = new Memory<char>(array, 0, charsRead);
                readMemory.CopyTo(buffer);
                return charsRead;
            }
            finally { arrayPool.Return(array); }
        }
    }
}
#endif // !NETCOREAPP
