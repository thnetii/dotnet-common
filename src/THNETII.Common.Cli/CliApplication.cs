using CommandLine;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace THNETII.Common.Cli
{
    using CommandFunc = Func<object, IServiceProvider, int>;
    using CommandAsyncFunc = Func<object, IServiceProvider, Task<int>>;

    public class CliApplication
    {
        private class CliCommandDefinition
        {
            public Type OptionsType { get; }
            public CommandFunc CommandFunc { get; }
            public CommandAsyncFunc CommandAsyncFunc { get; }
            public bool IsAsyncCommand => CommandAsyncFunc != null;
            public CliCommandDefinition(Type optionsType) { OptionsType = optionsType; }
            public CliCommandDefinition(Type optionsType, CommandFunc commandFunc)
                : this(optionsType) { CommandFunc = commandFunc; }
            public CliCommandDefinition(Type optionsType, CommandAsyncFunc commandAsyncFunc)
                : this(optionsType) { CommandAsyncFunc = commandAsyncFunc; }
        }

        private class CliCommandDefinition<TOptions> : CliCommandDefinition where TOptions : new()
        {
            private static readonly Type staticType = typeof(TOptions);
            public CliCommandDefinition(Func<TOptions, IServiceProvider, int> commandFunc)
                : base(staticType, GetCommandFunc(commandFunc.ThrowIfNull(nameof(commandFunc)))) { }
            public CliCommandDefinition(Func<TOptions, IServiceProvider, Task<int>> commandAsyncFunc)
                : base(staticType, GetCommandFunc(commandAsyncFunc.ThrowIfNull(nameof(commandAsyncFunc)))) { }
            private static CommandFunc GetCommandFunc(Func<TOptions, IServiceProvider, int> commandFunc)
                => (opts, serviceProvider) => commandFunc((TOptions)opts, serviceProvider);
            private static CommandAsyncFunc GetCommandFunc(Func<TOptions, IServiceProvider, Task<int>> commandAsyncFunc)
                => (opts, serviceProvider) => commandAsyncFunc((TOptions)opts, serviceProvider);
        }

        public const int ExitSuccess = 0;
        public const int ExitFailure = 1;

        private readonly Parser parser;
        private readonly ICollection<Action<IConfigurationBuilder>> configurationBuilderActions = new List<Action<IConfigurationBuilder>>(1);
        private readonly ICollection<Action<IServiceCollection>> configureServicesActions = new List<Action<IServiceCollection>>(1);
        private readonly ICollection<Action<IServiceProvider>> alwaysRunActions = new List<Action<IServiceProvider>>(1);
        private CliCommandDefinition rootCommand;
        private readonly ICollection<CliCommandDefinition> subCommands = new List<CliCommandDefinition>(5);

        public CliApplication() : this(Parser.Default) { }

        public CliApplication(Action<ParserSettings> parserConfiguration) : this(new Parser(parserConfiguration)) { }

        public CliApplication(Parser parser)
        {
            this.parser = parser.ThrowIfNull(nameof(parser));
        }

        public CliApplication ConfigurationBuilder(Action<IConfigurationBuilder> configurationBuilderAction)
        {
            if (configurationBuilderAction != null)
                configurationBuilderActions.Add(configurationBuilderAction);
            return this;
        }

        public CliApplication ConfigureServices(Action<IServiceCollection> configureServicesAction)
        {
            if (configureServicesAction != null)
                configureServicesActions.Add(configureServicesAction);
            return this;
        }

        public CliApplication AlwaysRun(Action<IServiceProvider> runAction)
        {
            if (runAction != null)
                alwaysRunActions.Add(runAction);
            return this;
        }

        public CliApplication RootCommand<TOptions>(Func<TOptions, IServiceProvider, int> commandFunc)
            where TOptions : new()
        {
            rootCommand = commandFunc != null ? new CliCommandDefinition<TOptions>(commandFunc) : null;
            return this;
        }

        public CliApplication RootCommand<TOptions>(Func<TOptions, IServiceProvider, Task<int>> commandAsyncFunc)
            where TOptions : new()
        {
            rootCommand = commandAsyncFunc != null ? new CliCommandDefinition<TOptions>(commandAsyncFunc) : null;
            return this;
        }

        public CliApplication UseShortenedCommandMatching()
        {
            throw new NotImplementedException();
        }

        public CliApplication AddSubCommand<TOptions>(Func<TOptions, IServiceProvider, int> subCommandFunc)
            where TOptions : new()
        {
            if (subCommandFunc != null)
                subCommands.Add(new CliCommandDefinition<TOptions>(subCommandFunc));
            return this;
        }

        public CliApplication AddSubCommand<TOptions>(Func<TOptions, IServiceProvider, Task<int>> subCommandAsyncFunc)
            where TOptions : new()
        {
            if (subCommandAsyncFunc != null)
                subCommands.Add(new CliCommandDefinition<TOptions>(subCommandAsyncFunc));
            return this;
        }

        public int Execute(IEnumerable<string> args)
        {
            args = args.IfNotNull(otherwise: Enumerable.Empty<string>());
            startExecute:
            var usableSubCommands = subCommands.Where(def => def.OptionsType != null);
            if (usableSubCommands.Any())
            {

            }
            else if (rootCommand != null)
            {

            }

            throw new NotImplementedException();
        }
    }
}
