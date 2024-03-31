using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

using THNETII.Common;
using THNETII.DependencyInjection.Configuration;

namespace THNETII.DependencyInjection.Nesting.Sample
{
    public static class Program
    {
        [SuppressMessage("Reliability", "CA2000: Dispose objects before losing scope")]
        public static void Main(string[] args)
        {
            const string configJsonPath = "appsettings.json";
            var rootConfig = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    [ConfigurationPath.Combine(nameof(Microsoft.Extensions.Logging), nameof(LogLevel), "Default")] = nameof(LogLevel.Information)
                })
                .AddJsonFile(configJsonPath, optional: true, reloadOnChange: true)
                .AddJsonFile(new PhysicalFileProvider(Environment.CurrentDirectory), configJsonPath, optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args.ZeroLengthIfNull())
                .Build();
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IConfiguration>(rootConfig)
                .AddUsingConfigurationSection(nameof(Microsoft.Extensions.Logging), services => services
                    .AddLogging(logging => logging
                        .AddDebug()
                        .AddConsole()
                        .AddConfiguration()
                        )
                    )
                .Build();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(nameof(Program));
            logger.LogDebug("DependencyInjection container configured");
            logger.LogInformation("Program complete, exiting.");
            if (serviceProvider is IDisposable disp)
                disp.Dispose();
        }
    }
}
