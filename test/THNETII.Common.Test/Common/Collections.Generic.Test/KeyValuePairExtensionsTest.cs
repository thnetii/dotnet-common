using System;
using System.Collections.Generic;
using Xunit;

namespace THNETII.Common.Collections.Generic.Test
{
    public static class KeyValuePairExtensionsTest
    {
        private const int KeyExpect = 42;
        private const string ValueExpect = nameof(KeyValuePairExtensionsTest);

        [Fact]
        public static void DeconstructKvpToVariables()
        {
            var kvp = KeyValuePair.Create(KeyExpect, ValueExpect);
            var (k, v) = kvp;
            Assert.Equal(KeyExpect, k);
            Assert.Equal(ValueExpect, v);
        }

        [Fact]
        public static void KvpAsValueTuple()
        {
            var kvp = KeyValuePair.Create(KeyExpect, ValueExpect);
#pragma warning disable IDE0042 // Deconstruct variable declaration
            var t = kvp.AsValueTuple();
#pragma warning restore IDE0042 // Deconstruct variable declaration
            Assert.Equal(KeyExpect, t.key);
            Assert.Equal(ValueExpect, t.value);
        }

        [Fact]
        public static void KvpToReferenceTuple()
        {
            var kvp = KeyValuePair.Create(KeyExpect, ValueExpect);
            var t = kvp.ToTuple();
            Assert.NotNull(t);
            Assert.Equal(KeyExpect, t.Item1);
            Assert.Equal(ValueExpect, t.Item2);
        }

        [Fact]
        public static void ValueTupleAsKvp()
        {
            var t = (key: KeyExpect, value: ValueExpect);
            var kvp = t.AsKeyValuePair();
            Assert.Equal(KeyExpect, kvp.Key);
            Assert.Equal(ValueExpect, kvp.Value);
        }

        [Fact]
        public static void ReferenceTupleAsKvp()
        {
            var t = Tuple.Create(KeyExpect, ValueExpect);
            var kvp = t.AsKeyValuePair();
            Assert.Equal(KeyExpect, kvp.Key);
            Assert.Equal(ValueExpect, kvp.Value);
        }
    }
}
