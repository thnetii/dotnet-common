using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using System.Threading;
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

            cancelToken.ThrowIfCancellationRequested();
            while (true)
            {
                ReadResult result = await reader.ReadAsync(cancelToken);

                foreach (var memoryBytes in result.Buffer)
                {
                    cancelToken.ThrowIfCancellationRequested();

                    int lengthChars = memoryBytes.Length / sizeof(char);
                    var bufferChars = ArrayMemoryPool<char>.Shared.Rent(lengthChars);
                    try
                    {
                        Memory<char> memoryChars = bufferChars.Memory.Slice(0, lengthChars);
                        memoryBytes.Span.CopyTo(MemoryMarshal.AsBytes(memoryChars.Span));

                        cancelToken.ThrowIfCancellationRequested();

                        await writer.WriteAndDisposeAsync(bufferChars, cancelToken)
                            .ConfigureAwait(false);
                    }
                    catch (Exception) when (DisposeBufferOnException(bufferChars)) { throw; }
                }

                reader.AdvanceTo(result.Buffer.End);

                if (result.IsCompleted)
                {
                    break;
                }
            }

            cancelToken.ThrowIfCancellationRequested();
            reader.Complete();

            bool DisposeBufferOnException(IMemoryOwner<char> buffer)
            {
                buffer?.Dispose();
                return false; // Return false to not catch exception
            }
        }
    }
}
