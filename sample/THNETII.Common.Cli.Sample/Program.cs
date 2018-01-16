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
                ;
        }
    }
}
