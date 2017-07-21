using System;
using System.Collections.Generic;

namespace THNETII.Common
{
    /// <summary>
    /// A two-way caching conversion tuple that converts values between the <typeparamref name="TRaw"/> and <typeparamref name="TConvert"/> types.
    /// </summary>
    /// <typeparam name="TRaw">The type of the source value.</typeparam>
    /// <typeparam name="TConvert">The type of the converted value.</typeparam>
    public class DuplexConversionTuple<TRaw, TConvert> : ConversionTuple<TRaw, TConvert>
    {
        private static readonly Func<TConvert, TConvert, bool> defaultConvertEqualityCheckFunction = GetEqualityCheckFunction<TConvert>();

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
                var localCached = cachedTuple;
                TRaw localRaw;
                if (localCached != null && convertedEquals(localCached.Item2, value))
                    localRaw = localCached.Item1;
                else
                {
                    localRaw = rawReverseConvert(value);
                    localCached = Tuple.Create(localRaw, value);
                }
                lock (sync)
                {
                    rawValue = localRaw;
                    cachedTuple = localCached;
                }
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
            : this(rawConvert, rawEqualityComparer, rawReverseConvert, convertedEqualityComparer != null ? convertedEqualityComparer.Equals : defaultConvertEqualityCheckFunction) { }

        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals, Func<TConvert, TRaw> rawReverseConvert, IEqualityComparer<TConvert> convertedEqualityComparer)
            : this(rawConvert, rawEquals, rawReverseConvert, convertedEqualityComparer != null ? convertedEqualityComparer.Equals : defaultConvertEqualityCheckFunction) { }

        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw> rawEqualityComparer,
            Func<TConvert, TRaw> rawReverseConvert, Func<TConvert, TConvert, bool> convertedEquals)
            : base(rawConvert, rawEqualityComparer)
        {
            this.rawReverseConvert = rawReverseConvert ?? throw new ArgumentNullException(nameof(rawReverseConvert));
            this.convertedEquals = convertedEquals ?? throw new ArgumentNullException(nameof(convertedEquals));
        }

        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals,
            Func<TConvert, TRaw> rawReverseConvert, Func<TConvert, TConvert, bool> convertedEquals)
            : base(rawConvert, rawEquals)
        {
            this.rawReverseConvert = rawReverseConvert ?? throw new ArgumentNullException(nameof(rawReverseConvert));
            this.convertedEquals = convertedEquals ?? throw new ArgumentNullException(nameof(convertedEquals));
        }
    }
}
