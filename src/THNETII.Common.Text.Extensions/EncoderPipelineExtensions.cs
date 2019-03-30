using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace THNETII.Common.Text
{
    public static class EncoderPipelineExtensions
    {
        public static async Task EncodeChannelToPipeline(this Encoder encoder,
            ChannelReader<IMemoryOwner<char>> reader, PipeWriter writer,
            CancellationToken cancelToken = default)
        {
            if (encoder is null)
                throw new ArgumentNullException(nameof(encoder));
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            cancelToken.ThrowIfCancellationRequested();

            try
            {
                var readTask = reader.ReadAsync(cancelToken);
                for (var buffer = await readTask.ConfigureAwait(false); buffer is IMemoryOwner<char>; buffer = await readTask.ConfigureAwait(false))
                {
                    cancelToken.ThrowIfCancellationRequested();
                    readTask = reader.ReadAsync(cancelToken);

                    Memory<char> charsMemory = buffer.Memory;
                    int bytesRequired = encoder.GetByteCount(charsMemory, flush: false);
                    Memory<byte> bytesMemory = writer.GetMemory(bytesRequired);
                    int bytesWritten = encoder.GetBytes(charsMemory, bytesMemory, flush: false);
                    writer.Advance(bytesWritten);
                    var result = await writer.FlushAsync(cancelToken)
                        .ConfigureAwait(false);
                    cancelToken.ThrowIfCancellationRequested();
                }
            }
            catch (ChannelClosedException)
            {
                cancelToken.ThrowIfCancellationRequested();

                char[] lastCharsArray = Array.Empty<char>();
                int lastBytesRequired = encoder.GetByteCount(lastCharsArray, index: 0, count: 0, flush: true);
                if (lastBytesRequired > 0)
                {
                    Memory<byte> lastBytesMemory = writer.GetMemory(lastBytesRequired);
                    int lastBytesWritten = encoder.GetBytes(lastCharsArray, lastBytesMemory, flush: true);
                    writer.Advance(lastBytesWritten);
                    await writer.FlushAsync(cancelToken).ConfigureAwait(false);
                }

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
