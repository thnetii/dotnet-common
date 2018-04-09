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

                .Execute(args ?? Array.Empty<string>());
        }
    }
}
