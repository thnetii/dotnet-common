using System;
using System.Buffers;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using THNETII.Common;
using THNETII.Common.Cli;

namespace THNETII.Base64UrlEncode
{
    public static class Program
    {
        private static readonly ArrayPool<byte> byteArrayPool = ArrayPool<byte>.Shared;

        public static Task<int> Main(string[] args)
        {
            var command = new Base64UrlCommand();
            return command.InvokeAsync(args ?? Array.Empty<string>());
        }

        internal static Task<int> InvokeAsync(IConsole console,
            bool decode = false, bool ignoreGarbage = false,
            int wrap = 76, FileInfo file = null, string data = null,
            Encoding encoding = null, int bufferSize = 4096) =>
            ConsoleUtils.RunAsync(cancelToken => RunAsync(console, decode,
                ignoreGarbage, wrap, file, data, encoding, bufferSize,
                cancelToken));

        internal static Task<int> RunAsync(IConsole console, bool decode = false,
            bool ignoreGarbage = false, int wrap = 76, FileInfo file = null,
            string data = null, Encoding encoding = null, int bufferSize = 4096,
            CancellationToken cancelToken = default)
        {
            if (decode)
                return InvokeDecodeAsync(ignoreGarbage, file, data, encoding, bufferSize, cancelToken);
            else
                return InvokeEncodeAsync(console, wrap, file, data, encoding, bufferSize, cancelToken);
        }

        private static async Task<int> InvokeEncodeAsync(IConsole console,
            int wrap, FileInfo file, string data, Encoding encoding, int bufferSize,
            CancellationToken cancelToken = default)
        {
            return ProcessExitCode.ExitSuccess;
        }

        private static async Task<int> InvokeDecodeAsync(bool ignoreGarbage,
            FileInfo file, string data, Encoding encoding, int bufferSize,
            CancellationToken cancelToken = default)
        {
            if (data is string)
            {
                using (var stringReader = new StringReader(data))
                {
                    return await RunDecodeAsync(stringReader, ignoreGarbage,
                        bufferSize, cancelToken
                        ).ConfigureAwait(false);
                }
            }
            else if (file is FileInfo)
            {
                using (var fileStream = file.OpenRead())
                using (var fileReader = new StreamReader(fileStream, encoding))
                {
                    return await RunDecodeAsync(fileReader, ignoreGarbage,
                        bufferSize, cancelToken
                        ).ConfigureAwait(false);
                }
            }
            else
            {
                var inStream = Console.OpenStandardInput(bufferSize);
                using (var inReader = new StreamReader(inStream, encoding))
                {
                    return await RunDecodeAsync(inReader, ignoreGarbage,
                        bufferSize, cancelToken
                        ).ConfigureAwait(false);
                }
            }
        }

        private static async Task<int> RunDecodeAsync(TextReader textReader,
            bool ignoreGarbage, int bufferSize, CancellationToken cancelToken = default)
        {
            Stream outStream = Console.OpenStandardOutput(bufferSize);

            char[] charsCurrent = new char[bufferSize];
            char[] charsNext = new char[bufferSize];

            byte[] bytes = byteArrayPool.Rent(bufferSize);
            try
            {
                bool first = true;
                ValueTask writeTask = default;
                ValueTask<int> readTask = textReader.ReadBlockAsync(charsNext, cancelToken);
                for (int charsRead = await readTask.ConfigureAwait(false); charsRead > 0;
                    charsRead = await readTask.ConfigureAwait(false), (charsCurrent, charsNext) = (charsNext, charsCurrent))
                {
                    readTask = textReader.ReadBlockAsync(charsNext, cancelToken);
                    // TODO: Decode: charsCurrent -> bytes

                    int bytesWritten = 0;
                    if (!first)
                        await writeTask.ConfigureAwait(false);
                    writeTask = outStream.WriteAsync(new ReadOnlyMemory<byte>(bytes, start: 0, length: bytesWritten), cancelToken);
                }

                await outStream.FlushAsync(cancelToken).ConfigureAwait(false);
                return ProcessExitCode.ExitSuccess;
            }
            finally
            {
                byteArrayPool.Return(bytes);
            }
        }
    }
}
