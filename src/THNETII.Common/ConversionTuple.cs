using System;
using System.Collections.Generic;
using System.Reflection;
using THNETII.Common.Collections.Generic;

namespace THNETII.Common
{
    public class ConversionTuple<TRaw, TConvert>
    {
        private static readonly Func<TRaw, TRaw, bool> defaultRawEqualityCheckFunction = GetEqualityCheckFunction<TRaw>();

        protected readonly object sync = new object();

        private readonly Func<TRaw, TConvert> rawConvert;
        private readonly Func<TRaw, TRaw, bool> rawEquals;
        protected TRaw rawValue;
        protected Tuple<TRaw, TConvert> cachedTuple;

        public TRaw RawValue
        {
            get => rawValue;
            set { lock (sync) { rawValue = value; } }
        }

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

        public void ClearCache() => cachedTuple = null;

        protected static Func<T, T, bool> GetEqualityCheckFunction<T>()
        {
            if (typeof(T).GetTypeInfo().IsValueType)
                return EqualityComparer<T>.Default.Equals;
            else
                return ReferenceEqualityComparer<T>.StaticEquals;
        }

        public ConversionTuple(Func<TRaw, TConvert> rawConvert) : this(rawConvert, defaultRawEqualityCheckFunction) { }
        public ConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw> rawEqualityComparer)
            : this(rawConvert, (rawEqualityComparer ?? throw new ArgumentNullException(nameof(rawEqualityComparer))).Equals) { }
        protected ConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals)
        {
            this.rawConvert = rawConvert ?? throw new ArgumentNullException(nameof(rawConvert));
            this.rawEquals = rawEquals ?? throw new ArgumentNullException(nameof(rawEquals));
        }
    }
}
