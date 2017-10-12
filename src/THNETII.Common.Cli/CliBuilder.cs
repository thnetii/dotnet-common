using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;


namespace THNETII.Common.Cli
{
    using CommandExecuteAction = Action<IConfigurationBuilder, IServiceCollection, IDictionary<string, string>>;
    using CommandBuildAction = Action<CommandLineApplication, ICollection<Action<IConfigurationBuilder, IServiceCollection, IDictionary<string, string>>>>;

    public class CliBuilder<TCommand> where TCommand : CliCommand
    {
        public const string DefaultHelpTemplate = @"-?|-h|--help";
        public const string DefaultVersionTemplate = @"--version";

        public Assembly ExecutingAssembly { get; }

        private readonly string rootNameString;
        private readonly string rootFullNameString;
        private readonly string rootDescriptionString;
        private readonly string shortVersionString;
        private readonly string longVersionString;

        private CommandLineApplication CreateDefaultRootCommand()
        {
            return new CommandLineApplication(throwOnUnexpectedArg: false)
            {
                Name = rootNameString,
                Description = rootDescriptionString,
                FullName = rootFullNameString
            };
        }

        public CliBuilder() : this(typeof(TCommand)) { }

        public CliBuilder(Type programType) : this(programType.ThrowIfNull(nameof(programType)).GetTypeInfo().Assembly) { }

        public CliBuilder(Assembly executingAssembly)
        {
            executingAssembly.ThrowIfNull(nameof(executingAssembly));
            var assemblyName = executingAssembly.GetName();
            rootNameString = assemblyName.Name;
            rootFullNameString = executingAssembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
            rootDescriptionString = executingAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;
            string assemblyNameVersion = assemblyName.Version.ToString();
            string informationalVersion = executingAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (string.IsNullOrWhiteSpace(informationalVersion))
            {
                shortVersionString = 'v' + assemblyNameVersion;
                longVersionString = 'v' + assemblyNameVersion;
            }
            else
            {
                shortVersionString = 'v' + informationalVersion;
                longVersionString = $"v{informationalVersion} (v{assemblyNameVersion})";
            }

            ExecutingAssembly = executingAssembly;
        }

        private readonly ICollection<CommandBuildAction> commandActions = new List<CommandBuildAction>();
        private readonly ICollection<Action<CommandLineApplication, IServiceProvider>> preRunActions = new List<Action<CommandLineApplication, IServiceProvider>>();
        private bool inheritedHelpOption = false;
        private bool inheritedVersionOption = false;

        public CliBuilder<TCommand> Configuration(Action<IConfigurationBuilder> configureAction)
        {
            commandActions.Add((_, execActions) => execActions.Add((configBuilder, _1, _2) => configureAction?.Invoke(configBuilder)));
            return this;
        }

        public CliBuilder<TCommand> ConfigureServices(Action<IServiceCollection> configureServices)
        {
            commandActions.Add((_, execActions) => execActions.Add((_0, services, _2) => configureServices?.Invoke(services)));
            return this;
        }

        public CliBuilder<TCommand> PreRunCommand(Action<CommandLineApplication, IServiceProvider> preRunAction)
        {
            preRunActions.Add(preRunAction);
            return this;
        }

        public CliBuilder<TCommand> AddHelpOption(string template = DefaultHelpTemplate, bool inherited = true)
            => AddHelpOption(cli => cli.HelpOption(template), inherited);

        protected CliBuilder<TCommand> AddHelpOption(Func<CommandLineApplication, CommandOption> helpBuilder, bool inherited)
        {
            if (helpBuilder != null)
            {
                commandActions.Add((cmd, _) => helpBuilder(cmd));
                inheritedHelpOption = inherited;
            }
            return this;
        }

        public CliBuilder<TCommand> AddVersionOption(string template = DefaultVersionTemplate, bool inherited = false)
            => AddVersionOption(cli => cli.VersionOption(template, shortVersionString, longVersionString), inherited);

        protected CliBuilder<TCommand> AddVersionOption(Func<CommandLineApplication, CommandOption> versionBuilder, bool inherited)
        {
            if (versionBuilder != null)
            {
                commandActions.Add((cmd, _) => versionBuilder(cmd));
                inheritedVersionOption = inherited;
            }
            return this;
        }

