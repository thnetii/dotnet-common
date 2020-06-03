using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace THNETII.Common.Test
{
    public class StringCommonExtensionsTest
    {
        [Fact]
        public void ContainsThrowsWithNullSource()
        {
            const string? test = null;
            Assert.Throws<ArgumentNullException>(() => StringCommonExtensions.Contains(test!, "test", default));
        }

        [Fact]
        public void ContainsThrowsWithNullValue()
        {
            const string test = "test";
            Assert.Throws<ArgumentNullException>(() => StringCommonExtensions.Contains(test, null!, default));
        }

        [Fact]
        public void ContainsUsingOrdinalIgnoreCase()
        {
            const string source = "test";
            string contains = source.ToUpperInvariant();

            Assert.True(source.Contains(contains, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void EnumerateLinesOfNullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null!).EnumerateLines());
        }

        [Fact]
        public void EnumerateLinesOfEmptyReturnsEmpty()
        {
            Assert.Empty(string.Empty.EnumerateLines());
        }

        [Theory]
        [InlineData("\r")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void EnumerateLinesUsesAnyLineSeparator(string newline)
        {
            var test = Enumerable.Range(0, 10)
                .Select(i => i.ToString(CultureInfo.InvariantCulture))
                .ToArray();
            var multiLine = string.Join(newline, test);
            Assert.Equal(test, multiLine.EnumerateLines());
        }

        [Fact]
        public void MultiReplaceOnNullThrows()
        {
            const string? nullString = null;
            Assert.Throws<ArgumentNullException>("s", () => nullString!.Replace(mappings: null!));
        }

        [Fact]
        public void MultiReplaceWithNullMappingsThrows()
        {
            const string test = nameof(MultiReplaceWithNullMappingsThrows);
            Assert.Throws<ArgumentNullException>("mappings", () => test.Replace(mappings: null!));
        }

        [Fact]
        public void MultiReplaceWithNullOldValueThrows()
        {
            const string test = nameof(MultiReplaceWithNullOldValueThrows);
            Assert.Throws<ArgumentNullException>(
                FormattableString.Invariant($"mappings[{0}].oldValue"),
                () => test.Replace(new[] { ((string)null!, string.Empty) })
                );
        }

        [Fact]
        public void MultiReplaceWithNullNewValueThrows()
        {
            const string test = nameof(MultiReplaceWithNullNewValueThrows);
            Assert.Throws<ArgumentNullException>(
                FormattableString.Invariant($"mappings[{0}].newValue"),
                () => test.Replace(new[] { (string.Empty, (string)null!) })
                );
        }

        [Fact]
        public void MultiReplaceWithEmptyMappingsReturnsSame()
        {
            const string test = nameof(MultiReplaceWithEmptyMappingsReturnsSame);
            Assert.Same(test, test.Replace(Enumerable.Empty<(string, string)>()));
        }

        [Fact]
        public void MultiReplaceWithEmptyOldValueReturnsSame()
        {
            const string test = nameof(MultiReplaceWithEmptyOldValueReturnsSame);
            Assert.Same(test, test.Replace(new[] { (string.Empty, string.Empty) }));
        }

        [Fact]
        public void MultiReplaceWithNoMatchingReplacementsReturnsSame()
        {
            const string test = nameof(MultiReplaceWithNoMatchingReplacementsReturnsSame);
            Assert.Same(test, test.Replace(new[] { (nameof(test), string.Empty) }));
        }

        public static IEnumerable<object[]> GetMultiReplaceParameters()
        {
            yield return new object[] { "abba", new[] { ("a", "b"), ("b", "a") }, "baab" };
            yield return new object[] {
                nameof(GetMultiReplaceParameters),
                Enumerable.Range('a', 26).Select(v =>
                {
                    char lower = (char)v, upper = char.ToUpperInvariant((char)v);
                    return (new string(lower, 1), new string(upper, 1));
                }),
                nameof(GetMultiReplaceParameters).ToUpperInvariant()
            };
        }

        [Theory]
        [MemberData(nameof(GetMultiReplaceParameters))]
        public void MultiReplaceReturnsReplacedString(string oldValue, IEnumerable<(string, string)> mappings, string expected)
        {
            Assert.Equal(expected, oldValue.Replace(mappings), StringComparer.Ordinal);
        }
    }
}
