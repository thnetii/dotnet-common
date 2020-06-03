using System;
using System.Collections.Generic;
using System.Linq;

namespace THNETII.Common.Linq.Test
{
    public class ListLinqExtensions : LinqExtensionsWithIntsTest
    {
        protected override object GetEmpty() => new List<int>(capacity: 0);
        protected override object GetMoreThan5ButLessThan100() => Enumerable.Range(0, 10).ToList();

        protected override int First(object source) =>
            ((List<int>)source).First();
        protected override int FirstOrDefault(object source) =>
            ((List<int>)source).FirstOrDefault();
        protected override int FirstOrDefault(object source, int @default) =>
            ((List<int>)source).FirstOrDefault(@default);
        protected override int FirstOrDefault(object source, Func<int> defaultFactory) =>
            ((List<int>)source).FirstOrDefault(defaultFactory);

        protected override int Last(object source) =>
            ((List<int>)source).Last();
        protected override int LastOrDefault(object source) =>
            ((List<int>)source).LastOrDefault();
        protected override int LastOrDefault(object source, int @default) =>
            ((List<int>)source).LastOrDefault(@default);
        protected override int LastOrDefault(object source, Func<int> defaultFactory) =>
            ((List<int>)source).LastOrDefault(defaultFactory);

        protected override int ElementAt(object source, int index) =>
            ((List<int>)source).ElementAt(index);
        protected override int ElementAtOrDefault(object source, int index) =>
            ((List<int>)source).ElementAtOrDefault(index);
        protected override int ElementAtOrDefault(object source, int index, int @default) =>
            ((List<int>)source).ElementAtOrDefault(index, @default);
        protected override int ElementAtOrDefault(object source, int index, Func<int> defaultFactory) =>
            ((List<int>)source).ElementAtOrDefault(index, defaultFactory);
    }
}
