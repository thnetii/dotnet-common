using System;
using System.Collections.Generic;
using System.Threading;

namespace THNETII.Common
{
    /// <summary>
    /// A two-way caching conversion tuple that converts values between the <typeparamref name="TRaw"/> and <typeparamref name="TConvert"/> types.
    /// </summary>
    /// <typeparam name="TRaw">The type of the source value.</typeparam>
    /// <typeparam name="TConvert">The type of the converted value.</typeparam>
    public class DuplexConversionTuple<TRaw, TConvert> : ConversionTuple<TRaw, TConvert>, IEquatable<DuplexConversionTuple<TRaw, TConvert>>
    {
        private readonly Func<TConvert, TConvert, bool> convertedEquals;
        private readonly Func<TConvert, TRaw> rawReverseConvert;

        /// <summary>
        /// Gets or sets the Converted value that represents the conversion from <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> to the <typeparamref name="TConvert"/> type.
        /// </summary>
        /// <value>The result of converting <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> from the <typeparamref name="TRaw"/> type to the <typeparamref name="TConvert"/> type.</value>
        /// <remarks>
        /// <para>Both concurrent set and get accesses to <see cref="ConvertedValue"/> can be considered thread-safe.</para>
        /// <para>The conversion from <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> is defined by the argument to the constructor of the <see cref="DuplexConversionTuple{TRaw, TConvert}"/> class.</para>
        /// <para>The conversion to <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> is defined by the argument to the constructor of the <see cref="DuplexConversionTuple{TRaw, TConvert}"/> class.</para>
        /// <para>The <see cref="DuplexConversionTuple{TRaw, TConvert}"/> class caches the result of the conversion. The conversion function is only called if <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> has changed since <see cref="ConvertedValue"/> was last accessed.</para>
        /// <para>If neither the value of <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> nor <see cref="ConvertedValue"/> are changed, subsequent get accesses to <see cref="ConvertedValue"/> will return the exact same value or instance of <typeparamref name="TConvert"/>.</para>
        /// <para>If the value of <see cref="ConvertedValue"/> is changed by setting a new value, it is immediately converted to <typeparamref name="TRaw"/> and <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> is set to the result of the conversion.</para>
        /// </remarks>
        new public TConvert ConvertedValue
        {
            get => base.ConvertedValue;
            set
            {
                var localCache = cache;
                TRaw localRaw;
                if (cacheInitalized && convertedEquals(localCache.converted, value))
                    localRaw = localCache.raw;
                else
                {
                    localRaw = rawReverseConvert(value);
                    localCache = (localRaw, value);
                }
                while (Interlocked.Exchange(ref sync, 1) != 0) ;
                rawValue = localRaw;
                cache = localCache;
                sync = 0;
                cacheInitalized = true;
            }
        }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directions.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/> or <paramref name="rawReverseConvert"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TConvert, TRaw> rawReverseConvert)
            : this(rawConvert, (IEqualityComparer<TRaw>)null, rawReverseConvert) { }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directions. Uses the specified equality comparer to detect changes for the raw value.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawEqualityComparer">An optional equality comparer, to check whether the cached value of <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> has been changed. Specify <c>null</c> to use default equality checks.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/> or <paramref name="rawReverseConvert"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw> rawEqualityComparer, Func<TConvert, TRaw> rawReverseConvert)
            : this(rawConvert, rawEqualityComparer, rawReverseConvert, (IEqualityComparer<TConvert>)null) { }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directrions. Uses the specified comparison function to detect changes for the raw value.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawEquals">The equality comparison function to use to check whether the cached value of <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> has been changed. Must not be <c>null</c>.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/>, <paramref name="rawEquals"/> or <paramref name="rawReverseConvert"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals, Func<TConvert, TRaw> rawReverseConvert)
            : this(rawConvert, rawEquals, rawReverseConvert, (IEqualityComparer<TConvert>)null) { }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directions. Uses the specified equality comparer to detect changes for the converted value.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <param name="convertedEqualityComparer">An optional equality comparer, to check whether the cached value of <see cref="ConvertedValue"/> has been changed. Specify <c>null</c> to use default equality checks.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/> or <paramref name="rawReverseConvert"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TConvert, TRaw> rawReverseConvert, IEqualityComparer<TConvert> convertedEqualityComparer)
            : this(rawConvert, (IEqualityComparer<TRaw>)null, rawReverseConvert, convertedEqualityComparer) { }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directions. Uses the specified equality comparison function to detect changes for the converted value.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <param name="convertedEquals">The equality comparison function to use to check whether the cached value of <see cref="ConvertedValue"/> has been changed. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/>, <paramref name="rawReverseConvert"/> or <paramref name="convertedEquals"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TConvert, TRaw> rawReverseConvert, Func<TConvert, TConvert, bool> convertedEquals)
            : this(rawConvert, (IEqualityComparer<TRaw>)null, rawReverseConvert, convertedEquals) { }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directions. Uses the specified equality comparers to detect changes for the raw and converted values.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawEqualityComparer">An optional equality comparer, to check whether the cached value of <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> has been changed. Specify <c>null</c> to use default equality checks.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <param name="convertedEqualityComparer">An optional equality comparer, to check whether the cached value of <see cref="ConvertedValue"/> has been changed. Specify <c>null</c> to use default equality checks.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/> or <paramref name="rawReverseConvert"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw> rawEqualityComparer, Func<TConvert, TRaw> rawReverseConvert, IEqualityComparer<TConvert> convertedEqualityComparer)
            : this(rawConvert, rawEqualityComparer, rawReverseConvert, convertedEqualityComparer != null ? convertedEqualityComparer.Equals : GetEqualityCheckFunction<TConvert>()) { }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directions.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawEquals">The equality comparison function to use to check whether the cached value of <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> has been changed. Must not be <c>null</c>.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <param name="convertedEqualityComparer">An optional equality comparer, to check whether the cached value of <see cref="ConvertedValue"/> has been changed. Specify <c>null</c> to use default equality checks.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/>, <paramref name="rawEquals"/> or <paramref name="rawReverseConvert"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals, Func<TConvert, TRaw> rawReverseConvert, IEqualityComparer<TConvert> convertedEqualityComparer)
            : this(rawConvert, rawEquals, rawReverseConvert, convertedEqualityComparer != null ? convertedEqualityComparer.Equals : GetEqualityCheckFunction<TConvert>()) { }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directions.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawEqualityComparer">An optional equality comparer, to check whether the cached value of <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> has been changed. Specify <c>null</c> to use default equality checks.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <param name="convertedEquals">The equality comparison function to use to check whether the cached value of <see cref="ConvertedValue"/> has been changed. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/>, <paramref name="rawReverseConvert"/> or <paramref name="convertedEquals"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw> rawEqualityComparer,
            Func<TConvert, TRaw> rawReverseConvert, Func<TConvert, TConvert, bool> convertedEquals)
            : base(rawConvert, rawEqualityComparer)
        {
            this.rawReverseConvert = rawReverseConvert ?? throw new ArgumentNullException(nameof(rawReverseConvert));
            this.convertedEquals = convertedEquals ?? throw new ArgumentNullException(nameof(convertedEquals));
        }

        /// <summary>
        /// Creates a new duplex conversion tuple with the specified conversion functions for both directions.
        /// </summary>
        /// <param name="rawConvert">The function to call to convert a value from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <c>null</c>.</param>
        /// <param name="rawEquals">The equality comparison function to use to check whether the cached value of <see cref="ConversionTuple{TRaw, TConvert}.RawValue"/> has been changed. Must not be <c>null</c>.</param>
        /// <param name="rawReverseConvert">The function to call to convert a value from <typeparamref name="TConvert"/> to <typeparamref name="TRaw"/>. Must not be <c>null</c>.</param>
        /// <param name="convertedEquals">The equality comparison function to use to check whether the cached value of <see cref="ConvertedValue"/> has been changed. Must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">Either <paramref name="rawConvert"/>, <paramref name="rawEquals"/>, <paramref name="rawReverseConvert"/> or <paramref name="convertedEquals"/> are <c>null</c>.</exception>
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals,
            Func<TConvert, TRaw> rawReverseConvert, Func<TConvert, TConvert, bool> convertedEquals)
            : base(rawConvert, rawEquals)
        {
            this.rawReverseConvert = rawReverseConvert ?? throw new ArgumentNullException(nameof(rawReverseConvert));
            this.convertedEquals = convertedEquals ?? throw new ArgumentNullException(nameof(convertedEquals));
        }

        /// <inheritdoc />
        public bool Equals(DuplexConversionTuple<TRaw, TConvert> other) =>
            base.Equals(other);

        /// <inheritdoc />
        public override bool Equals(object obj) => base.Equals(obj);

        /// <inheritdoc />
        public override int GetHashCode() => base.GetHashCode();
    }
}
