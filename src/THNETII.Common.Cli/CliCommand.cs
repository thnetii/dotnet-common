using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace THNETII.Common.Cli
{
    public class CliCommand
    {
        public IConfiguration Configuration { get; }

        public ILogger Logger { get; }

        public CliCommand() { }

        public CliCommand(IConfiguration configuration, ILogger<CliCommand> logger = null) : this()
        {
            Configuration = configuration;
            Logger = logger;
        }

        private int UnrecognizedCommandFallback(CommandLineApplication app, string unrecognizedCommand, int commandIdx, bool allowExecuteSingleMatch = false)
        {
            var matchCommands = app.Commands.Where(c => c.Name.StartsWith(unrecognizedCommand, StringComparison.OrdinalIgnoreCase)).ToList();
            switch (matchCommands.Count)
            {
                case 0:
                    if (unrecognizedCommand.Length > 1)
                        return UnrecognizedCommandFallback(app, unrecognizedCommand.Substring(0, unrecognizedCommand.Length - 1), commandIdx);
                    break;
                case 1:
                    if (!allowExecuteSingleMatch)
                        break;
                    Logger?.LogDebug("Matching shortened command '{0}' to command '{1}'", unrecognizedCommand, matchCommands[0].Name);
                    var commandArgs = new string[app.RemainingArguments.Count - 1];
                    for (int i = 0; i < app.RemainingArguments.Count; i++)
                    {
                        if (i < commandIdx)
                            commandArgs[i] = app.RemainingArguments[i];
                        else if (i > commandIdx)
                            commandArgs[i - 1] = app.RemainingArguments[i];
                    }
                    return matchCommands[0].Execute(commandArgs);
            }

            Logger?.LogError("Unrecognized Command: {0}", unrecognizedCommand);
            if (matchCommands.Any())
                app.Out.WriteLine("Did you mean:");
            foreach (var mc in matchCommands)
                app.Out.WriteLine("\t" + mc.Name);
            app.ShowHint();
            return 1;
        }

        public virtual int Run(CommandLineApplication app)
        {
            int i;
            string unrecognizedCommand = null;
            for (i = 0; i < app.RemainingArguments.Count; i++)
            {
                if (!app.RemainingArguments[i].StartsWith("-"))
                {
                    unrecognizedCommand = app.RemainingArguments[i];
                    break;
                }
            }

            if (unrecognizedCommand == null)
            {
                Logger?.LogError("Missing required command argument.");
                app.ShowHint();
                return 1;
            }

            return UnrecognizedCommandFallback(app, unrecognizedCommand, i, true);
        }
    }
}