using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace THNETII.DependencyInjection.Nesting
{
    public interface INestedServiceCollection : IServiceCollection
    {
        IServiceCollection InheritedServices { get; }

        RootServicesAddBehavior RootServicesAddBehavior { get; set; }
    }

    public interface INestedServiceCollection<out TKey> : INestedServiceCollection
    {
        TKey Key { get; }
    }

    public interface INestedServiceCollection<TFamily, out TKey>
        : INestedServiceCollection<TKey>
    { }
}
