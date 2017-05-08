using System;
using System.Collections.Generic;

namespace THNETII.Common
{
    public class DuplexConversionTuple<TRaw, TConvert> : ConversionTuple<TRaw, TConvert>
    {
        private static readonly Func<TConvert, TConvert, bool> defaultConvertEqualityCheckFunction = GetEqualityCheckFunction<TConvert>();

        private readonly Func<TConvert, TConvert, bool> convertedEquals;
        private readonly Func<TConvert, TRaw> rawReverseConvert;

        new public TConvert ConvertedValue
        {
            get => base.ConvertedValue;
            set
            {
                var localCached = cachedTuple;
                TRaw localRaw;
                if (convertedEquals(localCached.Item2, value))
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

        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TConvert, TRaw> rawReverseConvert)
            : this(rawConvert, GetEqualityCheckFunction<TRaw>(), rawReverseConvert, defaultConvertEqualityCheckFunction) { }
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw> rawEqualityComparer, Func<TConvert, TRaw> rawReverseConvert)
            : this(rawConvert, (rawEqualityComparer ?? throw new ArgumentNullException(nameof(rawEqualityComparer))).Equals,
                  rawReverseConvert, defaultConvertEqualityCheckFunction)
        { }
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TConvert, TRaw> rawReverseConvert, IEqualityComparer<TConvert> convertedEqualityComparer)
            : this(rawConvert, GetEqualityCheckFunction<TRaw>(),
                  rawReverseConvert, (convertedEqualityComparer ?? throw new ArgumentNullException(nameof(convertedEqualityComparer))).Equals)
        { }
        public DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, IEqualityComparer<TRaw> rawEqualityComparer, Func<TConvert, TRaw> rawReverseConvert, IEqualityComparer<TConvert> convertedEqualityComparer)
            : this(rawConvert, (rawEqualityComparer ?? throw new ArgumentNullException(nameof(rawEqualityComparer))).Equals,
                rawReverseConvert, (convertedEqualityComparer ?? throw new ArgumentNullException(nameof(convertedEqualityComparer))).Equals)
        { }

        protected DuplexConversionTuple(Func<TRaw, TConvert> rawConvert, Func<TRaw, TRaw, bool> rawEquals,
            Func<TConvert, TRaw> rawReverseConvert, Func<TConvert, TConvert, bool> convertedEquals)
            : base(rawConvert, rawEquals)
        {
            this.rawReverseConvert = rawReverseConvert ?? throw new ArgumentNullException(nameof(rawReverseConvert));
            this.convertedEquals = convertedEquals ?? throw new ArgumentNullException(nameof(convertedEquals));
        }
    }
}
