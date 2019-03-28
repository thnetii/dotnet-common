using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using THNETII.Common.Buffers;

namespace THNETII.Common.IO
{
    public static class TextReaderPipelinesExtensions
    {
        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        public static async Task ReadIntoPipelineAsync(this TextReader reader,
            PipeWriter writer, CancellationToken cancelToken = default)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            cancelToken.ThrowIfCancellationRequested();
            bool first = true;
            var readTask = reader.RentAndReadAsync(cancelToken);
            ValueTask<FlushResult> writeTask = default;
            for (var buffer = await readTask.ConfigureAwait(false); buffer is IMemoryOwner<char>; buffer = await readTask.ConfigureAwait(false))
            {
                using (buffer)
                {
                    cancelToken.ThrowIfCancellationRequested();
                    readTask = reader.RentAndReadAsync(cancelToken);

                    writer.Write(MemoryMarshal.AsBytes(buffer.Memory.Span));
                }

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
            }
            cancelToken.ThrowIfCancellationRequested();
            if (!first)
                await writeTask.ConfigureAwait(false);
            writer.Complete();
        }

        public static async Task ReadIntoChannelAsync(this TextReader reader,
            ChannelWriter<IMemoryOwner<char>> writer,
            CancellationToken cancelToken = default)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            Task<IMemoryOwner<char>> readTask = reader.RentAndReadAsync(cancelToken);
            for (var buffer = await readTask.ConfigureAwait(false); buffer is IMemoryOwner<char>; buffer = await readTask.ConfigureAwait(false))
            {
                cancelToken.ThrowIfCancellationRequested();
                readTask = reader.RentAndReadAsync(cancelToken);

                await writer.WriteAsync(buffer, cancelToken)
                    .ConfigureAwait(false);
            }

            writer.Complete();
        }

        private static async Task<IMemoryOwner<char>> RentAndReadAsync(
            this TextReader reader, CancellationToken cancelToken = default)
        {
            var bufferOwner = ArrayMemoryPool<char>.Shared.Rent();
            int charsRead = await reader.ReadAsync(bufferOwner.Memory, cancelToken)
                .ConfigureAwait(false);
            cancelToken.ThrowIfCancellationRequested();
            if (charsRead == 0)
            {
                bufferOwner.Dispose();
                return null;
            }
            else if (charsRead == bufferOwner.Memory.Length)
                return bufferOwner;
            return bufferOwner.Slice(0, charsRead);
        }
    }
}
