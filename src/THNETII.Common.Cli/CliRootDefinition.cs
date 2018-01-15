using McMaster.Extensions.CommandLineUtils;
using THNETII.Common.Cli.Infrastructure;

namespace THNETII.Common.Cli
{
    public class CliRootDefinition : ICommandLineApplicationFactory
    {
        public CliRootDefinition()
        {
        }

        CommandLineApplication ICommandLineApplicationFactory.CreateCommandLineApplication()
        {
            throw new System.NotImplementedException();
        }
    }
}
