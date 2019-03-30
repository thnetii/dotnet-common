using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace THNETII.Common.IO
{
    public static class TextWriterChannelsExtensions
    {
        public static async Task WriteFromChannelAsync(this TextWriter writer,
            ChannelReader<IMemoryOwner<char>> reader,
            CancellationToken cancelToken = default)
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
    }
}
