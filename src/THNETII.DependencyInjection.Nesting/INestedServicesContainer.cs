using System;

namespace THNETII.DependencyInjection.Nesting
{
    public interface INestedServicesContainer<TFamily, TKey>
    {
        IServiceProvider GetServiceProvider(TKey key);
    }
}
