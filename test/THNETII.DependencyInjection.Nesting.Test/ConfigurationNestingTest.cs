using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace THNETII.DependencyInjection.Nesting.Test
{
    public class ConfigurationNestingTest
    {
        [Fact]
        public static void NestedConfigurationInstancesAreIsolated()
        {
            const string test1Prefix = "test1";
            const string test2Prefix = "test2";

            var configDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [""] = ""
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(config)
                .AddNestedServices<IConfiguration>(test1Prefix, services => services
                    .AddAllRootServices()
                    .AddSingleton<IConfiguration>((rootServiceProvider, _)
                        => rootServiceProvider
                            .GetRequiredService<IConfiguration>()
                            .GetSection(test1Prefix)
                        )
                    )
                .BuildServiceProvider();
            using (serviceProvider)
            {

            }
        }
    }
}
