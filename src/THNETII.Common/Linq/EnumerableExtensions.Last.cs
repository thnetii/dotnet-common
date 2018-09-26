using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq
{
    public static partial class EnumerableExtensions
    {
        public static T LastOrDefault<T>(this IEnumerable<T> enumerable, T @default) =>
            enumerable.LastOrDefault(() => @default);

        public static T LastOrDefault<T>(this IEnumerable<T> enumerable, Func<T> defaultFactory)
        {
            switch (enumerable)
            {
                case null: throw new ArgumentNullException(nameof(enumerable));
                case T[] array when array.Length < 1: break;
                case T[] array:
                    return array[array.Length - 1];
                case IList<T> list when list.Count < 1: break;
                case IList<T> list:
                    return list[list.Count - 1];
                case IReadOnlyList<T> list when list.Count < 1: break;
                case IReadOnlyList<T> list:
                    return list[list.Count - 1];
                default:
                    using (var enumerator = enumerable.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            T result;
                            do
                            {
                                result = enumerator.Current;
                            } while (enumerator.MoveNext());
                            return result;
                        }
                    }
                    break;
            }
            return defaultFactory.ThrowIfNull(nameof(defaultFactory)).Invoke();
        }
    }
}
