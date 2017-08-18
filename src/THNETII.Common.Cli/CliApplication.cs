using CommandLine;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace THNETII.Common.Cli
{
    public class CliApplication
    {
        public const int ExitSuccess = 0;
        public const int ExitFailure = 1;

        private readonly Parser parser;

        public CliApplication AddCommand<TOptions>(Func<TOptions, IServiceProvider, int> command)
        {
            throw new NotImplementedException();
        }

        public CliApplication AddCommand<TOptions>(Func<TOptions, IServiceProvider, Task<int>> asyncCommand)
        {
            throw new NotImplementedException();
        }

        public CliApplication ConfigurationBuilder(Action<IConfigurationBuilder> configurationBuilderAction)
        {
            throw new NotImplementedException();
        }

        public CliApplication ConfigureServices(Action<IServiceCollection> configureDefaultServices)
        {
            throw new NotImplementedException();
        }

        public CliApplication AlwaysRun(Action<IServiceProvider> defaultAlwaysRun)
        {
            throw new NotImplementedException();
        }

        public int Execute(IEnumerable<string> args)
        {
            var parseResult = parser.ParseArguments(args);

            throw new NotImplementedException();
        }
    }
}
