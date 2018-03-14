using System;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace THNETII.Common.DataContractSerializer.Test
{
    public static class EnumStringConverterTest
    {
        public const string One = nameof(One);
        public const string Two = nameof(Two);
        public const string Three = nameof(Three);
        public const string Four = nameof(Four);

        public enum TestEnum
        {
            [EnumMember(Value = One)]
            First = 1,
            [EnumMember(Value = Two)]
            Second = 2,
            [EnumMember(Value = Three)]
            Third = 3,
            [EnumMember(Value = Four)]
            Fourth = 4,
        }

        [Theory]
        [InlineData(nameof(TestEnum.First))]
        [InlineData(nameof(TestEnum.Second))]
        [InlineData(nameof(TestEnum.Third))]
        [InlineData(nameof(TestEnum.Fourth))]
        public static void CanParseEnumFieldName(string name)
        {
            Assert.Equal(Enum.Parse<TestEnum>(name), EnumStringConverter<TestEnum>.Parse(name));
        }

        [Theory]
        [InlineData(One, TestEnum.First)]
        [InlineData(Two, TestEnum.Second)]
        [InlineData(Three, TestEnum.Third)]
        [InlineData(Four, TestEnum.Fourth)]
        public static void CanParseEnumAttributeName(string name, TestEnum expected)
        {
            Assert.Equal(expected, EnumStringConverter<TestEnum>.Parse(name));
        }
    }
}
