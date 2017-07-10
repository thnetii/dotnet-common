using System;
using Microsoft.Extensions.CommandLineUtils;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace THNETII.Common.Cli
{
    public class CliBuilder<TCommand> where TCommand : CliCommand
    {
        public const string DefaultHelpTemplate = @"-?|-h|--help";
        public const string DefaultVersionTemplate = @"--version";

        private static readonly Assembly CommandAssembly = typeof(TCommand).GetTypeInfo().Assembly;
        private static readonly AssemblyName CommandAssemblyName = CommandAssembly.GetName();
        private static readonly string shortVersionString = GetShortVersionString();
        private static readonly string longVersionString = GetLongVersionString();
        private static readonly string rootNameString = CommandAssemblyName.Name;
        private static readonly string rootFullNameString = CommandAssembly.GetCustomAttribute<AssemblyProductAttribute>()?.Product;
        private static readonly string rootDescriptionString = CommandAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description;

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

        public IConfigurationBuilder ConfigurationBuilder { get; } = new ConfigurationBuilder();
        public IServiceCollection ServiceCollection { get; } = new ServiceCollection();
        public IDictionary<string, string> ConfigurationDictionary { get; } = new Dictionary<string, string>();

        public CliBuilder()
        {
        }

        public CliBuilder<TCommand> AddHelpOption() => AddHelpOption(DefaultHelpTemplate);

        public CliBuilder<TCommand> AddHelpOption(string template)
        {
            throw new NotImplementedException();
        }

        public CliBuilder<TCommand> AddVersionOption() => AddVersionOption(DefaultVersionTemplate);

        public CliBuilder<TCommand> AddVersionOption(string template)
        {
            throw new NotImplementedException();
        }

        public CliBuilder<TCommand> AddOption(string template, string description, CommandOptionType type, Action<CommandOption, IDictionary<string, string>> readToConfiguration)
            => AddOption(template, description, type, null, default(bool), readToConfiguration);

        public CliBuilder<TCommand> AddOption(string template, string description, CommandOptionType type, Action<CommandOption> configureOption, Action<CommandOption, IDictionary<string, string>> readToConfiguration)
            => AddOption(template, description, type, configureOption, default(bool), readToConfiguration);

        public CliBuilder<TCommand> AddOption(string template, string description, CommandOptionType type, bool inherited, Action<CommandOption, IDictionary<string, string>> readToConfiguration)
            => AddOption(template, description, type, null, inherited, readToConfiguration);

        public CliBuilder<TCommand> AddOption(string template, string description, CommandOptionType type, Action<CommandOption> configureOption, bool inherited, Action<CommandOption, IDictionary<string, string>> readToConfiguration)
        {
            throw new NotImplementedException();
        }

        public CliBuilder<TCommand> AddSubCommand<TSubCommand>(string name, Action<CliBuilder<TSubCommand>> subCliBuilder, Action<CommandLineApplication> configuration = null)
            where TSubCommand : CliCommand
            => AddSubCommand(name, null, subCliBuilder, configuration);

        public CliBuilder<TCommand> AddSubCommand<TSubCommand>(string name, string description, Action<CliBuilder<TSubCommand>> subCliBuilder, Action<CommandLineApplication> configuration = null)
            where TSubCommand : CliCommand
        {
            throw new NotImplementedException();
        }

        public CliBuilder<TCommand> PrepareServiceProvider(Action<IServiceProvider> serviceProviderAction)
        {
            throw new NotImplementedException();
        }

        private int Run(CommandLineApplication app)
        {
            ServiceCollection.AddSingleton<IConfiguration>(ConfigurationBuilder.AddInMemoryCollection(ConfigurationDictionary).Build());
            ServiceCollection.AddSingleton<TCommand>();

            var serviceProvider = ServiceCollection.BuildServiceProvider();
            var command = serviceProvider.GetRequiredService<TCommand>();
            return command.Run(app);
        }

        public virtual CommandLineApplication Build(bool throwOnUnexpectedArg = false)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg)
            {
                Name = rootNameString,
                Description = rootDescriptionString,
                FullName = rootFullNameString
            };

            app.OnExecute(() => Run(app));

            return app;
        }
    }
}