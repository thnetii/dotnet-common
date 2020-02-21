using System;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace THNETII.CommandLine.Hosting
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddCommandLineParseResult(
            this IConfigurationBuilder configBuilder,
            HostBuilderContext context, 
            Action<CommandLineParseResultConfigurationSource>? sourceActions = null)
        {
            _ = context ?? throw new ArgumentNullException(nameof(context));

            _ = context.Properties.TryGetValue(typeof(InvocationContext),
                out object invocationObj);

            return AddCommandLineParseResult(configBuilder,
                (InvocationContext)invocationObj, sourceActions);
        }

        public static IConfigurationBuilder AddCommandLineParseResult(
            this IConfigurationBuilder configBuilder,
            InvocationContext context,
            Action<CommandLineParseResultConfigurationSource>? sourceActions = null) =>
            AddCommandLineParseResult(configBuilder,
                (context ?? throw new ArgumentNullException(nameof(context)))
                .ParseResult, sourceActions);

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
