﻿using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using THNETII.Common.Cli.Infrastructure;

namespace THNETII.Common.Cli
{
    using ConfigureCommandAction = Action<CliCommandDefinition>;

    public class CliBuilder<TCommand> where TCommand : CliCommand
    {
        private static readonly string[] emptyArguments = Array.Empty<string>();

        private Action<CliCommandDefinition> configureCommand;
        private ServiceProviderOptions serviceProviderOptions;

        public CliBuilder<TCommand> WithCommandDetails(ConfigureCommandAction configureCommand)
        {
            this.configureCommand += configureCommand;
            return this;
        }

        public CliBuilder<TCommand> WithServiceCollection(Action<IServiceCollection> configureServices)
        {
            throw new NotImplementedException();
        }

        public CliBuilder<TCommand> WithConfigurationBuilder(Action<IConfigurationBuilder> configurationBuilder)
        {
            throw new NotImplementedException();
        }

        protected TResult DoExecute<TResult>(CommandLineApplication app, string[] args, Func<TCommand, CommandLineApplication, TResult> commandExecute)
        {
            var commandDefinition = new CliCommandDefinition();
            configureCommand?.Invoke(commandDefinition);
            ICommandLineApplicationConfigure commandConfigure = commandDefinition;
            commandConfigure.ConfigureApplication(app);

            TResult executeResult = default;
            app.OnExecute(() =>
            {
                var serviceCollection = new ServiceCollection();

                var serviceProvider = serviceProviderOptions == null
                    ? serviceCollection.BuildServiceProvider()
                    : serviceCollection.BuildServiceProvider(serviceProviderOptions)
                    ;

                var cmd = serviceProvider.GetRequiredService<TCommand>();
                executeResult = commandExecute(cmd, app);
            });
            app.Execute(args ?? emptyArguments);
            return executeResult;
        }
    }
}
