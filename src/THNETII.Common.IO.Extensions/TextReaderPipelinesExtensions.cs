using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace THNETII.Common.IO
{
    using static CharBufferPipelineExtensions;

    public static class TextReaderPipelinesExtensions
    {
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static async Task ReadIntoPipelineAsync(this TextReader reader,
            PipeWriter writer, int bufferSize = MinimumBufferSize,
            CancellationToken cancelToken = default)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            int minimumLength = (int)Math.Min(MinimumBufferSize, (uint)bufferSize);
            IMemoryOwner<char> bufferCurrent = CharBufferPool.Rent(minimumLength);
            IMemoryOwner<char> bufferNext = CharBufferPool.Rent(minimumLength);
            using (bufferCurrent)
            using (bufferNext)
            {
                cancelToken.ThrowIfCancellationRequested();
                bool first = true;
                int charsRead;
                var readTask = reader.ReadBlockAsync(bufferCurrent.Memory, cancelToken);
                ValueTask<FlushResult> writeTask = default;
                for (charsRead = await readTask.ConfigureAwait(false); charsRead != 0; charsRead = await readTask.ConfigureAwait(false))
                {
                    readTask = reader.ReadBlockAsync(bufferNext.Memory, cancelToken);

                    Memory<char> memoryCurrent = bufferCurrent.Memory.Slice(0, charsRead);
                    writer.Write(MemoryMarshal.AsBytes(memoryCurrent.Span));

                    cancelToken.ThrowIfCancellationRequested();
                    if (first)
                        first = false;
                    else
                    {
                        FlushResult flushResult = await writeTask.ConfigureAwait(false);
                        if (flushResult.IsCompleted)
                            break;
                    }
                    writeTask = writer.FlushAsync(cancelToken);

                    // Swap buffer references
#pragma warning disable CS0728 // Possibly incorrect assignment to local which is the argument to a using or lock statement
                    (bufferCurrent, bufferNext) = (bufferNext, bufferCurrent);
#pragma warning restore CS0728 // Possibly incorrect assignment to local which is the argument to a using or lock statement
                }
                cancelToken.ThrowIfCancellationRequested();
                if (!first)
                    await writeTask.ConfigureAwait(false);
                writer.Complete();
            }
        }

        public static async Task ReadIntoChannelAsync(this TextReader reader,
            ChannelWriter<IMemoryOwner<char>> writer,
            CancellationToken cancelToken = default)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            throw new NotImplementedException();

            IMemoryOwner<char> bufferOwner = CharBufferPool.Rent();
            int charsRead = await reader.ReadBlockAsync(bufferOwner.Memory, cancelToken)
                .ConfigureAwait(false);

        }

#if !NETCOREAPP
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
#endif // !NETCOREAPP
    }
}
