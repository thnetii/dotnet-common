using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.Threading
{
    /// <summary>
    /// A disposable context token that can be used to track the lifetime during
    /// which an application holds access to a <see cref="SemaphoreSlim"/>
    /// instance.
    /// <para>
    /// When the context is disposed (or garbage collected), the
    /// <see cref="SemaphoreSlim.Release(int)"/> method on the original
    /// <see cref="SemaphoreSlim"/> instance is invoked.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Using this class allows developers to wrap a critical section protected
    /// by a semaphore in a <c>using</c> code-block. This guarantees for safe
    /// resource management similar to the <c>lock</c> code-block. However, in
    /// addition to the <c>lock</c> block, the <see cref="SemaphoreSlimContext"/>
    /// class also supports asynchronous locking through its Async-methods.
    /// </remarks>
    public class SemaphoreSlimContext : IDisposable
    {
        private readonly SemaphoreSlim semaphore;

        /// <summary>
        /// Gets the number of times the <see cref="SemaphoreSlim"/> object
        /// that is captured by the current instance should be released.
        /// </summary>
        /// <value>An <see cref="int"/> value greater than <c>0</c> (zero).</value>
        /// <remarks>
        /// The returned value corresponds to the value that is passed as the
        /// argument value to <see cref="SemaphoreSlim.Release(int)"/> when
        /// <see cref="Dispose()"/> is invoked.
        /// </remarks>
        protected int ReleaseCount { get; }

        private SemaphoreSlimContext(SemaphoreSlim semaphore, int releaseCount = 1, bool verify = false)
        {
            Debug.Assert(!verify, nameof(verify) + " is not false.");
            this.semaphore = semaphore;
            ReleaseCount = releaseCount;
        }

        /// <summary>
        /// Creates a new semaphore context for the specified and already
        /// acquired <see cref="SemaphoreSlim"/> instance.
        /// </summary>
        /// <param name="semaphore">The <see cref="SemaphoreSlim"/> instance that has been acquired by the application.</param>
        /// <param name="releaseCount">The number of times the semaphore has been acquired. Defaults to <c>1</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="releaseCount"/> is less than <c>1</c>.</exception>
        protected SemaphoreSlimContext(SemaphoreSlim semaphore,
            int releaseCount = 1)
            : this(
                  semaphore ?? throw new ArgumentNullException(nameof(semaphore)),
                  releaseCount >= 1 ? releaseCount : throw new ArgumentOutOfRangeException(nameof(releaseCount), releaseCount, nameof(releaseCount) + " is less than 1"),
                  verify: false
                  )
        { }

        internal static SemaphoreSlimContext CreateNoVerify(SemaphoreSlim semaphore,
            int releaseCount = 1) =>
            new SemaphoreSlimContext(semaphore, releaseCount, verify: false);

        /// <summary>
        /// Blocks the current thread until it can enter the <see cref="SemaphoreSlim" />
        /// and returns a context object that relases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <returns>
        /// A <see cref="SemaphoreSlimContext"/> instance that captures
        /// <paramref name="semaphore"/> and releases the semaphore once its
        /// <see cref="Dispose()"/> method is invoked.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="semaphore"/> instance has been disposed.</exception>
        public static SemaphoreSlimContext CreateOnEnter(SemaphoreSlim semaphore)
        {
            semaphore.ThrowIfNull(nameof(semaphore)).Wait();
            return new SemaphoreSlimContext(semaphore, verify: false);
        }

        /// <summary>
        /// Blocks the current thread until it can enter the <see cref="SemaphoreSlim" />,
        /// while observing a <see cref="CancellationToken"/>
        /// and returns a context object that relases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="cancelToken">The <see cref="CancellationToken" /> token to observe.</param>
        /// <returns>
        /// A <see cref="SemaphoreSlimContext"/> instance that captures
        /// <paramref name="semaphore"/> and releases the semaphore once its
        /// <see cref="Dispose()"/> method is invoked.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="OperationCanceledException"><paramref name="cancelToken" /> was canceled.</exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="semaphore"/> instance has been disposed.<br/>
        /// -or-<br/>
        /// The <see cref="CancellationTokenSource" /> that created <paramref name="cancelToken" /> has already been disposed.
        /// </exception>
        public static SemaphoreSlimContext CreateOnEnter(SemaphoreSlim semaphore,
            CancellationToken cancelToken)
        {
            semaphore.ThrowIfNull(nameof(semaphore)).Wait(cancelToken);
            return new SemaphoreSlimContext(semaphore, verify: false);
        }

        /// <summary>
        /// Blocks the current thread until it can enter the <see cref="SemaphoreSlim" />,
        /// using a 32-bit integer that specifies the timeout
        /// and returns a context object that relases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (<c>-1</c>) to wait indefinitely.</param>
        /// <returns>
        /// <c>null</c> if the timeout expired before the semphore could be entered; otherwise,
        /// a <see cref="SemaphoreSlimContext"/> instance that captures
        /// <paramref name="semaphore"/> and releases the semaphore once its
        /// <see cref="Dispose()"/> method is invoked.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is a negative value other than <c>-1</c>, which represents an infinite time-out.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="semaphore"/> instance has been disposed.</exception>
        public static SemaphoreSlimContext CreateOnEnter(SemaphoreSlim semaphore,
            int millisecondsTimeout)
        {
            bool entered = semaphore.ThrowIfNull(nameof(semaphore))
                .Wait(millisecondsTimeout);
            return entered
                ? new SemaphoreSlimContext(semaphore, verify: false)
                : null;
        }

        /// <summary>
        /// Blocks the current thread until it can enter the <see cref="SemaphoreSlim" />,
        /// using a 32-bit integer that specifies the timeout, while observing a <see cref="CancellationToken"/>
        /// and returns a context object that relases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (<c>-1</c>) to wait indefinitely.</param>
        /// <param name="cancelToken">The <see cref="CancellationToken" /> token to observe.</param>
        /// <returns>
        /// <c>null</c> if the timeout expired before the semphore could be entered; otherwise,
        /// a <see cref="SemaphoreSlimContext"/> instance that captures
        /// <paramref name="semaphore"/> and releases the semaphore once its
        /// <see cref="Dispose()"/> method is invoked.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout"/> is a negative value other than <c>-1</c>, which represents an infinite time-out.</exception>
        /// <exception cref="OperationCanceledException"><paramref name="cancelToken" /> was canceled.</exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="semaphore"/> instance has been disposed.<br/>
        /// -or-<br/>
        /// The <see cref="CancellationTokenSource" /> that created <paramref name="cancelToken" /> has already been disposed.
        /// </exception>
        public static SemaphoreSlimContext CreateOnEnter(SemaphoreSlim semaphore,
            int millisecondsTimeout, CancellationToken cancelToken)
        {
            bool entered = semaphore.ThrowIfNull(nameof(semaphore))
                .Wait(millisecondsTimeout, cancelToken);
            return entered
                ? new SemaphoreSlimContext(semaphore, verify: false)
                : null;
        }

        /// <summary>
        /// Blocks the current thread until it can enter the <see cref="SemaphoreSlim" />,
        /// using the specified timeout
        /// and returns a context object that relases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> value that represents time to wait, or a <see cref="TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <returns>
        /// <c>null</c> if the timeout expired before the semphore could be entered; otherwise,
        /// a <see cref="SemaphoreSlimContext"/> instance that captures
        /// <paramref name="semaphore"/> and releases the semaphore once its
        /// <see cref="Dispose()"/> method is invoked.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is a negative value other than -1 milliseconds, which represents an infinite time-out.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="semaphore"/> instance has been disposed.</exception>
        public static SemaphoreSlimContext CreateOnEnter(SemaphoreSlim semaphore,
            TimeSpan timeout)
        {
            bool entered = semaphore.ThrowIfNull(nameof(semaphore))
                .Wait(timeout);
            return entered
                ? new SemaphoreSlimContext(semaphore, verify: false)
                : null;
        }

        /// <summary>
        /// Blocks the current thread until it can enter the <see cref="SemaphoreSlim" />,
        /// using the specified timeout while observing a <see cref="CancellationToken"/>
        /// and returns a context object that relases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> value that represents time to wait, or a <see cref="TimeSpan" /> that represents -1 milliseconds to wait indefinitely.</param>
        /// <param name="cancelToken">The <see cref="CancellationToken" /> token to observe.</param>
        /// <returns>
        /// <c>null</c> if the timeout expired before the semphore could be entered; otherwise,
        /// a <see cref="SemaphoreSlimContext"/> instance that captures
        /// <paramref name="semaphore"/> and releases the semaphore once its
        /// <see cref="Dispose()"/> method is invoked.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout"/> is a negative value other than -1 milliseconds, which represents an infinite time-out.</exception>
        /// <exception cref="OperationCanceledException"><paramref name="cancelToken" /> was canceled.</exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="semaphore"/> instance has been disposed.<br/>
        /// -or-<br/>
        /// The <see cref="CancellationTokenSource" /> that created <paramref name="cancelToken" /> has already been disposed.
        /// </exception>
        public static SemaphoreSlimContext CreateOnEnter(SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancelToken)
        {
            bool entered = semaphore.ThrowIfNull(nameof(semaphore))
                .Wait(timeout, cancelToken);
            return entered
                ? new SemaphoreSlimContext(semaphore, verify: false)
                : null;
        }

        /// <summary>
        /// Asynchronously waits to enter the <see cref="SemaphoreSlim"/> and
        /// returns a context object that releases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <returns>A task that will complete when the semaphore has been entered.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="semaphore"/> has already been disposed.</exception>
        public static async Task<SemaphoreSlimContext> CreateOnEnterAsync(
            SemaphoreSlim semaphore)
        {
            await semaphore.ThrowIfNull(nameof(semaphore)).WaitAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
            return new SemaphoreSlimContext(semaphore, verify: false);
        }

        /// <summary>
        /// Asynchronously waits to enter a <see cref="SemaphoreSlim" />,
        /// while observing a <see cref="CancellationToken" /> and
        /// returns a context object that releases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <returns>A task that will complete when the semaphore has been entered.</returns>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="cancelToken">The <see cref="CancellationToken" /> token to observe.</param>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="semaphore"/> instance has been disposed.<br/>
        /// -or-<br/>
        /// The <see cref="CancellationTokenSource" /> that created <paramref name="cancelToken" /> has already been disposed.
        /// </exception>
        /// <exception cref="OperationCanceledException"><paramref name="cancelToken" /> was canceled. </exception>
        public static async Task<SemaphoreSlimContext> CreateOnEnterAsync(
            SemaphoreSlim semaphore, CancellationToken cancelToken)
        {
            await semaphore.ThrowIfNull(nameof(semaphore)).WaitAsync(cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return new SemaphoreSlimContext(semaphore, verify: false);
        }

        /// <summary>
        /// Asynchronously waits to enter a <see cref="SemaphoreSlim" />,
        /// using a 32-bit signed integer to measure the time interval and
        /// returns a context object that releases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <returns>
        /// A task that will complete with a result of a context object that
        /// releases the semaphore when disposed if the current thread
        /// successfully entered the <see cref="SemaphoreSlim" />,
        /// otherwise with a result of <c>null</c>.</returns>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (<c>-1</c>) to wait indefinitely.</param>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a negative number other than <c>-1</c>, which represents an infinite time-out.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="semaphore"/> has already been disposed.</exception>
        public static async Task<SemaphoreSlimContext> CreateOnEnterAsync(
            SemaphoreSlim semaphore, int millisecondsTimeout)
        {
            bool entered = await semaphore.ThrowIfNull(nameof(semaphore))
                .WaitAsync(millisecondsTimeout)
                .ConfigureAwait(continueOnCapturedContext: false);
            return entered
                ? new SemaphoreSlimContext(semaphore, verify: false)
                : null;
        }

        /// <summary>
        /// Asynchronously waits to enter a <see cref="SemaphoreSlim" />,
        /// using a 32-bit signed integer to measure the time interval
        /// while observing a <see cref="CancellationToken"/> and
        /// returns a context object that releases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <returns>
        /// A task that will complete with a result of a context object that
        /// releases the semaphore when disposed if the current thread
        /// successfully entered the <see cref="SemaphoreSlim" />,
        /// otherwise with a result of <c>null</c>.</returns>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or <see cref="Timeout.Infinite" /> (<c>-1</c>) to wait indefinitely.</param>
        /// <param name="cancelToken">The <see cref="CancellationToken" /> token to observe.</param>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="millisecondsTimeout" /> is a negative number other than <c>-1</c>, which represents an infinite time-out.</exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="semaphore"/> instance has been disposed.<br/>
        /// -or-<br/>
        /// The <see cref="CancellationTokenSource" /> that created <paramref name="cancelToken" /> has already been disposed.
        /// </exception>
        /// <exception cref="OperationCanceledException"><paramref name="cancelToken" /> was canceled. </exception>
        public static async Task<SemaphoreSlimContext> CreateOnEnterAsync(
            SemaphoreSlim semaphore, int millisecondsTimeout,
            CancellationToken cancelToken)
        {
            bool entered = await semaphore.ThrowIfNull(nameof(semaphore))
                .WaitAsync(millisecondsTimeout, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return entered
                ? new SemaphoreSlimContext(semaphore, verify: false)
                : null;
        }

        /// <summary>
        /// Asynchronously waits to enter a <see cref="SemaphoreSlim" />,
        /// using the specified timeout and
        /// returns a context object that releases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <returns>
        /// A task that will complete with a result of a context object that
        /// releases the semaphore when disposed if the current thread
        /// successfully entered the <see cref="SemaphoreSlim" />,
        /// otherwise with a result of <c>null</c>.</returns>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="timeout">The time to wait, or a <see cref="TimeSpan" /> value representing -1 milliseconds to wait indefinitely.</param>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative value other than -1 milliseconds, which represents an infinite time-out.</exception>
        /// <exception cref="ObjectDisposedException"><paramref name="semaphore"/> has already been disposed.</exception>
        public static async Task<SemaphoreSlimContext> CreateOnEnterAsync(
            SemaphoreSlim semaphore, TimeSpan timeout)
        {
            bool entered = await semaphore.ThrowIfNull(nameof(semaphore))
                .WaitAsync(timeout)
                .ConfigureAwait(continueOnCapturedContext: false);
            return entered
                ? new SemaphoreSlimContext(semaphore, verify: false)
                : null;
        }

        /// <summary>
        /// Asynchronously waits to enter a <see cref="SemaphoreSlim" />,
        /// using the specified timeout and
        /// while observing a <see cref="CancellationToken"/> and
        /// returns a context object that releases the semaphore when it is
        /// disposed.
        /// </summary>
        /// <returns>
        /// A task that will complete with a result of a context object that
        /// releases the semaphore when disposed if the current thread
        /// successfully entered the <see cref="SemaphoreSlim" />,
        /// otherwise with a result of <c>null</c>.</returns>
        /// <param name="semaphore">The semaphore to enter.</param>
        /// <param name="timeout">The time to wait, or a <see cref="TimeSpan" /> value representing -1 milliseconds to wait indefinitely.</param>
        /// <param name="cancelToken">The <see cref="CancellationToken" /> token to observe.</param>
        /// <exception cref="ArgumentNullException"><paramref name="semaphore"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="timeout" /> is a negative value other than -1 milliseconds, which represents an infinite time-out.</exception>
        /// <exception cref="ObjectDisposedException">
        /// <paramref name="semaphore"/> instance has been disposed.<br/>
        /// -or-<br/>
        /// The <see cref="CancellationTokenSource" /> that created <paramref name="cancelToken" /> has already been disposed.
        /// </exception>
        /// <exception cref="OperationCanceledException"><paramref name="cancelToken" /> was canceled. </exception>
        public static async Task<SemaphoreSlimContext> CreateOnEnterAsync(
            SemaphoreSlim semaphore, TimeSpan timeout,
            CancellationToken cancelToken)
        {
            bool entered = await semaphore.ThrowIfNull(nameof(semaphore))
                .WaitAsync(timeout, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return entered
                ? new SemaphoreSlimContext(semaphore, verify: false)
                : null;
        }

        #region IDisposable Support
        private int disposedValue = 0; // To detect redundant calls

        /// <summary>
        /// Releases the captured semaphore by calling
        /// <see cref="SemaphoreSlim.Release(int)"/> with <see cref="ReleaseCount"/>
        /// as the argument value.
        /// <para>
        /// This method is thread-safe and guaranteed to only release the
        /// semaphore exactly once during the lifetime of this context instance.
        /// </para>
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> if called from within an implementation of
        /// <see cref="IDisposable.Dispose"/>, <c>false</c> if called from
        /// within the finalizer.
        /// </param>
        /// <remarks>
        /// The method atomically flips a state flag variable when the method is
        /// executed the first time. Thus, subsequent calls to
        /// <see cref="Dispose(bool)"/> have no effect.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref disposedValue, 1) == 0)
            {
                semaphore?.Release(ReleaseCount);
            }
        }

        /// <summary>
        /// Releases the semaphore instance.
        /// </summary>
        /// <remarks>
        /// This method is thread-safe. The semaphore is only released once.
        /// After the initial call to <see cref="Dispose()"/>, subsequent calls
        /// have no effect.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the semaphore object captured by this instance when this is
        /// instance is garbage collected.
        /// </summary>
        /// <remarks>
        /// Applications should not rely on garbage collection to properly
        /// release held semaphore objects. Instead the current context instance
        /// should be used in a <c>using</c> code-block.
        /// </remarks>
        ~SemaphoreSlimContext()
        {
            Dispose(false);
        }
        #endregion
    }
}
