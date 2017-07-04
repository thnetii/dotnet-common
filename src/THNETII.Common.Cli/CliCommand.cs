using Microsoft.Extensions.CommandLineUtils;

namespace THNETII.Common.Cli
{
    public abstract class CliCommand
    {
        public abstract int Run(CommandLineApplication app);
    }
}