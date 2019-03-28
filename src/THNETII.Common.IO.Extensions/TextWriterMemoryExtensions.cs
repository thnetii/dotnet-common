#if !NETCOREAPP
using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.IO
{
    public static class TextWriterMemoryExtensions
    {
        public static Task WriteAsync(this TextWriter writer,
            ReadOnlyMemory<char> buffer, CancellationToken cancelToken = default)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            cancelToken.ThrowIfCancellationRequested();
            bool bufferIsArray = MemoryMarshal.TryGetArray(buffer, out ArraySegment<char> segment);
            if (bufferIsArray)
            {
                return writer.WriteAsync(segment.Array, segment.Offset, segment.Count);
            }
            else
            {
                return writer.WriteThroughIntermediateBufferAsync(buffer, cancelToken);
            }
        }

        private static async Task WriteThroughIntermediateBufferAsync(
            this TextWriter writer, ReadOnlyMemory<char> buffer,
            CancellationToken cancelToken = default)
        {
            ArrayPool<char> arrayPool = ArrayPool<char>.Shared;
            char[] array = arrayPool.Rent(buffer.Length);
            try
            {
                cancelToken.ThrowIfCancellationRequested();

                buffer.CopyTo(array);

                cancelToken.ThrowIfCancellationRequested();
                await writer.WriteAsync(array, 0, buffer.Length)
                    .ConfigureAwait(false);
            }
            finally { arrayPool.Return(array); }
        }
    }
}
#endif // !NETCOREAPP
