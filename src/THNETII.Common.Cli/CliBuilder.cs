using System;
using Microsoft.Extensions.CommandLineUtils;
using System.Reflection;

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

        public CommandLineApplication Build(bool throwOnUnexpectedArg = false)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg)
            {
                Name = rootNameString,
                Description = rootDescriptionString,
                FullName = rootFullNameString
            };

            return app;

        }
    }
}