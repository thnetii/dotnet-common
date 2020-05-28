using System;
using System.Xml.Serialization;
using Xunit;

namespace THNETII.TypeConverter.Xml.Test
{
    public static class XmlEnumStringConverterTest
    {
        public const string One = nameof(One);
        public const string Two = nameof(Two);
        public const string Three = nameof(Three);
        public const string Four = nameof(Four);

        public enum TestEnum
        {
            [XmlEnum("One")]
            First = 1,
            [XmlEnum("Two")]
            Second = 2,
            [XmlEnum("Three")]
            Third = 3,
            [XmlEnum("Four")]
            Fourth = 4,
        }

        [Theory]
        [InlineData(nameof(TestEnum.First))]
        [InlineData(nameof(TestEnum.Second))]
        [InlineData(nameof(TestEnum.Third))]
        [InlineData(nameof(TestEnum.Fourth))]
        public static void CanParseEnumFieldName(string name)
        {
            Assert.Equal(Enum.Parse<TestEnum>(name), XmlEnumStringConverter.Parse<TestEnum>(name));
        }

        [Theory]
        [InlineData(One, TestEnum.First)]
        [InlineData(Two, TestEnum.Second)]
        [InlineData(Three, TestEnum.Third)]
        [InlineData(Four, TestEnum.Fourth)]
        public static void CanParseEnumAttributeName(string name, TestEnum expected)
        {
            Assert.Equal(expected, XmlEnumStringConverter.Parse<TestEnum>(name));
        }
    }
}
