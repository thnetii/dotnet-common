using McMaster.Extensions.CommandLineUtils;

namespace THNETII.Common.Cli.Infrastructure
{
    public interface ICommandLineApplicationConfigure
    {
        void ConfigureApplication(CommandLineApplication app);
    }
}