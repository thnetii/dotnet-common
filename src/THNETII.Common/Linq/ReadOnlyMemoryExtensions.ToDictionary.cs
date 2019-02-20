using System;
using System.Collections.Generic;
using System.Linq;
using THNETII.Common.Collections.Specialized;

namespace THNETII.Common.Linq
{
    public static partial class ReadOnlyMemoryExtensions
    {
        /// <seealso cref="Enumerable.ToDictionary{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey})"/>
        public static ReadOnlyMemoryDictionary<TKey, TSource> ToDictionary<TKey, TSource>(
            this ReadOnlyMemory<TSource> source, RefReadonlyArgumentFunc<TSource, TKey> keySelector)
        {
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));
            var references = Enumerable.Range(0, source.Length).ToDictionary(
                i => keySelector(source.Span[i]),
                i => new ReadOnlyMemoryReference<TSource>(source, i));
            return new ReadOnlyMemoryDictionary<TKey, TSource>(references);
        }

        /// <seealso cref="Enumerable.ToDictionary{TSource, TKey}(IEnumerable{TSource}, Func{TSource, TKey}, IEqualityComparer{TKey})"/>
        public static ReadOnlyMemoryDictionary<TKey, TSource> ToDictionary<TKey, TSource>(
            this ReadOnlyMemory<TSource> source, RefReadonlyArgumentFunc<TSource, TKey> keySelector,
            IEqualityComparer<TKey> keyComparer)
        {
            if (keySelector is null)
                throw new ArgumentNullException(nameof(keySelector));
            var references = Enumerable.Range(0, source.Length).ToDictionary(
                i => keySelector(in source.Span[i]),
                i => new ReadOnlyMemoryReference<TSource>(source, i),
                keyComparer);
            return new ReadOnlyMemoryDictionary<TKey, TSource>(references);
        }
    }
}
