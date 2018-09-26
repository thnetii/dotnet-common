using System;
using System.Collections.Generic;

namespace THNETII.Common.Linq
{
    public static partial class EnumerableExtensions
    {
        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, T @default) =>
            enumerable.ElementAtOrDefault(0, @default);

        public static T FirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T> defaultFactory) =>
            enumerable.ElementAtOrDefault(0, defaultFactory);
    }
}
