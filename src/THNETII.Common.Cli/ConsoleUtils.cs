using System;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.Cli
{
    public static class ConsoleUtils
    {
        public static ConsoleColorContext UseConsoleColor(ConsoleColor? fgColor = default, ConsoleColor? bgColor = default) =>
            new ConsoleColorContext(fgColor, bgColor);

        public static Task RunAsync(Func<CancellationToken, Task> asyncMain)
        {
            return RunAsync(Array.Empty<string>(), (asyncMain is null)
                ? default(Func<string[], CancellationToken, Task<int>>)
                : async (_, cancelToken) =>
                {
                    await asyncMain(cancelToken)
                        .ConfigureAwait(continueOnCapturedContext: true);
                    return ProcessExitCode.ExitSuccess;
                });
        }

        public static Task RunAsync(string[] args, Func<string[], CancellationToken, Task> asyncMain)
        {
            return RunAsync(args, (asyncMain is null)
                ? default(Func<string[], CancellationToken, Task<int>>)
                : async (argvs, cancelToken) =>
            {
                await asyncMain(argvs, cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: true);
                return ProcessExitCode.ExitSuccess;
            });
        }

        public static Task<int> RunAsync(Func<CancellationToken, Task<int>> asyncMain) =>
            RunAsync(Array.Empty<string>(), (asyncMain is null)
                ? default(Func<string[], CancellationToken, Task<int>>)
                : (args, cancelToken) => asyncMain(cancelToken));

        public static async Task<int> RunAsync(string[] args, Func<string[], CancellationToken, Task<int>> asyncMain)
        {
            if (asyncMain is null)
                throw new ArgumentNullException(nameof(asyncMain));

            var cts = new CancellationTokenSource();
            void OnCancelKeyPress(object senter, ConsoleCancelEventArgs e)
            {
                // If cancellation already has been requested,
                // do not cancel process termination signal.
                e.Cancel = !cts.IsCancellationRequested;

                cts.Cancel(throwOnFirstException: true);
            }
            Console.CancelKeyPress += OnCancelKeyPress;

            var cancelToken = cts.Token;
            try
            {
                var returnValue = await asyncMain(args, cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                cancelToken.ThrowIfCancellationRequested();
                return returnValue;
            }
            finally
            {
                Console.CancelKeyPress -= OnCancelKeyPress;
            }
        }
    }
}
