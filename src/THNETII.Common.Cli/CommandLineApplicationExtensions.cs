using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace THNETII.Common.Cli
{
    using CommandLineApplicationWR = WeakReference<CommandLineApplication>;
    using ConfigurationBuilderAction = Action<IConfigurationBuilder>;
    using ConfigureServicesAction = Action<IConfiguration, IServiceCollection>;
    using ConfigureOptionAction = Action<CommandOption>;

    public static class CommandLineApplicationExtensions
    {
        private static readonly IEqualityComparer<CommandLineApplicationWR> comp = WeakReferenceComparer.GetDefault<CommandLineApplication>();
        private static readonly ConcurrentDictionary<CommandLineApplicationWR, ConfigurationBuilderAction> allConfigurationBuilders =
            new ConcurrentDictionary<CommandLineApplicationWR, ConfigurationBuilderAction>(comp);
        private static readonly ConcurrentDictionary<CommandLineApplicationWR, ConfigureServicesAction> allConfigureServices =
            new ConcurrentDictionary<CommandLineApplicationWR, ConfigureServicesAction>(comp);

        private static void ClearDisposedKeys<TRef, TValue>(this ConcurrentDictionary<WeakReference<TRef>, TValue> dictionary)
            where TRef : class
        {
            var removeList = new List<WeakReference<TRef>>(dictionary.Count);
            foreach (var wRef in dictionary.Keys)
            {
                if (wRef is null || !wRef.TryGetTarget(out var _))
                    removeList.Add(wRef);
            }
            foreach (var wRef in removeList)
                dictionary.TryRemove(wRef, out var _);
        }

        private static void ClearDisposed()
        {
            allConfigureServices.ClearDisposedKeys();
        }

        private static CommandLineApplicationWR GetWeakReference(this CommandLineApplication app)
            => new CommandLineApplicationWR(app.ThrowIfNull(nameof(app)));

        public static CommandLineApplication WithServiceCollection(this CommandLineApplication app, ConfigureServicesAction configureServices)
        {
            var weak = app.GetWeakReference();
            allConfigureServices.AddOrUpdate(
                weak, configureServices,
                (_, prevConfigureServices) => prevConfigureServices + configureServices);
            return app;
        }

        private static string GetOptionTemplateString(char? symbol, string shortName, string longName, string valueName)
        {
            var template = string.Join("|", new[]
            {
                symbol.HasValue ? "-" + symbol : null,
                string.IsNullOrEmpty(shortName) ? null : '-' + shortName,
                string.IsNullOrEmpty(longName) ? null : "--" + longName
            }.Where(s => s != null));
            if (template.Length == 0 || string.IsNullOrEmpty(valueName))
                return template;
            return template + '<' + valueName + '>';
        }

        public static CommandLineApplication WithOption(
            this CommandLineApplication app,
            string template, CommandOptionType optionType, 
            ConfigureOptionAction configureOption)
        {
            var option = app.ThrowIfNull(nameof(app)).Option(
                template,
                description: null,
                optionType: optionType);
            configureOption?.Invoke(option);
            return app;
        }

        public static int ExecuteCommand<T>(
            this CommandLineApplication app,
            params string[] args)
            where T : CliCommand
            => DoExecuteCommand<T, int>(app, args,
                cmd => cmd.Run(app));

        public static Task<int> ExecuteCommandAsync<T>(
            this CommandLineApplication app,
            params string[] args)
            where T : CliAsyncCommand
            => ExecuteCommandAsync<T>(app, args, default);

        public static Task<int> ExecuteCommandAsync<T>(
            this CommandLineApplication app,
            string[] args,
            CancellationToken cancellationToken)
            where T : CliAsyncCommand
            => DoExecuteCommand<T, Task<int>>(app, args,
                cmd => cmd.RunAsync(app, cancellationToken));

        private static void ConfigureOnExecute<TCommand>(
            CommandLineApplication app, 
            Action<TCommand> commandExecute)
            where TCommand : CliCommand
        {
            var weak = app.GetWeakReference();
            app.OnExecute(() =>
            {
                var configBuilder = new ConfigurationBuilder();
                if (allConfigurationBuilders.TryGetValue(weak, out var configureConfigurationBuilder))
                    configureConfigurationBuilder(configBuilder);
                var config = configBuilder.Build();
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddSingleton<IConfiguration>(config);
                if (allConfigureServices.TryGetValue(weak, out var configureServices))
                    configureServices(config, serviceCollection);
                serviceCollection.AddSingleton<TCommand>();
                var serviceProvider = serviceCollection.BuildServiceProvider();
                using (serviceProvider)
                {
                    var cmd = serviceProvider.GetRequiredService<TCommand>();
                    commandExecute(cmd);
                }
            });
        }

        private static TResult DoExecuteCommand<TCommand, TResult>(
            CommandLineApplication app,
            string[] args,
            Func<TCommand, TResult> commandExecute)
            where TCommand : CliCommand
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app));


            TResult result = default;
            ConfigureOnExecute<TCommand>(app, cmd => result = commandExecute(cmd));
            app.Execute(args ?? Array.Empty<string>());
            return result;
        }
    }
}
