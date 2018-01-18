using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace THNETII.Common.Cli.Sample
{
    class Program
    {
        public static int Main(string[] args)
        {
            return new CommandLineApplication()
                .WithOption("-f|--file <filename>", CommandOptionType.SingleValue, opt => opt
                    .WithDescription("The path to the file")
                    .EnableInheritance()
                    .HideFromHelpText()
                    )
                .WithOption("-l|--language <language>", CommandOptionType.SingleValue, opt => opt
                    .WithDescription("Language to use")
                    )
                .ExecuteCommand<CliCommand>(args);
        }
    }
}
