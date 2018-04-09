using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using THNETII.Common;
using THNETII.Common.Collections.Generic;

namespace THNETII.DependencyInjection.Nesting
{
    public class NestedServiceCollection<TFamily, TKey>
        : INestedServiceCollection<TFamily, TKey>
    {
        private readonly IEnumerable<ServiceDescriptor> inheritedServices;
        private readonly ServiceDescriptorExceptionList rootProxyDescriptors
            = new ServiceDescriptorExceptionList();
        private readonly IServiceCollection nestedServices;

        private int InheritedCount => (InheritedServices?.Count ?? 0) - rootProxyDescriptors.Count;

        public TKey Key { get; }

        public IServiceCollection InheritedServices { get; }

        public RootServicesAddBehavior RootServicesAddBehavior { get; set; }
            = RootServicesAddBehavior.Add;

        public NestedServiceCollection(TKey key,
            IServiceCollection inheritedServices)
        {
            Key = key;
            InheritedServices = inheritedServices;
            this.inheritedServices = inheritedServices.EmptyIfNull()
                .Except(rootProxyDescriptors,
                    ReferenceEqualityComparer<ServiceDescriptor>.Default);
            nestedServices = new ServiceCollection();
        }

        private ServiceDescriptor GetRootProxyDescriptor(ServiceDescriptor serviceDescriptor)
        {
            var serviceTypeRef = serviceDescriptor.ServiceType
#if NETSTANDARD1_3
                    .GetTypeInfo()
#endif
                    ;
            if (serviceTypeRef.IsGenericTypeDefinition)
                return serviceDescriptor;
            return new ServiceDescriptor(serviceDescriptor.ServiceType,
                rsp => rsp.GetNestedServiceProvider<TFamily, TKey>(Key)
                    .GetService(serviceDescriptor.ServiceType),
                serviceDescriptor.Lifetime);
        }

#if !NETSTANDARD1_3
        private static readonly MethodInfo InternalRemoveAllServicesMethodInfo =
            typeof(NestedServicesServiceCollectionExtensions)
            .GetMethod(nameof(InternalRemoveAllServices), BindingFlags.NonPublic | BindingFlags.Static);

        private static void InternalRemoveAllServices<T>(IServiceCollection services)
            => services.RemoveAll<T>();
#endif

        void ICollection<ServiceDescriptor>.Add(
            ServiceDescriptor serviceDescriptor)
        {
            nestedServices.Add(serviceDescriptor);
            RegisterDescriptorInInheritedServiceCollection(serviceDescriptor);
        }

        private void RegisterDescriptorInInheritedServiceCollection(ServiceDescriptor serviceDescriptor)
        {
            if (InheritedServices != null)
            {
                int beforeAdd = InheritedServices.Count;
                bool ensureAddProxy = false;
                var rootProxyDescriptor = GetRootProxyDescriptor(serviceDescriptor);
                switch (RootServicesAddBehavior)
                {
                    case RootServicesAddBehavior.Add:
                        InheritedServices.Add(rootProxyDescriptor);
                        ensureAddProxy = true;
                        break;
                    case RootServicesAddBehavior.TryAdd:
                        InheritedServices.TryAdd(rootProxyDescriptor);
                        break;
                    case RootServicesAddBehavior.TryAddEnumerable:
                        InheritedServices.TryAddEnumerable(rootProxyDescriptor);
                        break;
                    case RootServicesAddBehavior.Replace:
                        InheritedServices.Replace(rootProxyDescriptor);
                        ensureAddProxy = true;
                        break;
#if !NETSTANDARD1_3
                    case RootServicesAddBehavior.ReplaceAll:
                        var mi = InternalRemoveAllServicesMethodInfo
                            .MakeGenericMethod(rootProxyDescriptor.ServiceType);
                        mi.Invoke(null, new[] { InheritedServices });
                        ensureAddProxy = true;
                        goto case RootServicesAddBehavior.Add;
#endif
                }
                if (InheritedServices.Count != beforeAdd || ensureAddProxy)
                    rootProxyDescriptors.Add(rootProxyDescriptor);
            }
        }

        public IEnumerator<ServiceDescriptor> GetEnumerator()
        {
            return inheritedServices
                .Except(rootProxyDescriptors,
                    ReferenceEqualityComparer<ServiceDescriptor>.Default)
                .Concat(nestedServices)
                .GetEnumerator();
        }

        public ServiceDescriptor this[int index]
        {
            get => (index < InheritedCount)
                ? inheritedServices.ElementAt(index)
                : nestedServices[index - InheritedCount];
            set
            {
                if (index < InheritedCount)
                    InheritedServices[index] = value;
                else
                    nestedServices[index - InheritedCount] = value;
            }
        }

        public int Count => InheritedCount + nestedServices.Count;

        bool ICollection<ServiceDescriptor>.IsReadOnly => nestedServices.IsReadOnly;

        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
        {
            int result = inheritedServices
                .Select((d, i) => d == item ? (int?)i : null)
                .FirstOrDefault(i => i != null) ?? -1;
            if (result < 0)
            {
                result = nestedServices.IndexOf(item);
                if (result < 0)
                    result += InheritedCount;
            }
            return result;
        }

        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
        {
            if (index < InheritedCount)
                throw new InvalidOperationException();
            index -= InheritedCount;
            nestedServices.Insert(index, item);
            RegisterDescriptorInInheritedServiceCollection(item);
        }

        void IList<ServiceDescriptor>.RemoveAt(int index) => RemoveAt(index);

        private void RemoveAt(int index)
        {
            if (index < InheritedCount)
                throw new InvalidOperationException();
            index -= InheritedCount;
            var nestedDesc = nestedServices[index];
            int nestedServiceTypeIdx = nestedServices
                .Take(index)
                .Count(d => d.ServiceType == nestedDesc.ServiceType);
            
            var rootDesc = rootProxyDescriptors
                .Where(d => d.ServiceType == nestedDesc.ServiceType)
                .ElementAt(nestedServiceTypeIdx);
            InheritedServices.Remove(rootDesc);
            rootProxyDescriptors.Remove(rootDesc,
                ReferenceEqualityComparer<ServiceDescriptor>.Default);
        }

        void ICollection<ServiceDescriptor>.Clear()
        {
            nestedServices.Clear();
            foreach (var proxyDesc in rootProxyDescriptors)
                InheritedServices.Remove(proxyDesc);
            rootProxyDescriptors.Clear();
        }

        bool ICollection<ServiceDescriptor>.Contains(ServiceDescriptor item)
            => inheritedServices.Contains(item) && nestedServices.Contains(item);

        void ICollection<ServiceDescriptor>.CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            int i = 0;
            for (var enumerator = inheritedServices.GetEnumerator();
                enumerator.MoveNext(); i++)
                array[arrayIndex + i] = enumerator.Current;
            nestedServices.CopyTo(array, arrayIndex + i);
        }

        bool ICollection<ServiceDescriptor>.Remove(ServiceDescriptor item)
        {
            int index = nestedServices.IndexOf(item);
            if (index < 0)
                return false;
            RemoveAt(index);
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
