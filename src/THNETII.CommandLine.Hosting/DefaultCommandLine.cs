using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace THNETII.CommandLine.Hosting
{
    /// <summary>
    /// Provides helper methods for the default behaviour of a command-line
    /// application.
    /// </summary>
    public static class DefaultCommandLine
    {
        /// <summary>
        /// Build and configures a hosted command-line application using its
        /// application specific root-command definition and its command-line
        /// arguments as passed to main entry point of the application.
        /// </summary>
        /// <param name="definition">
        /// The application specific command definition that will be injected
        /// as a singleton service into the <see cref="IHost"/> that host the
        /// invocation.
        /// </param>
        /// <param name="args">
        /// The array of <see cref="string"/> values that are passed by the OS
        /// to the application main entry point as arguments. Defaults to an
        /// empty array.
        /// </param>
        /// <param name="configureHost">
        /// Configuration action to further configure application specific
        /// behaviour, such as custom services, logging and configuration.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous invocation of the defined
        /// root-command (or one of its sub-commands).
        /// </returns>
        public static Task<int> InvokeAsync(ICommandDefinition definition,
            string[] args, Action<IHostBuilder>? configureHost = null)
        {
            if (definition is null)
                throw new ArgumentNullException(nameof(definition));

            var parser = new CommandLineBuilder(definition.RootCommand)
                .UseDefaults()
                .UseHost(Host.CreateDefaultBuilder, host =>
                {
                    host.ConfigureServices((context, services) =>
                    {
                        services.Add(ServiceDescriptor.Singleton(
                            definition.GetType(), definition
                            ));
                        services.AddOptions<InvocationLifetimeOptions>()
                            .Configure<IConfiguration>((opts, config) =>
                                config.Bind("Lifetime", opts));
                    });
                    configureHost?.Invoke(host);
                })
                .Build();

            return parser.InvokeAsync(args ?? Array.Empty<string>());
        }
    }
}