        public CliBuilder<TCommand> AddOption(string template, string description, CommandOptionType optionType, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => AddOption(cli => cli.Option(template, description, optionType), optionReadAction);
        public CliBuilder<TCommand> AddOption(string template, string description, CommandOptionType optionType, bool inherited, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => AddOption(cli => cli.Option(template, description, optionType, inherited), optionReadAction);
        public CliBuilder<TCommand> AddOption(string template, string description, CommandOptionType optionType, Action<CommandOption> optionConfiguration, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => AddOption(cli => cli.Option(template, description, optionType, optionConfiguration), optionReadAction);
        public CliBuilder<TCommand> AddOption(string template, string description, CommandOptionType optionType, Action<CommandOption> optionConfiguration, bool inherited, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => AddOption(cli => cli.Option(template, description, optionType, optionConfiguration, inherited), optionReadAction);

        protected CliBuilder<TCommand> AddOption(Func<CommandLineApplication, CommandOption> optionBuilder, Action<CommandOption, IDictionary<string, string>> optionReadAction)
        {
            optionBuilder.ThrowIfNull(nameof(optionBuilder));
            commandActions.Add((cmd, execActions) =>
            {
                var option = optionBuilder(cmd);
                execActions.Add((_0, _1, configDict) =>
                {
                    if (option.OptionType == CommandOptionType.NoValue || option.HasValue())
                        optionReadAction?.Invoke(option, configDict);
                });
            });
            return this;
        }

        public CliBuilder<TCommand> AddArgument(string template, string description, Action<CommandArgument, IDictionary<string, string>> argumentReadAction)
            => AddArgument(cli => cli.Argument(template, description), argumentReadAction);
        public CliBuilder<TCommand> AddArgument(string template, string description, bool multipleValues, Action<CommandArgument, IDictionary<string, string>> argumentReadAction)
            => AddArgument(cli => cli.Argument(template, description, multipleValues), argumentReadAction);
        public CliBuilder<TCommand> AddArgument(string template, string description, Action<CommandArgument> argumentConfigure, Action<CommandArgument, IDictionary<string, string>> argumentReadAction)
            => AddArgument(cli => cli.Argument(template, description), argumentReadAction);
        public CliBuilder<TCommand> AddArgument(string template, string description, Action<CommandArgument> argumentConfigure, bool multipleValues, Action<CommandArgument, IDictionary<string, string>> argumentReadAction)
            => AddArgument(cli => cli.Argument(template, description), argumentReadAction);

        protected CliBuilder<TCommand> AddArgument(Func<CommandLineApplication, CommandArgument> argumentBuilder, Action<CommandArgument, IDictionary<string, string>> argumentReadAction)
        {
            argumentBuilder.ThrowIfNull(nameof(argumentBuilder));
            commandActions.Add((cmd, execActions) =>
            {
                var argument = argumentBuilder(cmd);
                execActions.Add((_0, _1, configDict) => argumentReadAction?.Invoke(argument, configDict));
            });
            return this;
        }

        public CliBuilder<TCommand> AddSubCommand<TSubCommand>(string name, Action<CliBuilder<TSubCommand>> subCliConfigure, Action<CommandLineApplication> configuration)
            where TSubCommand : CliCommand
            => AddSubCommand(cli => cli.Command(name, configuration), subCliConfigure);
        public CliBuilder<TCommand> AddSubCommand<TSubCommand>(string name, Action<CliBuilder<TSubCommand>> subCliConfigure, Action<CommandLineApplication> configuration, bool throwOnUnexpectedArg)
            where TSubCommand : CliCommand
            => AddSubCommand(cli => cli.Command(name, configuration, throwOnUnexpectedArg), subCliConfigure);

        protected CliBuilder<TCommand> AddSubCommand<TSubCommand>(Func<CommandLineApplication, CommandLineApplication> commandBuilder, Action<CliBuilder<TSubCommand>> subCliConfigure)
            where TSubCommand : CliCommand
        {
            commandBuilder.ThrowIfNull(nameof(commandBuilder));

            var subCliApp = new SubCliApplication<TSubCommand>(this);
            subCliConfigure?.Invoke(subCliApp);
            commandActions.Add((cmd, execActions) =>
            {
                var subCmd = commandBuilder(cmd);
                if (inheritedHelpOption)
                    subCmd.HelpOption(cmd.OptionHelp.Template);
                if (inheritedVersionOption)
                    subCmd.VersionOption(cmd.OptionVersion.Template, cmd.ShortVersionGetter, cmd.LongVersionGetter);
                subCliApp.PrepareCommand(subCmd, execActions);
            });

            return this;
        }

        protected virtual void ExecutePreRun(CommandLineApplication command, IServiceProvider serviceProvider)
        {
            foreach (var preRunAction in preRunActions)
                preRunAction?.Invoke(command, serviceProvider);
        }

        private void PrepareCommand(CommandLineApplication command, ICollection<CommandExecuteAction> executeActions)
        {
            foreach (var buildAction in commandActions)
                buildAction(command, executeActions);

            int Execute()
            {
                var serviceCollection = new ServiceCollection();
                var configBuilder = new ConfigurationBuilder();
                var configDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var execAction in executeActions)
                    execAction?.Invoke(configBuilder, serviceCollection, configDict);
                configBuilder.AddInMemoryCollection(configDict);

                serviceCollection.AddSingleton<IConfiguration>(_ => configBuilder.Build());
                serviceCollection.AddSingleton<TCommand>();

                var serviceProvider = serviceCollection.BuildServiceProvider();
                ExecutePreRun(command, serviceProvider);
                var cmdInstance = serviceProvider.GetRequiredService<TCommand>();
                return cmdInstance.Run(command);
            }

            command.OnExecute((Func<int>)Execute);
        }

        public virtual CommandLineApplication Build()
        {
            var rootCommand = CreateDefaultRootCommand();
            var executeActions = new List<CommandExecuteAction>(commandActions.Count);
            PrepareCommand(rootCommand, executeActions);

            return rootCommand;
        }

        private class SubCliApplication<TSubCommand> : CliBuilder<TSubCommand> where TSubCommand : CliCommand
        {
            public CliBuilder<TCommand> Parent { get; }

            public SubCliApplication(CliBuilder<TCommand> parent) : base()
            {
                Parent = parent;
            }

            protected override void ExecutePreRun(CommandLineApplication command, IServiceProvider serviceProvider)
            {
                Parent.ExecutePreRun(command, serviceProvider);
                base.ExecutePreRun(command, serviceProvider);
            }
        }
    }
}
