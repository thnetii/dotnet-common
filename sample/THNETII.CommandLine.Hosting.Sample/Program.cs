﻿using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace THNETII.CommandLine.Hosting.Sample
{
    public static class Program
    {
        public static Task Main(string[] args) => new CommandLineBuilder(
            new RootCommand
            {
                Handler = CommandHandler.Create<IHost, string>(Run),
                TreatUnmatchedTokensAsErrors = false
            })
            .AddOption(new Option("--foo", argument: new Argument<string>()))
            .UseDefaults()
            .UseHost(Host.CreateDefaultBuilder)
            .Build().InvokeAsync(args);

        internal static void Run(IHost host, string foo)
        {
            var logger = host.Services.GetService<ILoggerFactory>()?.CreateLogger(typeof(Program))
                ?? Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;

            logger.LogInformation($"Command-line argument {nameof(foo)} supplied: {{{nameof(foo)}}}", foo);
        }
    }
}
