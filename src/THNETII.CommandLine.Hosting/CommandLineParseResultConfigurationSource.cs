using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Globalization;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace THNETII.CommandLine.Hosting
{
    public class CommandLineParseResultConfigurationSource : IConfigurationSource
    {
        private readonly ParseResult parseResult;
        private Action<List<KeyValuePair<string, string>>, ParseResult>? buildActions;

        internal CommandLineParseResultConfigurationSource(ParseResult parseResult)
        {
            this.parseResult = parseResult;
        }

        public CommandLineParseResultConfigurationSource MapOption(string key,
            Option<string> option) =>
            MapOption(key, option, r => r.GetValueOrDefault<string>());

        public CommandLineParseResultConfigurationSource MapOption(string key,
            IOption option, Func<OptionResult, string?> resultToValue)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));
            _ = option ?? throw new ArgumentNullException(nameof(option));
            _ = resultToValue ?? throw new ArgumentNullException(nameof(resultToValue));

            buildActions += (data, parseResult) =>
            {
                if (!(parseResult.FindResultFor(option) is OptionResult result))
                    return;
                var value = resultToValue.Invoke(result) ?? string.Empty;
                data.Add(new KeyValuePair<string, string>(key, value));
            };

            return this;
        }

        public CommandLineParseResultConfigurationSource MapOption(string key,
            IOption option, Func<OptionResult, IEnumerable<string?>?> resultToValue)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));
            _ = option ?? throw new ArgumentNullException(nameof(option));
            _ = resultToValue ?? throw new ArgumentNullException(nameof(resultToValue));

            buildActions += (data, parseResult) =>
            {
                if (!(parseResult.FindResultFor(option) is OptionResult result))
                    return;
                var values = resultToValue.Invoke(result);
                data.AddRange(values.Select((value, idx) =>
                {
                    var itemKey = ConfigurationPath.Combine(key, idx.ToString(CultureInfo.InvariantCulture));
                    return new KeyValuePair<string, string>(itemKey, value ?? string.Empty);
                }));
            };

            return this;
        }

        public CommandLineParseResultConfigurationSource MapArgument(string key,
            Argument<string> argument) =>
            MapArgument(key, argument, r => r.GetValueOrDefault<string>());

        public CommandLineParseResultConfigurationSource MapArgument(string key,
            IArgument argument, Func<ArgumentResult, string?> resultToValue)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));
            _ = argument ?? throw new ArgumentNullException(nameof(argument));
            _ = resultToValue ?? throw new ArgumentNullException(nameof(resultToValue));

            buildActions += (data, parseResult) =>
            {
                if (!(parseResult.FindResultFor(argument) is ArgumentResult result))
                    return;
                var value = resultToValue.Invoke(result) ?? string.Empty;
                data.Add(new KeyValuePair<string, string>(key, value));
            };

            return this;
        }

        public CommandLineParseResultConfigurationSource MapArgument(string key,
            IArgument argument, Func<ArgumentResult, IEnumerable<string?>?> resultToValue)
        {
            _ = key ?? throw new ArgumentNullException(nameof(key));
            _ = argument ?? throw new ArgumentNullException(nameof(argument));
            _ = resultToValue ?? throw new ArgumentNullException(nameof(resultToValue));

            buildActions += (data, parseResult) =>
            {
                if (!(parseResult.FindResultFor(argument) is ArgumentResult result))
                    return;
                var values = resultToValue.Invoke(result);
                data.AddRange(values.Select((value, idx) =>
                {
                    var itemKey = ConfigurationPath.Combine(key, idx.ToString(CultureInfo.InvariantCulture));
                    return new KeyValuePair<string, string>(itemKey, value ?? string.Empty);
                }));
            };

            return this;
        }

        /// <inheritdoc/>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            var data = new List<KeyValuePair<string, string>>();
            buildActions?.Invoke(data, parseResult);
            var source = new MemoryConfigurationSource { InitialData = data };
            return new MemoryConfigurationProvider(source);
        }
    }
}
