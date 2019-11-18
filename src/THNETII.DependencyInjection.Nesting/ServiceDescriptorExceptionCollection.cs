using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.DependencyInjection.Nesting
{
    internal class ServiceDescriptorExceptionCollection : ICollection<ServiceDescriptor>
    {
        private readonly IDictionary<Type, LinkedList<ServiceDescriptor>> serviceDescriptors =
            new Dictionary<Type, LinkedList<ServiceDescriptor>>();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerable<ServiceDescriptor> GetEnumerable()
            => serviceDescriptors.Values.SelectMany(l => l);

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => serviceDescriptors.Values.Sum(l => l.Count);

        /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
        public bool IsReadOnly => serviceDescriptors.IsReadOnly;

        /// <inheritdoc cref="ICollection{T}.Add(T)"/>
        public void Add(ServiceDescriptor item)
        {
            if (!serviceDescriptors.TryGetValue(item.ServiceType, out var list))
            {
                list = new LinkedList<ServiceDescriptor>();
                serviceDescriptors[item.ServiceType] = list;
            }
            list.AddLast(item);
        }

        /// <inheritdoc cref="ICollection{T}.Clear"/>
        public void Clear()
        {
            foreach (var list in serviceDescriptors.Values)
                list.Clear();
            serviceDescriptors.Clear();
        }

        /// <inheritdoc cref="ICollection{T}.Contains(T)"/>
        public bool Contains(ServiceDescriptor item)
        {
            if (serviceDescriptors.TryGetValue(item.ServiceType, out var list))
                return list.Contains(item);
            return false;
        }

        /// <inheritdoc cref="Contains(ServiceDescriptor)"/>
        public bool Contains(ServiceDescriptor item,
            IEqualityComparer<ServiceDescriptor> comparer)
        {
            if (serviceDescriptors.TryGetValue(item.ServiceType, out var list))
                return list.Contains(item, comparer);
            return false;
        }

        /// <inheritdoc cref="ICollection{T}.CopyTo(T[], int)"/>
        public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
        {
            foreach (var list in serviceDescriptors.Values)
            {
                list.CopyTo(array, arrayIndex);
                arrayIndex += list.Count;
            }
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        public IEnumerator<ServiceDescriptor> GetEnumerator()
            => GetEnumerable().GetEnumerator();

        private int IndexOf(ServiceDescriptor item,
            Func<ServiceDescriptor, ServiceDescriptor, bool> predicate)
        {
            int preIdx = 0;
            if (!serviceDescriptors.ContainsKey(item.ServiceType))
                return -1;
            foreach (var kvp in serviceDescriptors)
            {
                if (kvp.Key != item.ServiceType)
                    preIdx += kvp.Value.Count;
                else
                {
                    var matchTuple = kvp.Value.Select((d, idx) => (d, idx))
                        .FirstOrDefault(t => predicate(t.d, item));
                    if (matchTuple.d is null)
                        return -1;
                    return preIdx + matchTuple.idx;

                }
            }
            return -1;
        }

        /// <inheritdoc cref="IList{T}.IndexOf(T)"/>
        public int IndexOf(ServiceDescriptor item) => IndexOf(item, (x, y) => x == y);

        /// <inheritdoc cref="Contains(ServiceDescriptor)"/>
        public int IndexOf(ServiceDescriptor item,
            IEqualityComparer<ServiceDescriptor> comparer)
            => IndexOf(item, (x, y) => comparer.Equals(x, y));

        /// <inheritdoc cref="ICollection{T}.Remove(T)"/>
        public bool Remove(ServiceDescriptor item)
        {
            if (serviceDescriptors.TryGetValue(item.ServiceType, out var list))
                return list.Remove(item);
            return false;
        }

        /// <inheritdoc cref="Remove(ServiceDescriptor)"/>
        public bool Remove(ServiceDescriptor item,
            IEqualityComparer<ServiceDescriptor> comparer)
        {
            if (serviceDescriptors.TryGetValue(item.ServiceType, out var list))
            {
                for (var nd = list.First; !(nd is null); nd = nd.Next)
                {
                    if (comparer.Equals(item, nd.Value))
                    {
                        list.Remove(nd);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal static class ServiceDescriptorExceptionCollectionExtensions
    {
        public static IEnumerable<ServiceDescriptor> Except(
            this IEnumerable<ServiceDescriptor> serviceDescriptors,
            ServiceDescriptorExceptionCollection exceptionList)
        {
            return serviceDescriptors.Where(d => !exceptionList.Contains(d));
        }

        public static IEnumerable<ServiceDescriptor> Except(
            this IEnumerable<ServiceDescriptor> serviceDescriptors,
            ServiceDescriptorExceptionCollection exceptionList,
            IEqualityComparer<ServiceDescriptor> equalityComparer)
        {
            return serviceDescriptors.Where(d => !exceptionList.Contains(d, equalityComparer));
        }
    }
}
