using System;
using System.Collections.Generic;

namespace THNETII.Common.Linq.Test
{
    public class StringLinqExtensionsTest : LinqExtensionsWithCharsTest
    {
        protected override object GetEmpty() => string.Empty;
        protected override object GetMoreThan5ButLessThan100() => "abcdefghijklmnopqrstuvwxyz";

        protected override bool Any(object source, out IEnumerable<char> nonEmpty) =>
            ((string)source).Any(out nonEmpty);

        protected override bool Any(object source, Func<char, bool> predicate, out IEnumerable<char> nonEmpty) =>
            ((string)source).Any(predicate, out nonEmpty);

        protected override char First(object source) =>
            ((string)source).First();
        protected override char FirstOrDefault(object source) =>
            ((string)source).FirstOrDefault();
        protected override char FirstOrDefault(object source, char @default) =>
            ((string)source).FirstOrDefault(@default);
        protected override char FirstOrDefault(object source, Func<char> defaultFactory) =>
            ((string)source).FirstOrDefault(defaultFactory);

        protected override char Last(object source) =>
            ((string)source).Last();
        protected override char LastOrDefault(object source) =>
            ((string)source).LastOrDefault();
        protected override char LastOrDefault(object source, char @default) =>
            ((string)source).LastOrDefault(@default);
        protected override char LastOrDefault(object source, Func<char> defaultFactory) =>
            ((string)source).LastOrDefault(defaultFactory);

        protected override char ElementAt(object source, int index) =>
            ((string)source).ElementAt(index);
        protected override char ElementAtOrDefault(object source, int index) =>
            ((string)source).ElementAtOrDefault(index);
        protected override char ElementAtOrDefault(object source, int index, char @default) =>
            ((string)source).ElementAtOrDefault(index, @default);
        protected override char ElementAtOrDefault(object source, int index, Func<char> defaultFactory) =>
            ((string)source).ElementAtOrDefault(index, defaultFactory);
    }
}
