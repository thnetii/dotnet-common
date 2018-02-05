using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using THNETII.DependencyInjection.Nesting;

namespace THNETII.DependencyInjection.Configuration
{
    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddUsingConfigurationSection(
            this IServiceCollection services, string sectionName,
            Action<INestedServiceCollection> configureServices)
        {
            IConfiguration getConfigurationSection(IServiceProvider serviceProvider)
            {
                return serviceProvider
                    .GetService<RootServiceProviderAccessor>()?
                    .RootServiceProvider
                    .GetService<IConfiguration>()?
                    .GetSection(sectionName);
            }

            Action<INestedServiceCollection> configNested;
            configNested = ns => ns.WithRootServicesAddBehavior(
                RootServicesAddBehavior.None, nsc => nsc
                    .AddSingleton(getConfigurationSection));
            configNested += configureServices;
            services.AddNestedServices<IConfiguration>(sectionName, true,
                configNested);
            return services;
        }
    }
}
