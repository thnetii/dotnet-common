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

            Task writeTask = Task.CompletedTask;
            cancelToken.ThrowIfCancellationRequested();
            while (true)
            {
                ReadResult result = await reader.ReadAsync(cancelToken);

                SequencePosition prevPosition = result.Buffer.Start;
                foreach (var memoryBytes in result.Buffer)
                {
                    SequencePosition nextPosition = result.Buffer.GetPosition(memoryBytes.Length, prevPosition);
                    cancelToken.ThrowIfCancellationRequested();

                    int lengthChars = memoryBytes.Length / sizeof(char);
                    var bufferChars = ArrayMemoryPool<char>.Shared.Rent(lengthChars);
                    try
                    {
                        Memory<char> memoryChars = bufferChars.Memory.Slice(0, lengthChars);
                        memoryBytes.Span.CopyTo(MemoryMarshal.AsBytes(memoryChars.Span));

                        cancelToken.ThrowIfCancellationRequested();

                        await writeTask.ConfigureAwait(false);
                        reader.AdvanceTo(prevPosition, examined: nextPosition);
                    }
                    catch (Exception) when (DisposeBufferOnException(bufferChars)) { throw; }
                    writeTask = writer.WriteAndDisposeAsync(bufferChars, cancelToken);

                    prevPosition = nextPosition;
                }

                if (result.IsCompleted)
                {
                    break;
                }
            }

            cancelToken.ThrowIfCancellationRequested();
            await writeTask.ConfigureAwait(false);
            reader.Complete();

            bool DisposeBufferOnException(IMemoryOwner<char> buffer)
            {
                buffer?.Dispose();
                return false; // Return false to not catch exception
            }
        }

        public static async Task WriteFromChannelAsync(this TextWriter writer,
            ChannelReader<IMemoryOwner<char>> reader, CancellationToken cancelToken = default)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            var writeTask = Task.CompletedTask;
            while (true)
            {
                IMemoryOwner<char> buffer;
                try { buffer = await reader.ReadAsync(cancelToken).ConfigureAwait(false); }
                catch (ChannelClosedException) { break; }
                await writeTask.ConfigureAwait(false);
                writeTask = writer.WriteAndDisposeAsync(buffer, cancelToken);
            }
            await writeTask.ConfigureAwait(false);
        }

        private static async Task WriteAndDisposeAsync(this TextWriter writer,
            IMemoryOwner<char> buffer, CancellationToken cancelToken = default)
        {
            using (buffer)
            {
                await writer.WriteAsync(buffer.Memory, cancelToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
