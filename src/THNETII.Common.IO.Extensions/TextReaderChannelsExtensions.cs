using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace THNETII.Common.IO
{
    public static class TextReaderChannelsExtensions
    {
        public static async Task ReadIntoChannelAsync(this TextReader reader,
            ChannelWriter<IMemoryOwner<char>> writer,
            CancellationToken cancelToken = default)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));
            if (writer is null)
                throw new ArgumentNullException(nameof(writer));

            try
            {
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
