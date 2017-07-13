using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace THNETII.Common.Cli
{
    public abstract class CliAsyncCommand : CliCommand
    {
        public CliAsyncCommand() : base() { }
        public CliAsyncCommand(IConfiguration configuration, ILogger<CliAsyncCommand> logger = null) : base(configuration, logger) { }

        public virtual Task<int> RunAsync(CommandLineApplication app)
            => Task.FromResult(base.Run(app));

        public override sealed int Run(CommandLineApplication app)
            => RunAsync(app)?.GetAwaiter().GetResult() ?? 1;
    }
}
