using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace THNETII.Common.Collections.Generic
{
    /// <summary>
    /// Wrapper around an <see cref="IEnumerable{T}"/> interface that provides
    /// iteration using an <see cref="IEnumerator{T}"/> that supports <see cref="IEnumerator.Reset"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to enumerate.</typeparam>
    [SuppressMessage("Naming", "CA1710: Identifiers should have correct suffix", Justification = "Type is not a collection, Enumerable suffix is by design.")]
    public class BufferedEnumerable<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> enumerable;

        /// <summary>
        /// Wraps the specified enumerable into a buffered <see cref="IEnumerable{T}"/> interface
        /// that allows for iteration using an <see cref="IEnumerator{T}"/> that supports <see cref="IEnumerator.Reset"/>.
        /// </summary>
        /// <param name="enumerable">The enumerable to wrap. Must not be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/> is <see langword="null"/>.</exception>
        public BufferedEnumerable(IEnumerable<T> enumerable) : base()
        {
            this.enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
        }

        /// <summary>
        /// Returns a buffered enumerator that iterates through the enumerable
        /// and supports <see cref="IEnumerator.Reset"/>.
        /// </summary>
        /// <returns>A buffered, disposable enumerator that can be used to iterate through the enumerable.</returns>
        /// <remarks>
        /// The returned <see cref="IEnumerator{T}"/> instance is guaranteed to
        /// support the <see cref="IEnumerator.Reset"/> method.
        /// <para>
        /// The enumerator maintains an internal buffer to which it adds the
        /// current element when <see cref="IEnumerator.MoveNext"/> is called.
        /// </para>
        /// <para>
        /// When <see cref="IEnumerator.Reset"/> is called on the returned
        /// enumerator, the underlying <see cref="IEnumerator{T}"/> from the
        /// original enumerable is disposed and replaced by a new
        /// <see cref="IEnumerator{T}"/> obtained from the internal buffer.
        /// </para>
        /// </remarks>
        public IEnumerator<T> GetEnumerator()
        {
            return new BufferedEnumerator<T>(
                enumerable.GetEnumerator(),
                (enumerable as ICollection<T>)?.Count
                );
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>Provides buffered enumable extensions for <see cref="IEnumerable{T}"/> instances.</summary>
    /// <seealso cref="BufferedEnumerable{T}"/>
    public static class BufferedEnumerable
    {
        /// <inheritdoc cref="BufferedEnumerable{T}(IEnumerable{T})"/>
        public static BufferedEnumerable<T> AsBufferedEnumerable<T>(this IEnumerable<T> enumerable) =>
            new BufferedEnumerable<T>(enumerable);
    }
}
