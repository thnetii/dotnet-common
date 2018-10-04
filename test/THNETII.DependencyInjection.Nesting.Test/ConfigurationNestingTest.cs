using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;
using THNETII.DependencyInjection.Configuration;

namespace THNETII.DependencyInjection.Nesting.Test
{
    public static class ConfigurationNestingTest
    {
        public class TestService
        {
            public const string testKey = "test";
            private readonly IConfiguration config;

            public TestService(IConfiguration config)
            {
                this.config = config;
            }

            public string Test => config?[testKey];
        }

        [Fact]
        public static void NestedConfigurationInstancesAreIsolated()
        {
            const string test1Prefix = "test1";
            const string test2Prefix = "test2";

            var configDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [ConfigurationPath.Combine(test1Prefix, TestService.testKey)] = nameof(test1Prefix),
                [ConfigurationPath.Combine(test2Prefix, TestService.testKey)] = nameof(test2Prefix),
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(configDict)
                .Build();

            void configureTestService(INestedServiceCollection services)
                => services.AddSingleton<TestService>();

            var serviceProvider = new ServiceCollection()
                .AddSingleton(config)
                .AddUsingConfigurationSection(test1Prefix, configureTestService)
                .AddUsingConfigurationSection(test2Prefix, configureTestService)
                .Build();
            try
            {
                IServiceProvider nestedServiceProvider;
                TestService testInstance;

                nestedServiceProvider = serviceProvider
                    .GetNestedServiceProvider<IConfiguration>(test1Prefix);
                testInstance = nestedServiceProvider.GetRequiredService<TestService>();
                Assert.Equal(nameof(test1Prefix), testInstance.Test);

                nestedServiceProvider = serviceProvider
                    .GetNestedServiceProvider<IConfiguration>(test2Prefix);
                testInstance = nestedServiceProvider.GetRequiredService<TestService>();
                Assert.Equal(nameof(test2Prefix), testInstance.Test);
            }
            finally
            {
                if (serviceProvider is IDisposable disp)
                    disp.Dispose();
            }
        }
    }
}
