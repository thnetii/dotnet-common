using System;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.Cli.Sample
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ConsoleUtils.RunAsync(args, (Func<string[], CancellationToken, Task>)MainAsync)
                .GetAwaiter().GetResult();
        }

        public static async Task MainAsync(string[] args, CancellationToken cancelToken = default)
        {
            await Task.Yield();
            var line = Console.ReadLine();
            cancelToken.ThrowIfCancellationRequested();
            Console.WriteLine("Read line: " + line ?? string.Empty);
        }
    }
}
