using System;
using System.Collections;
using System.Collections.Generic;

namespace THNETII.Common.Collections.Specialized
{
    public sealed class ReadOnlyMemoryDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, ReadOnlyMemoryReference<TValue>>
    {
        private readonly Dictionary<TKey, ReadOnlyMemoryReference<TValue>> references;

        public ReadOnlyMemoryDictionary() : base() =>
            references = new Dictionary<TKey, ReadOnlyMemoryReference<TValue>>();
        public ReadOnlyMemoryDictionary(int capacity) : base() =>
            references = new Dictionary<TKey, ReadOnlyMemoryReference<TValue>>(capacity);
        public ReadOnlyMemoryDictionary(IEqualityComparer<TKey> comparer) : base() =>
            references = new Dictionary<TKey, ReadOnlyMemoryReference<TValue>>(comparer);
        public ReadOnlyMemoryDictionary(int capacity, IEqualityComparer<TKey> comparer) : base() =>
            references = new Dictionary<TKey, ReadOnlyMemoryReference<TValue>>(capacity, comparer);
        public ReadOnlyMemoryDictionary(IDictionary<TKey, ReadOnlyMemoryReference<TValue>> dictionary) : base() =>
            references = new Dictionary<TKey, ReadOnlyMemoryReference<TValue>>(dictionary);
        public ReadOnlyMemoryDictionary(IDictionary<TKey, ReadOnlyMemoryReference<TValue>> dictionary, IEqualityComparer<TKey> comparer) : base() =>
            references = new Dictionary<TKey, ReadOnlyMemoryReference<TValue>>(dictionary, comparer);
        internal ReadOnlyMemoryDictionary(Dictionary<TKey, ReadOnlyMemoryReference<TValue>> dictionary, bool createNew = false) : base() =>
            references = dictionary is null || createNew ? new Dictionary<TKey, ReadOnlyMemoryReference<TValue>>(dictionary) : dictionary;

        private IDictionary<TKey, ReadOnlyMemoryReference<TValue>> ReferencesInterface => references;

        ReadOnlyMemoryReference<TValue> IDictionary<TKey, ReadOnlyMemoryReference<TValue>>.this[TKey key]
        {
            get => references[key];
            set => references[key] = value;
        }

        public ref readonly TValue this[TKey key]
        {
            get
            {
                var reference = references[key];
                return ref reference.Memory.Span[reference.Index];
            }
        }

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] =>
            this[key];

        /// <inheritdoc cref="Dictionary{TKey, TValue}.Keys" />
        public ICollection<TKey> Keys => references.Keys;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        ICollection<ReadOnlyMemoryReference<TValue>> IDictionary<TKey, ReadOnlyMemoryReference<TValue>>.Values =>
            references.Values;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get
            {
                foreach (var reference in references.Values)
                    yield return reference.Memory.Span[reference.Index];
            }
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.Count" />
        public int Count => references.Count;

        bool ICollection<KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>>>.IsReadOnly =>
            ReferencesInterface.IsReadOnly;

        void IDictionary<TKey, ReadOnlyMemoryReference<TValue>>.Add(TKey key, ReadOnlyMemoryReference<TValue> value) =>
            Add(key, value);

        public void Add(TKey key, in ReadOnlyMemoryReference<TValue> value) =>
            references.Add(key, value);

        void ICollection<KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>>>.Add(KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>> item) =>
            Add(item);

        public void Add(in KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>> item) =>
            Add(item.Key, item.Value);

        private delegate void ReferenceUpdater(TKey key, in ReadOnlyMemoryReference<TValue> reference);

        public void AddOrKeepRange(ReadOnlyMemory<TValue> memory, RefReadonlyArgumentFunc<TValue, TKey> keySelector)
        {
            void TryUpdate(TKey key, in ReadOnlyMemoryReference<TValue> reference)
            {
                try { references.Add(key, reference); }
                catch (ArgumentException) when (references.ContainsKey(key)) { }
            }
            AddOrUpdate(memory, keySelector, TryUpdate);
        }

        public void AddOrUpdateRange(ReadOnlyMemory<TValue> memory, RefReadonlyArgumentFunc<TValue, TKey> keySelector)
        {
            void SetItem(TKey key, in ReadOnlyMemoryReference<TValue> reference) =>
                references[key] = reference;
            AddOrUpdate(memory, keySelector, SetItem);
        }

        private static void AddOrUpdate(ReadOnlyMemory<TValue> memory, RefReadonlyArgumentFunc<TValue, TKey> keySelector, ReferenceUpdater updater)
        {
            var span = memory.Span;
            for (int i = 0; i < span.Length; i++)
            {
                var key = keySelector(in span[i]);
                var reference = new ReadOnlyMemoryReference<TValue>(memory, i);
                updater(key, reference);
            }
        }

        /// <inheritdoc cref="Dictionary{TKey, TValue}.Clear" />
        public void Clear() => references.Clear();

        bool ICollection<KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>>>.Contains(KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>> item) =>
            ReferencesInterface.Contains(item);

        /// <inheritdoc cref="Dictionary{TKey, TValue}.ContainsKey" />
        public bool ContainsKey(TKey key) => references.ContainsKey(key);

        void ICollection<KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>>>.CopyTo(KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>>[] array, int arrayIndex) =>
            ReferencesInterface.CopyTo(array, arrayIndex);

        /// <inheritdoc cref="Dictionary{TKey, TValue}.GetEnumerator" />
        public IEnumerator<KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>>> GetEnumerator() =>
            references.GetEnumerator();

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            IEnumerable<KeyValuePair<TKey, TValue>> GetKvpEnumerable()
            {
                foreach (var reference in references)
                    yield return new KeyValuePair<TKey, TValue>(reference.Key, reference.Value.Memory.Span[reference.Value.Index]);
            }
            return GetKvpEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => references.GetEnumerator();

        /// <inheritdoc cref="Dictionary{TKey, TValue}.Remove" />
        public bool Remove(TKey key) => references.Remove(key);

        bool ICollection<KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>>>.Remove(KeyValuePair<TKey, ReadOnlyMemoryReference<TValue>> item) =>
            ReferencesInterface.Remove(item);

        bool IDictionary<TKey, ReadOnlyMemoryReference<TValue>>.TryGetValue(TKey key, out ReadOnlyMemoryReference<TValue> value) =>
            references.TryGetValue(key, out value);

        /// <inheritdoc cref="Dictionary{TKey, TValue}.TryGetValue" />
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!references.TryGetValue(key, out var reference))
            {
                value = default;
                return false;
            }
            value = reference.Memory.Span[reference.Index];
            return true;
        }
    }
}
