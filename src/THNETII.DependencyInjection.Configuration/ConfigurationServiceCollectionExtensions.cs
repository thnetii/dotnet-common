using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

using THNETII.DependencyInjection.Nesting;

namespace THNETII.DependencyInjection.Configuration
{
    /// <summary>
    /// Provides Configuration related extension methods for
    /// <see cref="IServiceCollection"/> instances.
    /// </summary>
    public static class ConfigurationServiceCollectionExtensions
    {
        /// <summary>
        /// Configures a nested service container providing access to an
        /// <see cref="IConfiguration"/> service that using the specified
        /// configuration section name.
        /// </summary>
        /// <param name="services">The parent service container.</param>
        /// <param name="sectionName">The configuration section name to use.</param>
        /// <param name="configureServices">The actions to execute to configure the nested service container.</param>
        /// <returns>The instance specified in the <paramref name="services"/> parameter, to allow to fluent API chaining.</returns>
        public static IServiceCollection AddUsingConfigurationSection(
            this IServiceCollection services, string sectionName,
            Action<INestedServiceCollection> configureServices)
        {
            IConfiguration? getConfigurationSection(IServiceProvider serviceProvider)
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
