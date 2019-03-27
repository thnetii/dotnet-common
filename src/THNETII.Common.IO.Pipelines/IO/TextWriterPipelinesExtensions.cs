﻿using System;
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
            PipeReader reader, bool clearIntermediateBuffer = false,
            CancellationToken cancelToken = default)
        {
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            char[] bufferChars = null, bufferNext = null;
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
                        if (bufferChars is null || bufferChars.Length < lengthChars)
                        {
                            CharBufferPool.Return(bufferChars, clearIntermediateBuffer);
                            bufferChars = null;
                        }
                        if (bufferChars is null)
                            bufferChars = CharBufferPool.Rent(lengthChars);

                        Memory<char> memoryChars = new Memory<char>(bufferChars, 0, lengthChars);
                        memoryBytes.Span.CopyTo(MemoryMarshal.AsBytes(memoryChars.Span));

                        cancelToken.ThrowIfCancellationRequested();

                        await writeTask.ConfigureAwait(false);
                        reader.AdvanceTo(prevPosition, examined: nextPosition);
                        writeTask = writer.WriteAsync(bufferChars, 0, lengthChars);

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
                if (bufferChars is char[])
                    CharBufferPool.Return(bufferChars, clearIntermediateBuffer);
                if (bufferNext is char[])
                    CharBufferPool.Return(bufferNext, clearIntermediateBuffer);
            }
        }
    }

}
