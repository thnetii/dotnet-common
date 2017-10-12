using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace THNETII.Common.Cli
{
    public abstract class CliAsyncCommand : CliCommand
    {
        public CliAsyncCommand() : base() { }
        public CliAsyncCommand(IConfiguration configuration, ILogger<CliAsyncCommand> logger = null) : base(configuration, logger) { }

        public virtual Task<int> RunAsync(CommandLineApplication app)
            => Task.FromResult(base.Run(app));

        /// <inheritdoc />
        public override sealed int Run(CommandLineApplication app)
            => RunAsync(app)?.GetAwaiter().GetResult() ?? 1;
    }
}
