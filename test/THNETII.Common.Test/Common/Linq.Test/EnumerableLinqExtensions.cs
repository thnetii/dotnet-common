using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq.Test
{
    public class EnumerableLinqExtensions : LinqExtensionsWithIntsTest
    {
        protected override object GetEmpty() => Enumerable.Empty<int>();
        protected override object GetMoreThan5ButLessThan100() => Enumerable.Range(0, 10);

        protected override int First(object source) =>
            ((IEnumerable<int>)source).First();
        protected override int FirstOrDefault(object source) =>
            ((IEnumerable<int>)source).FirstOrDefault();
        protected override int FirstOrDefault(object source, int @default) =>
            ((IEnumerable<int>)source).FirstOrDefault(@default);
        protected override int FirstOrDefault(object source, Func<int> defaultFactory) =>
            ((IEnumerable<int>)source).FirstOrDefault(defaultFactory);

        protected override int Last(object source) =>
            ((IEnumerable<int>)source).Last();
        protected override int LastOrDefault(object source) =>
            ((IEnumerable<int>)source).LastOrDefault();
        protected override int LastOrDefault(object source, int @default) =>
            ((IEnumerable<int>)source).LastOrDefault(@default);
        protected override int LastOrDefault(object source, Func<int> defaultFactory) =>
            ((IEnumerable<int>)source).LastOrDefault(defaultFactory);

        protected override int ElementAt(object source, int index) =>
            ((IEnumerable<int>)source).ElementAt(index);
        protected override int ElementAtOrDefault(object source, int index) =>
            ((IEnumerable<int>)source).ElementAtOrDefault(index);
        protected override int ElementAtOrDefault(object source, int index, int @default) =>
            ((IEnumerable<int>)source).ElementAtOrDefault(index, @default);
        protected override int ElementAtOrDefault(object source, int index, Func<int> defaultFactory) =>
            ((IEnumerable<int>)source).ElementAtOrDefault(index, defaultFactory);
    }
}
