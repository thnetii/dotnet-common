using System;
using System.Collections.Generic;

namespace THNETII.Common.Collections.Generic
{
    /// <summary>
    /// Provides extension methods for converting to or from <see cref="KeyValuePair{TKey, TValue}"/>
    /// structures.
    /// </summary>
    public static class KeyValuePairExtensions
    {
        /// <summary>
        /// Deconstructs a <see cref="KeyValuePair{TKey, TValue}"/> into its individual components.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="kvp">The <see cref="KeyValuePair{TKey, TValue}"/> structure to deconstruct.</param>
        /// <param name="key">Receives the <see cref="KeyValuePair{TKey, TValue}.Key"/> component specified in <paramref name="key"/></param>
        /// <param name="value">Receives the <see cref="KeyValuePair{TKey, TValue}.Value"/> component specified in <paramref name="key"/></param>
        public static void Deconstruct<TKey, TValue>(in this KeyValuePair<TKey, TValue> kvp,
            out TKey key, out TValue value)
            => (key, value) = kvp.AsValueTuple();

        /// <summary>
        /// Copies the specified <see cref="KeyValuePair{TKey, TValue}"/> into a value tuple.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="kvp">The <see cref="KeyValuePair{TKey, TValue}"/> structure to copy.</param>
        /// <returns>
        /// A two-item value tuple with its individual components set to the 
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> and <see cref="KeyValuePair{TKey, TValue}.Value"/>
        /// properties of <paramref name="kvp"/>.
        /// </returns>
        public static (TKey key, TValue value) AsValueTuple<TKey, TValue>(in this KeyValuePair<TKey, TValue> kvp) =>
            (kvp.Key, kvp.Value);

        /// <summary>
        /// Creates a <see cref="KeyValuePair{TKey, TValue}"/> structure from the specified value tuple.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="tuple">The value tuple to convert into a <see cref="KeyValuePair{TKey, TValue}"/> structure.</param>
        /// <returns>
        /// A <see cref="KeyValuePair{TKey, TValue}"/> structure with the key and value set to the respective
        /// components of <paramref name="tuple"/>.
        /// </returns>
        public static KeyValuePair<TKey, TValue> AsKeyValuePair<TKey, TValue>(in this (TKey key, TValue value) tuple) =>
            new KeyValuePair<TKey, TValue>(tuple.key, tuple.value);

        /// <summary>
        /// Converts the specified <see cref="KeyValuePair{TKey, TValue}"/> structure to a new
        /// <see cref="Tuple{T1, T2}"/> instance.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="kvp">The <see cref="KeyValuePair{TKey, TValue}"/> structure to convert.</param>
        /// <returns>
        /// A two-item tuple instance with its individual components set to the 
        /// <see cref="KeyValuePair{TKey, TValue}.Key"/> and <see cref="KeyValuePair{TKey, TValue}.Value"/>
        /// properties of <paramref name="kvp"/>.
        /// </returns>
        public static Tuple<TKey, TValue> ToTuple<TKey, TValue>(in this KeyValuePair<TKey, TValue> kvp) =>
            Tuple.Create(kvp.Key, kvp.Value);

        /// <summary>
        /// Creates a <see cref="KeyValuePair{TKey, TValue}"/> structure from the specified value tuple.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="tuple">The tuple instance to convert into a <see cref="KeyValuePair{TKey, TValue}"/> structure.</param>
        /// <returns>
        /// A <see cref="KeyValuePair{TKey, TValue}"/> structure with the key and value set to the respective
        /// components of <paramref name="tuple"/>, or the default value for the <see cref="KeyValuePair{TKey, TValue}"/>
        /// structure if <paramref name="tuple"/> is <see langword="null"/>.
        /// </returns>
        public static KeyValuePair<TKey, TValue> AsKeyValuePair<TKey, TValue>(this Tuple<TKey, TValue> tuple) =>
            tuple is null ? default : new KeyValuePair<TKey, TValue>(tuple.Item1, tuple.Item2);
    }
}
