using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using THNETII.Common.Cli.Infrastructure;

namespace THNETII.Common.Cli
{
    public class CliRootBuilder<TCommand> : CliBuilder<TCommand> where TCommand : CliCommand
    {
        private Action<CliRootDefinition> configureRoot;

        public CliRootBuilder<TCommand> WithRootCommandDetails(Action<CliRootDefinition> configureRoot)
        {
            this.configureRoot += configureRoot;
            return this;
        }

        public int Execute(params string[] args) => DoExecute(args, (cmd, app) => cmd.Run(app));

        public Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken = default) => DoExecute(args, (cmd, app) =>
        {
            if (cmd is CliAsyncCommand asyncCmd)
                return asyncCmd.RunAsync(app, cancellationToken);
            return Task.Run(() => cmd.Run(app), cancellationToken);
        });

        private TResult DoExecute<TResult>(string[] args, Func<TCommand, CommandLineApplication, TResult> commandExecute)
        {
            var rootDefinition = new CliRootDefinition();
            configureRoot?.Invoke(rootDefinition);
            ICommandLineApplicationFactory rootFactory = rootDefinition;
            CommandLineApplication app = rootFactory.CreateCommandLineApplication();
            return DoExecute(app, args, commandExecute);
        }
    }
}
