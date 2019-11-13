using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using THNETII.Common.Collections.Generic;

namespace THNETII.Common
{
    /// <summary>
    /// A one-way caching conversion tuple that converts a value from a source type into its corresponding destination type.
    /// </summary>
    /// <typeparam name="TRaw">The type of the source value.</typeparam>
    /// <typeparam name="TConvert">The type of the source raw value is converted into.</typeparam>
    public class ConversionTuple<TRaw, TConvert> : IEquatable<ConversionTuple<TRaw, TConvert>>
    {
        /// <summary>
        /// Synchronization lock object. All changes to either <see cref="rawValue"/> or <see cref="cache"/> should be guarded by locking <see cref="sync"/> in order to ensure thread safety.
        /// </summary>
        /// <value>A spin-lock supporting integer value. <c>0</c> (zero) if not contended; non-zero otherwise.</value>
        [SuppressMessage(null, "CA1051", Justification = "Member only visible internally.")]
        internal protected int sync;

        /// <summary>
        /// Flag that signals whether the local cache of the tuple has been previously
        /// written to during the Tuple's lifetime.
        /// </summary>
        /// <value><see langword="true"/> if <see cref="cache"/> has been initialized; otherwise, <see langword="false"/>.</value>
        /// <remarks>
        /// This flag prevents conversion errors for intial default-value assignments.
        /// When the <see cref="ConversionTuple{TRaw, TConvert}"/> instance is first created
        /// the local cache is populated with the default values for <typeparamref name="TRaw"/> and <typeparamref name="TConvert"/>.
        /// Depending on the conversion function the default value may not be the correct desired
        /// value to be stored in the cache.
        /// </remarks>
        [SuppressMessage(null, "CA1051", Justification = "Member only visible internally.")]
        internal protected bool cacheInitalized = false;

        private readonly Func<TRaw, TConvert> rawConvert;
        private readonly Func<TRaw, TRaw, bool> rawEquals;

        /// <summary>
        /// The field storing the current raw source value.
        /// </summary>
        /// <value>The <typeparamref name="TRaw"/> value containg the value exposed by the <see cref="RawValue"/> property.</value>
        [SuppressMessage(null, "CA1051", Justification = "Member only visible internally.")]
        internal protected TRaw rawValue = default!;

        /// <summary>
        /// The field storing the the tuple containing the source value and the converted value of the last performed conversion.
        /// </summary>
        /// <value>A tuple containing the cached raw and corresponding converted value.</value>
        /// <remarks>If <see cref="Tuple{TRaw, TConvert}.Item1"/> is evaluated as being equal to <see cref="rawValue"/>, <see cref="Tuple{TRaw, TConvert}.Item2"/> contains a valid converted value of the source value.</remarks>
        [SuppressMessage(null, "CA1051", Justification = "Member only visible internally.")]
        internal protected (TRaw raw, TConvert converted) cache;

        /// <summary>
        /// Gets or sets the source value for the conversion.
        /// </summary>
        /// <value>The original source value of type <typeparamref name="TRaw"/> that is convertible to a <typeparamref name="TConvert"/> value when accessing <see cref="ConvertedValue"/>.</value>
        /// <remarks>
        /// Both concurrent get and set accesses to <see cref="RawValue"/> can be considered thread-safe.
        /// </remarks>
        public TRaw RawValue
        {
            get => rawValue;
            set
            {
                while (Interlocked.Exchange(ref sync, 1) != 0) ;
                rawValue = value;
                sync = 0;
            }
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
                while (Interlocked.Exchange(ref sync, 1) != 0) ;
                var localRaw = rawValue;
                var localCache = cache;
                sync = 0;

                if (!cacheInitalized || !rawEquals(localRaw, localCache.raw))
                {
                    localCache = (localRaw, rawConvert(localRaw));
                    while (Interlocked.Exchange(ref sync, 1) != 0) ;
                    if (rawEquals(rawValue, localCache.raw))
                        cache = localCache;
                    sync = 0;
                }

                cacheInitalized = true;
                return localCache.converted;
            }
        }

