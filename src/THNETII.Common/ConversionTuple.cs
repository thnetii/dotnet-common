using System;
using System.Collections.Generic;
using System.Reflection;
using THNETII.Common.Collections.Generic;

namespace THNETII.Common
{
    /// <summary>
    /// A one-way caching conversion tuple that converts a value from a source type into its corresponding destination type.
    /// </summary>
    /// <typeparam name="TRaw">The type of the source value.</typeparam>
    /// <typeparam name="TConvert">The type of the source raw value is converted into.</typeparam>
    public class ConversionTuple<TRaw, TConvert>
    {
        private static readonly Func<TRaw, TRaw, bool> defaultRawEqualityCheckFunction = GetEqualityCheckFunction<TRaw>();

        /// <summary>
        /// Synchronization lock object. All changes to either <see cref="rawValue"/> or <see cref="cachedTuple"/> should be guarded by locking <see cref="sync"/> in order to ensure thread safety.
        /// </summary>
        protected readonly object sync = new object();

        private readonly Func<TRaw, TConvert> rawConvert;
        private readonly Func<TRaw, TRaw, bool> rawEquals;
        /// <summary>
        /// The field storing the current raw source value.
        /// </summary>
        protected TRaw rawValue;
        /// <summary>
        /// The field storing the the tuple containing the source value and the converted value of the last performed conversion.
        /// </summary>
        /// <remarks>If <see cref="Tuple{TRaw, TConvert}.Item1"/> is evaluated as being equal to <see cref="rawValue"/>, <see cref="Tuple{TRaw, TConvert}.Item2"/> contains a valid converted value of the source value.</remarks>
        protected Tuple<TRaw, TConvert> cachedTuple;

        /// <summary>
        /// Gets or sets the source value for the conversion.
        /// </summary>
        /// <remarks>
        /// Both concurrent get and set accesses to <see cref="RawValue"/> can be considered thread-safe.
        /// </remarks>
        public TRaw RawValue
        {
            get => rawValue;
            set { lock (sync) { rawValue = value; } }
        }

        /// <summary>
        /// Gets the Converted value from the current <see cref="RawValue"/>.
        /// </summary>
        /// <value>The result of converting <see cref="RawValue"/> from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>.</value>
        /// <remarks>
        /// <para>Concurrent accesses to <see cref="ConvertedValue"/> can be considered thread-safe.</para>
        /// <para>The conversion from <see cref="RawValue"/> is defined by the argument to the constructor of the <see cref="ConversionTuple{TRaw, TConvert}"/> class.</para>
        /// <para>The <see cref="ConversionTuple{TRaw, TConvert}"/> class caches the result of the conversion. The conversion function is only called if <see cref="RawValue"/> has changed since <see cref="ConvertedValue"/> was last accessed.</para>
        /// <para>If the value of <see cref="RawValue"/> is not changed, subsequent accesses to <see cref="ConvertedValue"/> will return the exact same value or instance of <typeparamref name="TConvert"/>.</para>
        /// </remarks>
        public TConvert ConvertedValue
        {
            get
            {
                TRaw localRaw;
                Tuple<TRaw, TConvert> localCached;
                lock (sync)
                {
                    localRaw = rawValue;
                    localCached = cachedTuple;
                }

                if (localCached == null || !rawEquals(localRaw, localCached.Item1))
                {
                    localCached = Tuple.Create(localRaw, rawConvert(localRaw));
                    lock (sync)
                    {
                        if (rawEquals(rawValue, localCached.Item1))
                            cachedTuple = localCached;
                    }
                }

                return localCached.Item2;
            }
        }

        /// <summary>
        /// Clears the cache of the last performed conversion, forcing the next access to <see cref="ConvertedValue"/> to perform a conversion of <see cref="RawValue"/> to <typeparamref name="TConvert"/>.
        /// </summary>
        public void ClearCache() => cachedTuple = null;

        /// <summary>
        /// Returns the equality check function to use to check for equality between two values of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to check equality for. Typically this will be either <typeparamref name="TRaw"/> or <typeparamref name="TConvert"/>.</typeparam>
        /// <returns>The default comparison function for <typeparamref name="T"/> (as specified by <see cref="EqualityComparer{T}.Equals(T, T)"/>), or the reference equality function (as specified by <see cref="object.ReferenceEquals(object, object)"/> for reference types.</returns>
        protected static Func<T, T, bool> GetEqualityCheckFunction<T>()
        {
            if (typeof(T).GetTypeInfo().IsValueType)
                return EqualityComparer<T>.Default.Equals;
            else
                return ReferenceEqualityComparer<T>.StaticEquals;
        }

        /// <summary>
        /// Creates a new conversion tuple with the specified conversion function.
        /// <para>Optionally, an EqualityComparer can be specified that is used to determine whether the raw value of the conversion has changed.</para>
        /// </summary>
        /// <param name="rawConvert">The conversion function to use, to convert values from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawEqualityComparer">An optional equality comparer, to check whether the cached value of <see cref="RawValue"/> has been changed. Omit or specify <c>null</c> to use default equality checks.</param>
        /// <remarks>
        /// The conversion function <paramref name="rawConvert"/> is only when accessing <see cref="ConvertedValue"/> and then only if the value of <see cref="RawValue"/> has changed since the last access to <see cref="ConvertedValue"/>.
        /// <para>To determine whether <see cref="RawValue"/> has changed, the <see cref="ConversionTuple{TRaw, TConvert}"/> class caches the <typeparamref name="TRaw"/> value that was used in the last conversion and compares it for equality against the current value of <see cref="RawValue"/>.</para>
        /// <para>If no custom <see cref="IEqualityComparer{TRaw}"/> is specified in <paramref name="rawEqualityComparer"/> or if <paramref name="rawEqualityComparer"/> is <c>null</c>, a reference equality check (<see cref="object.ReferenceEquals(object, object)"/> is used if <typeparamref name="TRaw"/> is a reference type. If <typeparamref name="TRaw"/> is a value type, <see cref="EqualityComparer{TRaw}.Default"/> is used as the equality comparer.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="rawConvert"/> is <c>null</c>.</exception>
        public ConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw> rawEqualityComparer = null)
            : this(rawConvert, (rawEqualityComparer != null ? rawEqualityComparer.Equals : defaultRawEqualityCheckFunction)) { }

        /// <summary>
        /// Creates a new conversion tuple with the specified conversion and equality functions.
        /// </summary>
        /// <param name="rawConvert">The conversion function to use, to convert values from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawEquals">An equality check function that determines equality between two values or instances of the <typeparamref name="TRaw"/> type. Must not be <c>null</c>.</param>
        /// <remarks>
        /// The conversion function <paramref name="rawConvert"/> is only when accessing <see cref="ConvertedValue"/> and then only if the value of <see cref="RawValue"/> has changed since the last access to <see cref="ConvertedValue"/>.
        /// <para>To determine whether <see cref="RawValue"/> has changed, the <see cref="ConversionTuple{TRaw, TConvert}"/> class caches the <typeparamref name="TRaw"/> value that was used in the last conversion and compares it for equality against the current value of <see cref="RawValue"/>.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="rawConvert"/> or <paramref name="rawEquals"/> is <c>null</c>.</exception>
        public ConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals)
        {
            this.rawConvert = rawConvert ?? throw new ArgumentNullException(nameof(rawConvert));
            this.rawEquals = rawEquals ?? throw new ArgumentNullException(nameof(rawEquals));
        }
    }
}
