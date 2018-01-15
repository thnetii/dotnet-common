using McMaster.Extensions.CommandLineUtils;

namespace THNETII.Common.Cli.Infrastructure
{
    public interface ICommandLineApplicationFactory
    {
        CommandLineApplication CreateCommandLineApplication();
    }
}
