using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace THNETII.Common.Cli.Hosting
{
    public static class CommandLineBuilderHostExtensions
    {
        public static CommandLineBuilder UseHost(this CommandLineBuilder commandLine,
            Func<string[], IHostBuilder> createHostBuilder)
        {
            return commandLine.UseMiddleware((context, next) =>
            {
                return createHostBuilder(context.ParseResult.UnmatchedTokens.ToArray())
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton(context);
                        services.AddSingleton(context.BindingContext);
                        services.AddSingleton(context.Console);
                        services.AddSingleton(context.ParseResult);
                    })
                    .RunConsoleAsync();
            });
        }
    }
}
