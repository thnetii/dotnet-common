using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using THNETII.Common.Buffers;

namespace THNETII.Common.Text
{
    public static class DecoderPipelineExtensions
    {
        public static async Task DecodePipelineIntoChannel(this Decoder decoder,
            PipeReader reader, ChannelWriter<IMemoryOwner<char>> writer,
            CancellationToken cancelToken = default)
        {
            if (decoder is null)
                throw new ArgumentNullException(nameof(decoder));
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            cancelToken.ThrowIfCancellationRequested();

            try
            {
                while (true)
                {
                    var result = await reader.ReadAsync(cancelToken)
                        .ConfigureAwait(false);

                    foreach (var bytesMemory in result.Buffer)
                    {
                        cancelToken.ThrowIfCancellationRequested();

                        int charsRequired = decoder.GetCharCount(bytesMemory, flush: false);
                        var charsOwner = ArrayMemoryPool<char>.Shared.Rent(charsRequired);
                        try
                        {
                            int charsWritten = decoder.GetChars(bytesMemory, charsOwner.Memory, flush: false);
                            await writer.WriteAsync(charsOwner.Slice(0, charsWritten), cancelToken);
                        }
                        catch (Exception) when (DisposeBufferOnException(charsOwner)) { throw; }
                    }

                    reader.AdvanceTo(result.Buffer.End);

                    if (result.IsCompleted)
                    {
                        ReadOnlyMemory<byte> bytesMemory = ReadOnlyMemory<byte>.Empty;
                        int charsRequired = decoder.GetCharCount(bytesMemory, flush: true);
                        if (charsRequired > 0)
                        {
                            var charsOwner = ArrayMemoryPool<char>.Shared.Rent(charsRequired);
                            try
                            {
                                int charsWritten = decoder.GetChars(bytesMemory, charsOwner.Memory, flush: true);
                                await writer.WriteAsync(charsOwner.Slice(0, charsWritten), cancelToken);
                            }
                            catch (Exception) when (DisposeBufferOnException(charsOwner)) { throw; }
                        }
                        break;
                    }
                }

                cancelToken.ThrowIfCancellationRequested();
                reader.Complete();
            }
            catch (Exception except) when (CompleteWriterWithoutUnwind(except))
            { throw; }

            bool DisposeBufferOnException(IDisposable disposable)
            {
                disposable?.Dispose();
                return false; // Return false to not catch exception
            }
            bool CompleteWriterWithoutUnwind(Exception e)
            {
                writer.Complete(e);
                return false; // Return false to not catch exception
            }
        }
    }
}
