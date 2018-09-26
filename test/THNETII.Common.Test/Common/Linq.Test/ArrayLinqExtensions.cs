using System;
using System.Linq;

namespace THNETII.Common.Linq.Test
{
    public class ArrayLinqExtensions : LinqExtensionsWithIntsTest
    {
        protected override object GetEmpty() => Array.Empty<int>();
        protected override object GetMoreThan5ButLessThan100() => Enumerable.Range(0, 10).ToArray();

        protected override int First(object source) =>
            ((int[])source).First();
        protected override int FirstOrDefault(object source) =>
            ((int[])source).FirstOrDefault();
        protected override int FirstOrDefault(object source, int @default) =>
            ((int[])source).FirstOrDefault(@default);
        protected override int FirstOrDefault(object source, Func<int> defaultFactory) =>
            ((int[])source).FirstOrDefault(defaultFactory);

        protected override int Last(object source) =>
            ((int[])source).Last();
        protected override int LastOrDefault(object source) =>
            ((int[])source).LastOrDefault();
        protected override int LastOrDefault(object source, int @default) =>
            ((int[])source).LastOrDefault(@default);
        protected override int LastOrDefault(object source, Func<int> defaultFactory) =>
            ((int[])source).LastOrDefault(defaultFactory);

        protected override int ElementAt(object source, int index) =>
            ((int[])source).ElementAt(index);
        protected override int ElementAtOrDefault(object source, int index) =>
            ((int[])source).ElementAtOrDefault(index);
        protected override int ElementAtOrDefault(object source, int index, int @default) =>
            ((int[])source).ElementAtOrDefault(index, @default);
        protected override int ElementAtOrDefault(object source, int index, Func<int> defaultFactory) =>
            ((int[])source).ElementAtOrDefault(index, defaultFactory);
    }
}
