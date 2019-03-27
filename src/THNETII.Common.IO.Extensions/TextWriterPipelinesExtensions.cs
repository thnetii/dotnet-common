using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.IO
{
    using static CharBufferPipelineExtensions;

    public static class TextWriterPipelinesExtensions
    {
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static async Task WriteFromPipelineAsync(this TextWriter writer,
            PipeReader reader, CancellationToken cancelToken = default)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            IMemoryOwner<char> bufferChars = null, bufferNext = null;
            try
            {
                cancelToken.ThrowIfCancellationRequested();
                while (true)
                {
                    ReadResult result = await reader.ReadAsync(cancelToken);

                    Task writeTask = Task.CompletedTask;
                    SequencePosition prevPosition = result.Buffer.Start;
                    foreach (var memoryBytes in result.Buffer)
                    {
                        SequencePosition nextPosition = result.Buffer.GetPosition(memoryBytes.Length, prevPosition);
                        cancelToken.ThrowIfCancellationRequested();

                        int lengthChars = memoryBytes.Length / sizeof(char);
                        if (bufferChars is null || bufferChars.Memory.Length < lengthChars)
                        {
                            bufferChars.Dispose();
                            bufferChars = null;
                        }
                        if (bufferChars is null)
                            bufferChars = CharBufferPool.Rent(lengthChars);

                        Memory<char> memoryChars = bufferChars.Memory.Slice(0, lengthChars);
                        memoryBytes.Span.CopyTo(MemoryMarshal.AsBytes(memoryChars.Span));

                        cancelToken.ThrowIfCancellationRequested();

                        await writeTask.ConfigureAwait(false);
                        reader.AdvanceTo(prevPosition, examined: nextPosition);
                        writeTask = writer.WriteAsync(memoryChars, cancelToken);

                        prevPosition = nextPosition;
                        (bufferChars, bufferNext) = (bufferNext, bufferChars);
                    }

                    if (result.IsCompleted)
                    {
                        break;
                    }
                }

                cancelToken.ThrowIfCancellationRequested();
                reader.Complete();
            }
            finally
            {
                bufferChars?.Dispose();
                bufferNext?.Dispose();
            }
        }

#if !NETCOREAPP
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
#endif // !NETCOREAPP
    }
}
