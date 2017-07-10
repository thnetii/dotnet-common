using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace THNETII.Common.Cli
{
    public interface ICliApplication<out TCommand> where TCommand : CliCommand
    {
        ICliApplication<TCommand> AddHelpOption(Func<CommandLineApplication, CommandOption> helpBuilder, bool inherited);
        ICliApplication<TCommand> AddVersionOption(Func<CommandLineApplication, CommandOption> versionBuilder, bool inherited);
        ICliApplication<TCommand> AddOption(Func<CommandLineApplication, CommandOption> optionBuilder, Action<CommandOption, IDictionary<string, string>> optionReadAction);
        ICliApplication<TCommand> AddArgument(Func<CommandLineApplication, CommandArgument> argumentBuilder, Action<CommandArgument, IDictionary<string, string>> argumentReadAction);
        ICliApplication<TCommand> AddSubCommand<TSubCommand>(Func<CommandLineApplication, CommandLineApplication> commandBuilder, Action<ICliApplication<TSubCommand>> subCliConfigure) where TSubCommand : CliCommand;
    }

    public static class CliApplication
    {
        public const string DefaultHelpTemplate = @"-?|-h|--help";
        public const string DefaultVersionTemplate = @"--version";

        private static string GetShortVersionString<TCommand>() where TCommand: CliCommand
        {
            Assembly assembly = typeof(TCommand).GetTypeInfo().Assembly;
            AssemblyName assemblyName = assembly.GetName();
            string informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (string.IsNullOrWhiteSpace(informationalVersion))
                return 'v' + assemblyName.Version.ToString();
            return 'v' + informationalVersion;
        }

        private static string GetLongVersionString<TCommand>() where TCommand : CliCommand
        {
            Assembly CommandAssembly = typeof(TCommand).GetTypeInfo().Assembly;
            AssemblyName CommandAssemblyName = CommandAssembly.GetName();
            string informationalVersion = CommandAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (string.IsNullOrWhiteSpace(informationalVersion))
                return 'v' + CommandAssemblyName.Version.ToString();
            return $"v{informationalVersion} (v{CommandAssemblyName.Version})";
        }

        public static ICliApplication<TCommand> AddHelpOption<TCommand>(this ICliApplication<TCommand> app, string template = DefaultHelpTemplate, bool inherited = true) where TCommand : CliCommand
            => app.AddHelpOption(cli => cli.HelpOption(template), inherited);

        public static ICliApplication<TCommand> AddVersionOption<TCommand>(this ICliApplication<TCommand> app, string template = DefaultHelpTemplate, bool inherited = true) where TCommand : CliCommand
            => app.AddVersionOption(cli => cli.VersionOption(template, GetShortVersionString<TCommand>, GetLongVersionString<TCommand>), inherited);

        public static ICliApplication<TCommand> AddOption<TCommand>(this ICliApplication<TCommand> app, string template, string description, CommandOptionType optionType, Action<CommandOption, IDictionary<string, string>> optionReadAction) where TCommand : CliCommand
            => app.AddOption(cli => cli.Option(template, description, optionType), optionReadAction);
        public static ICliApplication<TCommand> AddOption<TCommand>(this ICliApplication<TCommand> app, string template, string description, CommandOptionType optionType, bool inherited, Action<CommandOption, IDictionary<string, string>> optionReadAction) where TCommand : CliCommand
            => app.AddOption(cli => cli.Option(template, description, optionType, inherited), optionReadAction);
        public static ICliApplication<TCommand> AddOption<TCommand>(this ICliApplication<TCommand> app, string template, string description, CommandOptionType optionType, Action<CommandOption> optionConfiguration, Action<CommandOption, IDictionary<string, string>> optionReadAction) where TCommand : CliCommand
            => app.AddOption(cli => cli.Option(template, description, optionType, optionConfiguration), optionReadAction);
        public static ICliApplication<TCommand> AddOption<TCommand>(this ICliApplication<TCommand> app, string template, string description, CommandOptionType optionType, Action<CommandOption> optionConfiguration, bool inherited, Action<CommandOption, IDictionary<string, string>> optionReadAction) where TCommand : CliCommand
            => app.AddOption(cli => cli.Option(template, description, optionType, optionConfiguration, inherited), optionReadAction);

        public static ICliApplication<TCommand> AddArgument<TCommand>(this ICliApplication<TCommand> app, string template, string description, Action<CommandArgument, IDictionary<string, string>> argumentReadAction) where TCommand : CliCommand
            => app.AddArgument(cli => cli.Argument(template, description), argumentReadAction);
        public static ICliApplication<TCommand> AddArgument<TCommand>(this ICliApplication<TCommand> app, string template, string description, bool multipleValues, Action<CommandArgument, IDictionary<string, string>> argumentReadAction) where TCommand : CliCommand
            => app.AddArgument(cli => cli.Argument(template, description, multipleValues), argumentReadAction);
        public static ICliApplication<TCommand> AddArgument<TCommand>(this ICliApplication<TCommand> app, string template, string description, Action<CommandArgument> argumentConfigure, Action<CommandArgument, IDictionary<string, string>> argumentReadAction) where TCommand : CliCommand
            => app.AddArgument(cli => cli.Argument(template, description), argumentReadAction);
        public static ICliApplication<TCommand> AddArgument<TCommand>(this ICliApplication<TCommand> app, string template, string description, Action<CommandArgument> argumentConfigure, bool multipleValues, Action<CommandArgument, IDictionary<string, string>> argumentReadAction) where TCommand : CliCommand
            => app.AddArgument(cli => cli.Argument(template, description), argumentReadAction);

        public static ICliApplication<TCommand> AddSubCommand<TCommand, TSubCommand>(this ICliApplication<TCommand> app, string name, Action<CommandLineApplication> configuration, Action<ICliApplication<TSubCommand>> subCliConfigure)
            where TCommand : CliCommand
            where TSubCommand : CliCommand
            => app.AddSubCommand(cli => cli.Command(name, configuration), subCliConfigure);
        public static ICliApplication<TCommand> AddSubCommand<TCommand, TSubCommand>(this ICliApplication<TCommand> app, string name, Action<CommandLineApplication> configuration, bool throwOnUnexpectedArg, Action<ICliApplication<TSubCommand>> subCliConfigure)
            where TCommand : CliCommand
            where TSubCommand : CliCommand
            => app.AddSubCommand(cli => cli.Command(name, configuration, throwOnUnexpectedArg), subCliConfigure);
    }

    public class CliApplication<TCommand> : ICliApplication<TCommand> where TCommand : CliCommand
    {
        private static readonly Assembly CommandAssembly = typeof(TCommand).GetTypeInfo().Assembly;
        private static readonly AssemblyName CommandAssemblyName = CommandAssembly.GetName();

        private static readonly string rootNameString = CommandAssemblyName.Name;
        private static readonly string rootFullNameString = CommandAssembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
        private static readonly string rootDescriptionString = CommandAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        private static CommandLineApplication CreateDefaultRootCommand()
        {
            return new CommandLineApplication(throwOnUnexpectedArg: false)
            {
                Name = rootNameString,
                Description = rootDescriptionString,
                FullName = rootFullNameString
            };
        }

        private readonly Func<CommandLineApplication> commandFactory;
        private readonly ICollection<Action<CommandLineApplication>> commandActions;
        private readonly ICollection<Action<IConfigurationBuilder>> configBuilderActions;
        private readonly ICollection<Action<IServiceCollection>> serviceCollectionActions;
        private readonly ICollection<Action<CommandOption, IDictionary<string, string>>> optionReadActions;
        private readonly ICollection<Action<CommandArgument, IDictionary<string, string>>> argumentReadActions;

        private CliApplication(Func<CommandLineApplication> rootCommandFactory, ICollection<Action<IConfigurationBuilder>> configBuilderActions, ICollection<Action<IServiceCollection>> serviceCollectionActions)
        {
            commandFactory = rootCommandFactory ?? CreateDefaultRootCommand;
            commandActions = new List<Action<CommandLineApplication>>();
            this.configBuilderActions = configBuilderActions ?? new List<Action<IConfigurationBuilder>>();
            this.serviceCollectionActions = serviceCollectionActions ?? new List<Action<IServiceCollection>>();
        }

        public CliApplication(Func<CommandLineApplication> rootCommandFactory = null) : this(rootCommandFactory, null, null) { }

        public CliApplication<TCommand> Configuration(Action<IConfigurationBuilder> configureAction)
        {
            if (configureAction != null)
                configBuilderActions.Add(configureAction);
            return this;
        }

        public CliApplication<TCommand> ConfigureServices(Action<IServiceCollection> configureServices)
        {
            if (configureServices != null)
                serviceCollectionActions.Add(configureServices);
            return this;
        }

        ICliApplication<TCommand> ICliApplication<TCommand>.AddHelpOption(Func<CommandLineApplication, CommandOption> helpBuilder, bool inherited)
            => throw new NotImplementedException();

        ICliApplication<TCommand> ICliApplication<TCommand>.AddVersionOption(Func<CommandLineApplication, CommandOption> versionBuilder, bool inherited)
            => throw new NotImplementedException();

        ICliApplication<TCommand> ICliApplication<TCommand>.AddOption(Func<CommandLineApplication, CommandOption> optionBuilder, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => throw new NotImplementedException();

        ICliApplication<TCommand> ICliApplication<TCommand>.AddArgument(Func<CommandLineApplication, CommandArgument> optionBuilder, Action<CommandArgument, IDictionary<string, string>> argumentReadAction)
            => throw new NotImplementedException();

        ICliApplication<TCommand> ICliApplication<TCommand>.AddSubCommand<TSubCommand>(Func<CommandLineApplication, CommandLineApplication> commandBuilder, Action<ICliApplication<TSubCommand>> subCliConfigure)
            => throw new NotImplementedException();

        public event EventHandler<IServiceProvider> Executing;

        protected virtual int ExecuteCommand()
        {
            throw new NotImplementedException();
        }

        public virtual int Execute(string[] args)
        {
            throw new NotImplementedException();
        }

        private class SubCliApplication<TSubCommand> : CliApplication<TSubCommand> where TSubCommand : CliCommand
        {
            public CliApplication<TCommand> Parent { get; }

            public SubCliApplication(CliApplication<TCommand> parent)
                : base(null, parent.configBuilderActions, parent.serviceCollectionActions)
            {
                Parent = parent;
            }

            public override int Execute(string[] args)
            {
                return base.Execute(args);
            }
        }
    }
}
