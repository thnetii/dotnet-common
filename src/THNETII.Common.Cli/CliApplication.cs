using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace THNETII.Common.Cli
{
    public class CliApplication<TCommand> where TCommand : CliCommand
    {
        public const string DefaultHelpTemplate = @"-?|-h|--help";
        public const string DefaultVersionTemplate = @"--version";

        private static string GetShortVersionString()
        {
            string informationalVersion = CommandAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (string.IsNullOrWhiteSpace(informationalVersion))
                return 'v' + CommandAssemblyName.Version.ToString();
            return 'v' + informationalVersion;
        }

        private static string GetLongVersionString()
        {
            string informationalVersion = CommandAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (string.IsNullOrWhiteSpace(informationalVersion))
                return 'v' + CommandAssemblyName.Version.ToString();
            return $"v{informationalVersion} (v{CommandAssemblyName.Version})";
        }

        private static readonly Assembly CommandAssembly = typeof(TCommand).GetTypeInfo().Assembly;
        private static readonly AssemblyName CommandAssemblyName = CommandAssembly.GetName();
        private static readonly string shortVersionString = GetShortVersionString();
        private static readonly string longVersionString = GetLongVersionString();
        private static readonly string rootNameString = CommandAssemblyName.Name;
        private static readonly string rootFullNameString = CommandAssembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
        private static readonly string rootDescriptionString = CommandAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

        public CommandLineApplication RootCommand { get; }

        public CliApplication(CommandLineApplication rootCommand = null)
        {
            RootCommand = rootCommand ?? new CommandLineApplication(throwOnUnexpectedArg: false)
            {
                Name = rootNameString,
                Description = rootDescriptionString,
                FullName = rootFullNameString
            };
        }

        public CliApplication<TCommand> AddHelpOption(string template = DefaultHelpTemplate) => throw new NotImplementedException();
        public CliApplication<TCommand> AddVersionOption(string template = DefaultVersionTemplate) => throw new NotImplementedException();

        public CliApplication<TCommand> AddOption(string template, string description, CommandOptionType type, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => AddOptionInternal(RootCommand.Option(template, description, type), optionReadAction);
        public CliApplication<TCommand> AddOption(string template, string description, CommandOptionType type, Action<CommandOption> optionConfigure, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => AddOptionInternal(RootCommand.Option(template, description, type, optionConfigure), optionReadAction);
        public CliApplication<TCommand> AddOption(string template, string description, CommandOptionType type, bool inherited, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => AddOptionInternal(RootCommand.Option(template, description, type, inherited), optionReadAction);
        public CliApplication<TCommand> AddOption(string template, string description, CommandOptionType type, Action<CommandOption> optionConfigure, bool inherited, Action<CommandOption, IDictionary<string, string>> optionReadAction)
            => AddOptionInternal(RootCommand.Option(template, description, type, optionConfigure, inherited), optionReadAction);

        private CliApplication<TCommand> AddOptionInternal(CommandOption option, Action<CommandOption, IDictionary<string, string>> optionReadAction)
        {
            throw new NotImplementedException();
        }

        public CliApplication<TCommand> AddArgument(string name, string description, Action<CommandOption, IDictionary<string, string>> argumentReadAction)
            => AddArgumentInternal(RootCommand.Argument(name, description), argumentReadAction);
        public CliApplication<TCommand> AddArgument(string name, string description, bool multipleValues, Action<CommandOption, IDictionary<string, string>> argumentReadAction)
            => AddArgumentInternal(RootCommand.Argument(name, description, multipleValues), argumentReadAction);
        public CliApplication<TCommand> AddArgument(string name, string description, Action<CommandArgument> argumentConfigure, Action<CommandOption, IDictionary<string, string>> argumentReadAction)
            => AddArgumentInternal(RootCommand.Argument(name, description, argumentConfigure), argumentReadAction);
        public CliApplication<TCommand> AddArgument(string name, string description, Action<CommandArgument> argumentConfigure, bool multipleValues, Action<CommandOption, IDictionary<string, string>> argumentReadAction)
            => AddArgumentInternal(RootCommand.Argument(name, description, argumentConfigure, multipleValues), argumentReadAction);

        private CliApplication<TCommand> AddArgumentInternal(CommandArgument argument, Action<CommandOption, IDictionary<string, string>> argumentReadAction)
        {
            throw new NotImplementedException();
        }

        private CliApplication<TCommand> AddSubCommandInternal<TSubCommand>(CommandLineApplication subCommand, Action<CliApplication<TSubCommand>> subCliAppConfigure)
            where TSubCommand : CliCommand
        {
            subCliAppConfigure?.Invoke(new SubCliApplication<TSubCommand>(this, subCommand));

            return this;
        }

        public virtual int Execute(string[] args)
        {
            return RootCommand.Execute(args ?? new string[0]);
        }

        private class SubCliApplication<TSubCommand> : CliApplication<TSubCommand> where TSubCommand : CliCommand
        {
            public CliApplication<TCommand> Parent { get; }

            public SubCliApplication(CliApplication<TCommand> parent, CommandLineApplication subCommand)
                : base(subCommand)
            {
                Parent = parent;
            }
        }
    }
}
