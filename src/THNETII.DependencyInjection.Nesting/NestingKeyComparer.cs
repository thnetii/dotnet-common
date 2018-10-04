using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace THNETII.DependencyInjection.Nesting
{
    internal static class NestingKeyComparer<TKey>
    {
        private static readonly bool KeyIsStringType =
              typeof(TKey) == typeof(string);

        public static IEqualityComparer<TKey> GetNestingKeyComparer(
            IServiceProvider rootServiceProvider)
        {
            var keyComparer = rootServiceProvider?
                .GetService<IEqualityComparer<TKey>>();
            if (!(keyComparer is null))
                return keyComparer;
            else if (KeyIsStringType)
            {
                keyComparer = StringComparer.OrdinalIgnoreCase
                    as IEqualityComparer<TKey>;
            }
            return keyComparer ?? EqualityComparer<TKey>.Default;
        }
    }
}
