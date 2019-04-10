using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace THNETII.Common.Collections.Generic
{
    internal sealed class BufferedEnumerator<T> : IEnumerator<T>
    {
        internal IEnumerator<T> Enumerator => enumerator ?? throw new ObjectDisposedException(nameof(enumerator));

        internal ICollection<T> Buffer => buffer ?? throw new ObjectDisposedException(nameof(buffer));

        public T Current => Enumerator.Current;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            var enumerator = Enumerator;
            if (enumerator.MoveNext())
            {
                Buffer.Add(enumerator.Current);
                return true;
            }
            return false;
        }

        public void Reset()
        {
            var (previousEnumerator, previousBuffer) = (Enumerator, Buffer);
            (enumerator, buffer) = (previousBuffer.GetEnumerator(), new List<T>(previousBuffer.Count));
            previousEnumerator.Dispose();
        }

        private IEnumerator<T> enumerator;
        private ICollection<T> buffer;

        /// <summary>
        /// Creates a new wrapper aorund the specified enumerable, optionally
        /// with an indication of the number of exepected enumerabled items for
        /// optimal allocation of the internal buffer.
        /// </summary>
        /// <param name="enumerator">The <see cref="IEnumerator{T}"/> to wrap. Must not be <see langword="null"/>.</param>
        /// <param name="capacity">Optional. The initial capacity for the internal buffer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumerator"/> is <see langword="null"/>.</exception>
        public BufferedEnumerator(IEnumerator<T> enumerator, int? capacity = null)
            : this(enumerator, capacity.HasValue ? new List<T>(capacity.Value) : new List<T>())
        { }

        internal BufferedEnumerator(IEnumerator<T> enumerator, ICollection<T> buffer)
        {
            this.enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
            this.buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref enumerator, null)?.Dispose();
            if (Interlocked.Exchange(ref this.buffer, null).TryNotNull(out var buffer))
            {
                buffer.Clear();
            }
        }
    }
}
