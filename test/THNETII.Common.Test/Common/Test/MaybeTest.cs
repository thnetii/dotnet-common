using System;

using Xunit;

namespace THNETII.Common.Test
{
    public static class MaybeTest
    {
        [Fact]
        public static void NewHasNoValueAndThrows()
        {
            var maybe = new Maybe<string>();
            Assert.False(maybe.HasValue);
            Assert.Throws<InvalidOperationException>(() => maybe.Value);
        }

        [Fact]
        public static void NewWithArgumentHasValue()
        {
            const string expect = nameof(NewWithArgumentHasValue);
            var maybe = Maybe.Create(expect);
            Assert.True(maybe.HasValue);
            Assert.Equal(expect, maybe.Value);
        }

        [Fact]
        public static void NewWithNullArgumentHasValue()
        {
            object value = null;
            var maybe = Maybe.Create(value);
            Assert.True(maybe.HasValue);
            Assert.Null(maybe.Value);
        }
    }
}
