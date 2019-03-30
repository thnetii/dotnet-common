// Adapted source code for compatability with netstandard1.3
// Copied from
// - https://github.com/aspnet/Extensions/blob/7afd61be3be198ee07720383c27425d70b955866/src/ObjectPool/src/DefaultObjectPool.cs
// - https://github.com/aspnet/Extensions/blob/7afd61be3be198ee07720383c27425d70b955866/src/ObjectPool/src/DefaultPooledObjectPolicy.cs
// - https://github.com/aspnet/Extensions/blob/7afd61be3be198ee07720383c27425d70b955866/src/ObjectPool/src/IPooledObjectPolicy.cs
// - https://github.com/aspnet/Extensions/blob/7afd61be3be198ee07720383c27425d70b955866/src/ObjectPool/src/ObjectPool.cs
// - https://github.com/aspnet/Extensions/blob/7afd61be3be198ee07720383c27425d70b955866/src/ObjectPool/src/PooledObjectPolicy.cs

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See https://github.com/aspnet/Extensions/blob/7afd61be3be198ee07720383c27425d70b955866/LICENSE.txt for license information of the original source code.

#if NETSTANDARD1_3
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Microsoft.Extensions.ObjectPool
{
    internal abstract class ObjectPool<T> where T : class
    {
        public abstract T Get();
        public abstract void Return(T obj);
    }

    internal class DefaultObjectPool<T> : ObjectPool<T> where T : class
    {
        private protected readonly ObjectWrapper[] _items;
        private protected readonly IPooledObjectPolicy<T> _policy;
        private protected readonly bool _isDefaultPolicy;
        private protected T _firstItem;

        // This class was introduced in .NET Core 2.1 to avoid the interface call where possible
        private protected readonly PooledObjectPolicy<T> _fastPolicy;

        public DefaultObjectPool(IPooledObjectPolicy<T> policy)
            : this(policy, Environment.ProcessorCount * 2)
        {
        }

        public DefaultObjectPool(IPooledObjectPolicy<T> policy, int maximumRetained)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            _fastPolicy = policy as PooledObjectPolicy<T>;
            _isDefaultPolicy = IsDefaultPolicy();

            // -1 due to _firstItem
            _items = new ObjectWrapper[maximumRetained - 1];

            bool IsDefaultPolicy()
            {
                var type = policy.GetType();

                return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(DefaultPooledObjectPolicy<>);
            }
        }

        public override T Get()
        {
            var item = _firstItem;
            if (item is null || !ReferenceEquals(Interlocked.CompareExchange(ref _firstItem, null, item), item))
            {
                var items = _items;
                for (var i = 0; i < items.Length; i++)
                {
                    item = items[i].Element;
                    if (item is T && ReferenceEquals(Interlocked.CompareExchange(ref items[i].Element, null, item), item))
                    {
                        return item;
                    }
                }

                item = Create();
            }

            return item;
        }

        // Non-inline to improve its code quality as uncommon path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private T Create() => _fastPolicy?.Create() ?? _policy.Create();

        public override void Return(T obj)
        {
            if (_isDefaultPolicy || (_fastPolicy?.Return(obj) ?? _policy.Return(obj)))
            {
                if (_firstItem is T || Interlocked.CompareExchange(ref _firstItem, obj, null) is T)
                {
                    var items = _items;
                    for (var i = 0; i < items.Length && Interlocked.CompareExchange(ref items[i].Element, obj, null) is T; ++i)
                    {
                    }
                }
            }
        }

        // PERF: the struct wrapper avoids array-covariance-checks from the runtime when assigning to elements of the array.
        [DebuggerDisplay("{Element}")]
        private protected struct ObjectWrapper
        {
            public T Element;
        }
    }

    internal interface IPooledObjectPolicy<T>
    {
        T Create();

        bool Return(T obj);
    }

    internal abstract class PooledObjectPolicy<T> : IPooledObjectPolicy<T>
    {
        public abstract T Create();

        public abstract bool Return(T obj);
    }

    internal class DefaultPooledObjectPolicy<T> : PooledObjectPolicy<T> where T : class, new()
    {
        public override T Create()
        {
            return new T();
        }

        // DefaultObjectPool<T> doesn't call 'Return' for the default policy.
        // So take care adding any logic to this method, as it might require changes elsewhere.
        public override bool Return(T obj)
        {
            return true;
        }
    }
}
#endif
