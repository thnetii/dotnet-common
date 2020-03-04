using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
        /// arguments as passed to the main entry point of the application.
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
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="definition"/> or the
        /// <see cref="ICommandDefinition.RootCommand"/> property of
        /// <paramref name="definition"/> is <see langword="null"/> .
        /// </exception>
        public static Task<int> InvokeAsync(ICommandDefinition definition,
            string[] args, Action<IHostBuilder>? configureHost = null)
        {
            _ = definition ?? throw new ArgumentNullException(nameof(definition));

            configureHost = (host => host.ConfigureServices(services =>
            {
                services.Add(ServiceDescriptor.Singleton(
                    definition.GetType(), definition
                    ));
            })) + configureHost;

            return InvokeAsync(definition.RootCommand, args, configureHost);
        }

        /// <summary>
        /// Build and configures a hosted command-line application using its
        /// application specific root-command definition and its command-line
        /// arguments as passed to the main entry point of the application.
        /// </summary>
        /// <param name="rootCommand">
        /// The application specific root command that instruct the
        /// command-line argument parser on how to parse the arguments supplied
        /// in <paramref name="args"/>.
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rootCommand"/> is <see langword="null"/>.
        /// </exception>
        public static Task<int> InvokeAsync(Command rootCommand, string[] args,
            Action<IHostBuilder>? configureHost = null)
        {
            _ = rootCommand ?? throw new ArgumentNullException(nameof(rootCommand));

            var parser = new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .UseHost(Host.CreateDefaultBuilder, host =>
                {
                    host.ConfigureServices((context, services) =>
                    {
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
