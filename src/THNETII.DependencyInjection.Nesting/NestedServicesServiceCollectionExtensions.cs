using Microsoft.Extensions.DependencyInjection;
using System;

namespace THNETII.DependencyInjection.Nesting
{
    public static class NestedServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddNestedServices(
            this IServiceCollection rootServices,
            string key,
            Action<INestedServiceCollection> configureServices)
        {
            throw new NotImplementedException();
            return rootServices;
        }
    }
}
