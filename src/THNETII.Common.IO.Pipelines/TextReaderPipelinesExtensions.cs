using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.IO.Pipelines
{
    public static class TextReaderPipelinesExtensions
    {
        private static readonly ArrayPool<char> charArrayPool = ArrayPool<char>.Shared;

        public const int MinimumBufferSize = 4096;

        public static async Task ReadIntoPipelineAsync(this TextReader reader,
            PipeWriter writer, int bufferSize = MinimumBufferSize,
            bool clearIntermediateBuffer = false,
            CancellationToken cancelToken = default)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            int minimumLength = (int)Math.Min(MinimumBufferSize, (uint)bufferSize);
            char[] bufferCurrent = charArrayPool.Rent(minimumLength);
            char[] bufferNext = charArrayPool.Rent(minimumLength);
            try
            {
                await ReadIntoPipelineAsync(reader, writer, bufferCurrent, bufferNext, cancelToken)
                    .ConfigureAwait(false);
            }
            finally
            {
                charArrayPool.Return(bufferCurrent, clearIntermediateBuffer);
                charArrayPool.Return(bufferNext, clearIntermediateBuffer);
            }
        }

        [SuppressMessage("Usage", "PC001: API not supported on all platforms", Justification = "https://github.com/dotnet/platform-compat/issues/123")]
        private static async Task ReadIntoPipelineAsync(TextReader reader,
            PipeWriter writer, char[] bufferCurrent, char[] bufferNext,
            CancellationToken cancelToken = default)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            cancelToken.ThrowIfCancellationRequested();
            bool first = false;
            int charsRead;
            var readTask = reader.ReadBlockAsync(bufferCurrent, cancelToken)
                .ConfigureAwait(false);
            ValueTask<FlushResult> writeTask = default;
            for (charsRead = await readTask; charsRead != 0; charsRead = await readTask)
            {
                readTask = reader.ReadBlockAsync(bufferNext, cancelToken)
                    .ConfigureAwait(false);

                MemoryMarshal.AsBytes(bufferCurrent.AsSpan())
                    .CopyTo(writer.GetSpan(charsRead * 2));

                cancelToken.ThrowIfCancellationRequested();

                if (first)
                    first = false;
                else
                {
                    FlushResult result = await writeTask.ConfigureAwait(false);
                    if (result.IsCompleted)
                    {
                        break;
                    }
                }
                writer.Advance(charsRead * 2);
                writeTask = writer.FlushAsync(cancelToken);

                // Swap buffer references
                (bufferCurrent, bufferNext) = (bufferNext, bufferCurrent);
            }
            cancelToken.ThrowIfCancellationRequested();
            if (!first)
                await writeTask.ConfigureAwait(false);
            writer.Complete();
        }

#if !NETCOREAPP
        private static Task<int> ReadBlockAsync(this TextReader reader, char[] buffer, CancellationToken cancelToken)
        {
            cancelToken.ThrowIfCancellationRequested();
            return reader.ReadBlockAsync(buffer, 0, buffer.Length);
        }
#endif // !NETCOREAPP
    }
}
