using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace THNETII.Common.Cli
{
    public class CliBuilder<TCommand> where TCommand : CliCommand
    {
        /// <summary>
        /// The default command-line option template string that is used for the version option in a CLI.
        /// </summary>
        public const string DefaultVersionTemplate = @"--version";

        private Action<IConfigurationBuilder> configureAction;
        private Action<IConfiguration, IServiceCollection> configureServices;
        private bool inheritedHelpOption;
        private bool inheritedVersionOption;

        /// <summary>
        /// Adds an action to be executed prior to running a CLI command in order to add configuration sources for the Application.
        /// </summary>
        /// <param name="configureAction">The action to execute on the Application configuration builder.</param>
        /// <returns>The current instance to allow for chaining method invocations in a functional-style.</returns>
        public CliBuilder<TCommand> AddConfiguration(Action<IConfigurationBuilder> configureAction)
        {
            this.configureAction += configureAction;
            return this;
        }

        /// <summary>
        /// Add an action to be executed prior to running a CLI command in order to add services to the Application DI-container.
        /// </summary>
        /// <param name="configureServices">The action to execute on the Service Collection of the Application DI-container.</param>
        /// <returns>The current instance to allow for chaining method invocations in a functional-style.</returns>
        public CliBuilder<TCommand> ConfigureServices(Action<IServiceCollection> configureServices)
            => ConfigureServices((_, services) => configureServices?.Invoke(services));

        /// <summary>
        /// Add an action to be executed prior to running a CLI command in order to add services to the Application DI-container.
        /// </summary>
        /// <param name="configureServices">The action to execute on the Service Collection of the Application DI-container.</param>
        /// <returns>The current instance to allow for chaining method invocations in a functional-style.</returns>
        public CliBuilder<TCommand> ConfigureServices(Action<IConfiguration, IServiceCollection> configureServices)
        {
            this.configureServices += configureServices;
            return this;
        }

        public CliBuilder<TCommand> AddHelpOption()
            => throw new NotImplementedException();

        public CliBuilder<TCommand> AddHelpOption(string template)
            => throw new NotImplementedException();

        public virtual CommandLineApplication Build()
        {
            throw new NotImplementedException();
        }

        private class NestedCliBuilder<TSubCommand> : CliBuilder<TSubCommand> where TSubCommand : CliCommand
        {
            public CliBuilder<TCommand> Parent { get; }

            public NestedCliBuilder(CliBuilder<TCommand> parent) : base()
            {
                Parent = parent;
            }
        }
    }
}
