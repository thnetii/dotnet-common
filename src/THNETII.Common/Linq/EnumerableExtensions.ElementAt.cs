using System;
using System.Collections.Generic;

namespace THNETII.Common.Linq
{
    public static partial class EnumerableExtensions
    {
        public static T ElementAtOrDefault<T>(this IEnumerable<T> enumerable, int index, T @default) =>
            enumerable.ElementAtOrDefault(index, () => @default);

        public static T ElementAtOrDefault<T>(this IEnumerable<T> enumerable, int index, Func<T> defaultFactory)
        {
            if (index < 0)
                return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array when index >= array.Length: break;
                case T[] array:
                    return array[index];
                case IList<T> list when index >= list.Count: break;
                case IList<T> list:
                    return list[index];
                case IReadOnlyList<T> list when index >= list.Count: break;
                case IReadOnlyList<T> list:
                    return list[index];
                default:
                    using (var enumerator = enumerable.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            if (index == 0)
                            {
                                return enumerator.Current;
                            }
                            index--;
                        }
                    }
                    break;
            }
            return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
        }
    }
}
