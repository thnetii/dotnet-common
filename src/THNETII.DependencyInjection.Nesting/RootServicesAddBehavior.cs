using Microsoft.Extensions.DependencyInjection.Extensions;

namespace THNETII.DependencyInjection.Nesting
{
    /// <summary>
    /// Specifies different behaviours controlling if and how nested service
    /// descriptors are added the parent service collection.
    /// </summary>
    public enum RootServicesAddBehavior
    {
        /// <summary>
        /// Do not add nested services to the parent collection.
        /// </summary>
        None = 0,
        /// <summary>
        /// Add nested services to the parent collection using the
        /// <see cref="ServiceCollectionDescriptorExtensions.Add(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.Extensions.DependencyInjection.ServiceDescriptor)"/>
        /// extension method.
        /// </summary>
        Add = 1,
        /// <summary>
        /// Add nested services to the parent collection using the
        /// <see cref="ServiceCollectionDescriptorExtensions.TryAdd(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.Extensions.DependencyInjection.ServiceDescriptor)"/>
        /// extension method.
        /// </summary>
        TryAdd = 2,
        /// <summary>
        /// Add nested services to the parent collection using the
        /// <see cref="ServiceCollectionDescriptorExtensions.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.Extensions.DependencyInjection.ServiceDescriptor)"/>
        /// extension method.
        /// </summary>
        TryAddEnumerable = 3,
        /// <summary>
        /// Add nested services to the parent collection using the
        /// <see cref="ServiceCollectionDescriptorExtensions.Replace(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.Extensions.DependencyInjection.ServiceDescriptor)"/>
        /// extension method.
        /// </summary>
        Replace = 4,
        /// <summary>
        /// Add nested services to the parent collection using the
        /// <see cref="ServiceCollectionDescriptorExtensions.Add(Microsoft.Extensions.DependencyInjection.IServiceCollection, Microsoft.Extensions.DependencyInjection.ServiceDescriptor)"/>
        /// extension method, ensuring that no other services using the same
        /// service type are registered.
        /// </summary>
        ReplaceAll = 5
    }
}
