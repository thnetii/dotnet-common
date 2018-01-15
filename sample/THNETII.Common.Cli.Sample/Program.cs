using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace THNETII.Common.Cli.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new CliBuilder<CliCommand>()
                .WithServiceCollection(configureServices: (IServiceCollection services) => { })
                .WithConfigurationBuilder(configurationBuilder: (IConfigurationBuilder config) => { })
                .WithHelpOption()
                .WithVersionOption()
                .WithVersionOption("-V|--version")
                .WithVersionOption(configureOption: (CliOptionBuilder option) => { })
                .WithVersionOption("-V|--version", configureOption: (CliOptionBuilder option) => { })
                .WithOption("-a|--alpha")
                ;
        }
    }
}
