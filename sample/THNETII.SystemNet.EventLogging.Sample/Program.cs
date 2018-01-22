using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using THNETII.Common;
using THNETII.Logging.EventSource;

namespace THNETII.SystemNet.EventLogging.Sample
{
    public static class Program
    {
        private const string LoggingKey = nameof(Microsoft.Extensions.Logging);

        public static void Main()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { ConfigurationPath.Combine(LoggingKey, nameof(LogLevel), "System.Net"), nameof(LogLevel.Trace) }
                })
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton(config)
                .AddLogging(logging => logging
                    .AddConfiguration(config?.GetSection(LoggingKey))
                    .AddConsole()
                    .AddDebug()
                    )
                .BuildServiceProvider();
#if NET461
            var anyEntry = Dns.GetHostEntry(IPAddress.Loopback);
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
            System.Diagnostics.Trace.Refresh();
            var socketsTraceSource = new System.Diagnostics.TraceSource("System.Net.Sockets", System.Diagnostics.SourceLevels.All);
#endif
            using (var eventListener = LoggingEventSourceListener.Create(config?.GetSection(LoggingKey), serviceProvider.GetService<ILoggerFactory>()))
            {
                var hostEntry = Dns.GetHostEntry("www.google.com");
                var hostEntryTextBuilder = new StringBuilder();
                hostEntryTextBuilder.AppendLine($"DNS IP Host Entry for {hostEntry.HostName}:");
                if (hostEntry.Aliases.EmptyIfNull().Any())
                {
                    foreach (var hostEntryAlias in hostEntry.Aliases.Select((a, i) => (a, i)))
                        hostEntryTextBuilder.Append($"\tAlias {hostEntryAlias.i}: {hostEntryAlias.a}");
                }
                if (hostEntry.AddressList.EmptyIfNull().Any())
                {
                    foreach (var hostEntryAddress in hostEntry.AddressList.Select((ip, i) => (ip, i)))
                        hostEntryTextBuilder.Append($"\tAddress {hostEntryAddress.i}: {hostEntryAddress.ip}");
                }
                Console.WriteLine(hostEntryTextBuilder.ToString());
            }
        }
    }
}
