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
            try
            {
                var readTask = reader.RentAndReadAsync(cancelToken);
                for (var buffer = await readTask.ConfigureAwait(false); buffer is IMemoryOwner<char>; buffer = await readTask.ConfigureAwait(false))
                {
                    using (buffer)
                    {
                        cancelToken.ThrowIfCancellationRequested();
                        readTask = reader.RentAndReadAsync(cancelToken);

                        writer.Write(MemoryMarshal.AsBytes(buffer.Memory.Span));
                    }

                    cancelToken.ThrowIfCancellationRequested();
                    FlushResult flushResult = await writer.FlushAsync(cancelToken)
                        .ConfigureAwait(false);
                    if (flushResult.IsCompleted)
                        break;
                }
                cancelToken.ThrowIfCancellationRequested();
                writer.Complete();
            }
            catch (Exception except) when (CompleteWriterWithoutUnwind(except))
            { throw; }

            bool CompleteWriterWithoutUnwind(Exception e)
            {
                writer.Complete(e);
                return false; // Return false to not catch exception
            }
        }
    }
}
