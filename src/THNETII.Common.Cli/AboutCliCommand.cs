using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace THNETII.Common.Cli
{
    public class AboutCliCommand : CliCommand
    {
        private static readonly Regex pascalCaseBoundaryMatcher = new Regex(@"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])");
        private readonly Assembly executeAssembly;

        public AboutCliCommand(
            Assembly executeAssembly,
            IConfiguration configuration = null, 
            ILogger<AboutCliCommand> logger = null) 
            : base(configuration, logger)
        {
            this.executeAssembly = executeAssembly ?? typeof(AboutCliCommand).GetTypeInfo().Assembly;
        }

        private static void WriteKeyValuePairFormatted(TextWriter writer, IEnumerable<KeyValuePair<string, object>> keyValuePairs)
        {
            keyValuePairs = keyValuePairs.ToList();
            var longestKeyLength = keyValuePairs.Max(kvp => kvp.Key.Length + 1);
            foreach (var kvp in keyValuePairs)
                writer.WriteLine($"{$"{kvp.Key}:".PadRight(longestKeyLength)} {kvp.Value}");
        }

        public override int Run(CommandLineApplication app)
        {
            app.ShowRootCommandFullNameAndVersion();

            var writer = app?.Out ?? Console.Out;

            KeyValuePair<string, object> MemberInfoToKvp(PropertyInfo propInfo, object instance)
            {
                string key = pascalCaseBoundaryMatcher.Replace(propInfo.Name, " ");
                return new KeyValuePair<string, object>(key, propInfo.GetValue(instance));
            }
            KeyValuePair<string, object> StaticMemberInfoToKvp(PropertyInfo propInfo) => MemberInfoToKvp(propInfo, null);

            writer.WriteLine("Executable Assembly Attributes:");
            var execAssemblyName = executeAssembly.GetName();
            WriteKeyValuePairFormatted(writer, typeof(AssemblyName).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(propInfo => MemberInfoToKvp(propInfo, executeAssembly.GetName())));
            WriteKeyValuePairFormatted(writer, executeAssembly.GetCustomAttributes().Select(attr =>
            {
                Type type = attr.GetType();
                string key = type.Name;
                if (key.StartsWith("Assembly", StringComparison.OrdinalIgnoreCase))
                    key = key.Substring("Assembly".Length);
                if (key.EndsWith(nameof(Attribute), StringComparison.OrdinalIgnoreCase))
                    key = key.Substring(0, key.Length - nameof(Attribute).Length);
                var propInfo = type.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propInfo == null)
                    return default(KeyValuePair<string, object>);
                key = pascalCaseBoundaryMatcher.Replace(key, " ");
                var value = propInfo.GetValue(attr);
                return new KeyValuePair<string, object>(key, value);
            }).Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key)));

            writer.WriteLine();

            writer.WriteLine("Execution environment:");
            

            WriteKeyValuePairFormatted(writer, typeof(RuntimeInformation).GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(StaticMemberInfoToKvp));

            return 0;
        }
    }
}