        /// <summary>
        /// Clears the cache of the last performed conversion, forcing the next access to <see cref="ConvertedValue"/> to perform a conversion of <see cref="RawValue"/> to <typeparamref name="TConvert"/>.
        /// </summary>
        public void ClearCache()
        {
            while (Interlocked.Exchange(ref sync, 1) != 0) ;
            cacheInitalized = false;
            cache = default;
            sync = 0;
        }

        /// <summary>
        /// Returns the equality check function to use to check for equality between two values of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to check equality for. Both reference and value types are allowed.</typeparam>
        /// <returns>The default comparison function for <typeparamref name="T"/> (as specified by <see cref="EqualityComparer{T}.Equals(T, T)"/>), or the reference equality function (as specified by <see cref="object.ReferenceEquals(object, object)"/> for reference types.</returns>
        internal protected static Func<T, T, bool> GetEqualityCheckFunction<T>()
        {
            var typeInfo = typeof(T)
#if NETSTANDARD1_3
                .GetTypeInfo()
#endif
                ;
            if (typeInfo.IsValueType)
                return EqualityComparer<T>.Default.Equals;
            else
                return (T x, T y) => ReferenceEquals(x, y);
        }

        /// <summary>
        /// Creates a new conversion tuple with the specified conversion function.
        /// <para>Optionally, an EqualityComparer can be specified that is used to determine whether the raw value of the conversion has changed.</para>
        /// </summary>
        /// <param name="rawConvert">The conversion function to use, to convert values from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <see langword="null"/>.</param>
        /// <param name="rawEqualityComparer">An optional equality comparer, to check whether the cached value of <see cref="RawValue"/> has been changed. Omit or specify <see langword="null"/> to use default equality checks.</param>
        /// <remarks>
        /// The conversion function <paramref name="rawConvert"/> is only invoked when accessing <see cref="ConvertedValue"/> and then only if the value of <see cref="RawValue"/> has changed since the last access to <see cref="ConvertedValue"/>.
        /// <para>To determine whether <see cref="RawValue"/> has changed, the <see cref="ConversionTuple{TRaw, TConvert}"/> class caches the <typeparamref name="TRaw"/> value that was used in the last conversion and compares it for equality against the current value of <see cref="RawValue"/>.</para>
        /// <para>If no custom <see cref="IEqualityComparer{TRaw}"/> is specified in <paramref name="rawEqualityComparer"/> or if <paramref name="rawEqualityComparer"/> is <see langword="null"/>, a reference equality check (<see cref="object.ReferenceEquals(object, object)"/> is used if <typeparamref name="TRaw"/> is a reference type. If <typeparamref name="TRaw"/> is a value type, <see cref="EqualityComparer{TRaw}.Default"/> is used as the equality comparer.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="rawConvert"/> is <see langword="null"/>.</exception>
        public ConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw>? rawEqualityComparer = null)
            : this(rawConvert, (!(rawEqualityComparer is null) ? rawEqualityComparer.Equals : GetEqualityCheckFunction<TRaw>())) { }

        /// <summary>
        /// Creates a new conversion tuple with the specified conversion and equality functions.
        /// </summary>
        /// <param name="rawConvert">The conversion function to use, to convert values from <typeparamref name="TRaw"/> to <typeparamref name="TConvert"/>. Must not be <see langword="null"/>.</param>
        /// <param name="rawEquals">An equality check function that determines equality between two values or instances of the <typeparamref name="TRaw"/> type. Must not be <see langword="null"/>.</param>
        /// <remarks>
        /// The conversion function <paramref name="rawConvert"/> is only invoked when accessing <see cref="ConvertedValue"/> and then only if the value of <see cref="RawValue"/> has changed since the last access to <see cref="ConvertedValue"/>.
        /// <para>To determine whether <see cref="RawValue"/> has changed, the <see cref="ConversionTuple{TRaw, TConvert}"/> class caches the <typeparamref name="TRaw"/> value that was used in the last conversion and compares it for equality against the current value of <see cref="RawValue"/>.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="rawConvert"/> or <paramref name="rawEquals"/> is <see langword="null"/>.</exception>
        public ConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals)
        {
            this.rawConvert = rawConvert ?? throw new ArgumentNullException(nameof(rawConvert));
            this.rawEquals = rawEquals ?? throw new ArgumentNullException(nameof(rawEquals));
        }

        /// <summary>
        /// Determines whether the current conversion tuple is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if this instance logically refers to the same conversion tuple as <paramref name="obj"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// Equality between to conversion tuple instances is determined by reference equality or by satifying all of the following characteristics:
        /// <list type="number">
        /// <item><term>Reference equality of the raw comparsion function.</term></item>
        /// <item><term>Reference equality of the raw conversion function.</term></item>
        /// <item><term>Applying the raw equality function to <see cref="rawValue"/> of both instances returns <see langword="true"/>.</term></item>
        /// </list>
        /// </remarks>
        public override bool Equals(object obj)
        {
            if (obj is ConversionTuple<TRaw, TConvert> other)
                return this == other;
            return false;
        }

        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// <returns>The hash code obtained by invoking <see cref="object.GetHashCode"/> on <see cref="rawValue"/>, or <c>0</c> (zero) if <see cref="rawValue"/> is <see langword="null"/>.</returns>
        public override int GetHashCode() => rawValue?.GetHashCode() ?? default;

        /// <summary>
        /// Determines whether two conversion tuple instances are logically equal.
        /// </summary>
        /// <param name="left">The conversion tuple instance on the left side of the operator.</param>
        /// <param name="right">The conversion tuple instance on the right side of the operator.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is logically equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// Equality between to conversion tuple instances is determined by reference equality or by satifying all of the following characteristics:
        /// <list type="number">
        /// <item><term>Reference equality of the raw comparsion function.</term></item>
        /// <item><term>Reference equality of the raw conversion function.</term></item>
        /// <item><term>Applying the raw equality function to <see cref="rawValue"/> of both instances returns <see langword="true"/>.</term></item>
        /// </list>
        /// </remarks>
        public static bool operator ==(ConversionTuple<TRaw, TConvert> left, ConversionTuple<TRaw, TConvert> right)
        {
            if (left is null)
                return right is null;
            else if (right is null)
                return false;
            else if (ReferenceEquals(left, right))
                return true;
            return left.rawEquals == right.rawEquals
                && left.rawConvert == right.rawConvert
                && left.rawEquals(left.rawValue, right.rawValue)
                ;
        }

        /// <summary>
        /// Determines whether two conversion tuple instances are logically not equal.
        /// </summary>
        /// <param name="left">The conversion tuple instance on the left side of the operator.</param>
        /// <param name="right">The conversion tuple instance on the right side of the operator.</param>
        /// <returns><see langword="true"/> if <paramref name="left"/> is logically not equal to <paramref name="right"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// Inequality between to conversion tuple instances is determined by reference inequality and by satifying any of the following characteristics:
        /// <list type="number">
        /// <item><term>Reference inequality of the raw comparsion function.</term></item>
        /// <item><term>Reference inequality of the raw conversion function.</term></item>
        /// <item><term>Applying the raw equality function to <see cref="rawValue"/> of both instances returns <see langword="false"/>.</term></item>
        /// </list>
        /// </remarks>
        public static bool operator !=(ConversionTuple<TRaw, TConvert> left, ConversionTuple<TRaw, TConvert> right)
        {
            if (left is null)
                return !(right is null);
            else if (right is null)
                return true;
            else if (ReferenceEquals(left, right))
                return false;
            return left.rawEquals != right.rawEquals
                || left.rawConvert != right.rawConvert
                || !left.rawEquals(left.rawValue, right.rawValue)
                ;
        }

        /// <summary>
        /// Determines whether the current instance is logically equal to the specified conversion tuple instance.
        /// </summary>
        /// <param name="other">The conversion tuple to check against.</param>
        /// <returns><see langword="true"/> if the current instance is logically equal to <paramref name="other"/>; otherwise, <see langword="false"/>.</returns>
        /// <remarks>
        /// Equality between to conversion tuple instances is determined by reference equality or by satifying all of the following characteristics:
        /// <list type="number">
        /// <item><term>Reference equality of the raw comparsion function.</term></item>
        /// <item><term>Reference equality of the raw conversion function.</term></item>
        /// <item><term>Applying the raw equality function to <see cref="rawValue"/> of both instances returns <see langword="true"/>.</term></item>
        /// </list>
        /// </remarks>
        public bool Equals(ConversionTuple<TRaw, TConvert> other) => this == other;
    }
}
