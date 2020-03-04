using System;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace THNETII.CommandLine.Hosting
{
    /// <summary>
    /// Provides extension methods for <see cref="IConfiguration"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Add a source that maps configuration entries from the command-line
        /// parser <see cref="ParseResult"/> using the specified host builder
        /// context.
        /// </summary>
        /// <param name="configBuilder">The configuration builder to add the configuration source to.</param>
        /// <param name="context">The current host builder context providing the <see cref="ParseResult"/>.</param>
        /// <param name="sourceActions">A method or lambda expression specifying how command-line arguments are to be translates to configuration keys.</param>
        /// <returns><paramref name="configBuilder"/> to enable the possibility for chaining method calls.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configBuilder"/> or <paramref name="context"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="context"/> does not provide an <see cref="InvocationContext"/>.
        /// </exception>
        [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
        public static IConfigurationBuilder AddCommandLineParseResult(
            this IConfigurationBuilder configBuilder,
            HostBuilderContext context,
            Action<CommandLineParseResultConfigurationSource>? sourceActions = null)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            _ = context.Properties.TryGetValue(typeof(InvocationContext),
                out object invocationObj);
            _ = invocationObj ?? throw new ArgumentException("The provided host builder context does not provide an Command-line Parser Invocation context.", nameof(context));

            return AddCommandLineParseResult(configBuilder,
                (InvocationContext)invocationObj, sourceActions);
        }

        /// <summary>
        /// Add a source that maps configuration entries from the command-line
        /// parser <see cref="ParseResult"/> using the specified invocation
        /// context.
        /// </summary>
        /// <param name="configBuilder">The configuration builder to add the configuration source to.</param>
        /// <param name="context">The command-line parser invocation context providing the <see cref="ParseResult"/>.</param>
        /// <param name="sourceActions">A method or lambda expression specifying how command-line arguments are to be translates to configuration keys.</param>
        /// <returns><paramref name="configBuilder"/> to enable the possibility for chaining method calls.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configBuilder"/> or <paramref name="context"/> is <see langword="null"/>.
        /// </exception>
        public static IConfigurationBuilder AddCommandLineParseResult(
            this IConfigurationBuilder configBuilder,
            InvocationContext context,
            Action<CommandLineParseResultConfigurationSource>? sourceActions = null) =>
            AddCommandLineParseResult(configBuilder,
                (context ?? throw new ArgumentNullException(nameof(context)))
                .ParseResult, sourceActions);

        /// <summary>
        /// Add a source that maps configuration entries from the command-line
        /// parser <see cref="ParseResult"/>.
        /// </summary>
        /// <param name="configBuilder">The configuration builder to add the configuration source to.</param>
        /// <param name="parseResult">The command-line parser result to map configuration keys from.</param>
        /// <param name="sourceActions">A method or lambda expression specifying how command-line arguments are to be translates to configuration keys.</param>
        /// <returns><paramref name="configBuilder"/> to enable the possibility for chaining method calls.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configBuilder"/> or <paramref name="parseResult"/> is <see langword="null"/>.
        /// </exception>
        public static IConfigurationBuilder AddCommandLineParseResult(
            this IConfigurationBuilder configBuilder, ParseResult parseResult,
            Action<CommandLineParseResultConfigurationSource>? sourceActions = null)
        {
            _ = configBuilder ?? throw new ArgumentNullException(nameof(configBuilder));
            _ = parseResult ?? throw new ArgumentNullException(nameof(parseResult));

            var source = new CommandLineParseResultConfigurationSource(parseResult);
            sourceActions?.Invoke(source);
            configBuilder.Add(source);

            return configBuilder;
        }
    }
}
