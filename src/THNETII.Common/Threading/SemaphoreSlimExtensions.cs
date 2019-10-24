using System;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.Threading
{
    /// <summary>
    /// Provides extension methods for the <see cref="SemaphoreSlim"/> class.
    /// </summary>
    public static class SemaphoreSlimExtensions
    {
        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnter(SemaphoreSlim)" />
        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore) =>
            SemaphoreSlimContext.CreateOnEnter(semaphore);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnter(SemaphoreSlim, int)" />
        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            int millisecondsTimeout) =>
            SemaphoreSlimContext.CreateOnEnter(semaphore, millisecondsTimeout);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnter(SemaphoreSlim, TimeSpan)" />
        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            TimeSpan timeout) =>
            SemaphoreSlimContext.CreateOnEnter(semaphore, timeout);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnter(SemaphoreSlim, CancellationToken)" />
        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            CancellationToken cancelToken) =>
            SemaphoreSlimContext.CreateOnEnter(semaphore, cancelToken);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnter(SemaphoreSlim, int, CancellationToken)" />
        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            int millisecondsTimeout, CancellationToken cancelToken) =>
            SemaphoreSlimContext.CreateOnEnter(semaphore, millisecondsTimeout, cancelToken);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnter(SemaphoreSlim, TimeSpan, CancellationToken)" />
        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancelToken) =>
            SemaphoreSlimContext.CreateOnEnter(semaphore, timeout, cancelToken);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnterAsync(SemaphoreSlim)" />
        public static Task<SemaphoreSlimContext> WaitContextAsync(
            this SemaphoreSlim semaphore) =>
            SemaphoreSlimContext.CreateOnEnterAsync(semaphore);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnterAsync(SemaphoreSlim, int)" />
        public static Task<SemaphoreSlimContext> WaitContextAsync(
            this SemaphoreSlim semaphore, int millisecondsTimeout) =>
            SemaphoreSlimContext.CreateOnEnterAsync(semaphore, millisecondsTimeout);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnterAsync(SemaphoreSlim, TimeSpan)" />
        public static Task<SemaphoreSlimContext> WaitContextAsync(
            this SemaphoreSlim semaphore, TimeSpan timeout) =>
            SemaphoreSlimContext.CreateOnEnterAsync(semaphore, timeout);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnterAsync(SemaphoreSlim, CancellationToken)" />
        public static Task<SemaphoreSlimContext> WaitContextAsync(
            this SemaphoreSlim semaphore, CancellationToken cancelToken) =>
            SemaphoreSlimContext.CreateOnEnterAsync(semaphore, cancelToken);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnterAsync(SemaphoreSlim, int, CancellationToken)" />
        public static Task<SemaphoreSlimContext> WaitContextAsync(
            this SemaphoreSlim semaphore, int millisecondsTimeout,
            CancellationToken cancelToken) =>
            SemaphoreSlimContext.CreateOnEnterAsync(semaphore, millisecondsTimeout, cancelToken);

        /// <inheritdoc cref="SemaphoreSlimContext.CreateOnEnterAsync(SemaphoreSlim, TimeSpan, CancellationToken)" />
        public static Task<SemaphoreSlimContext> WaitContextAsync(
            this SemaphoreSlim semaphore, TimeSpan timeout,
            CancellationToken cancelToken) =>
            SemaphoreSlimContext.CreateOnEnterAsync(semaphore, timeout, cancelToken);
    }
}
