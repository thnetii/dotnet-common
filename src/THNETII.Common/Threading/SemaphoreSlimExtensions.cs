using System;
using System.Threading;
using System.Threading.Tasks;

namespace THNETII.Common.Threading
{
    public static class SemaphoreSlimExtensions
    {
        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            semaphore.Wait();
            return SemaphoreSlimContext.CreateNoVerify(semaphore);
        }

        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            int millisecondsTimeout)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            var entered = semaphore.Wait(millisecondsTimeout);
            return entered ? SemaphoreSlimContext.CreateNoVerify(semaphore) : null;
        }

        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            TimeSpan timeout)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            var entered = semaphore.Wait(timeout);
            return entered ? SemaphoreSlimContext.CreateNoVerify(semaphore) : null;
        }

        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            CancellationToken cancelToken)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            semaphore.Wait(cancelToken);
            return SemaphoreSlimContext.CreateNoVerify(semaphore);
        }

        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            int millisecondsTimeout, CancellationToken cancelToken)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            var entered = semaphore.Wait(millisecondsTimeout, cancelToken);
            return entered ? SemaphoreSlimContext.CreateNoVerify(semaphore) : null;
        }

        public static SemaphoreSlimContext WaitContext(this SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancelToken)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            var entered = semaphore.Wait(timeout, cancelToken);
            return entered ? SemaphoreSlimContext.CreateNoVerify(semaphore) : null;
        }

        public static async Task<SemaphoreSlimContext> WaitContextAsync(this SemaphoreSlim semaphore)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            await semaphore.WaitAsync()
                .ConfigureAwait(continueOnCapturedContext: false);
            return SemaphoreSlimContext.CreateNoVerify(semaphore);
        }

        public static async Task<SemaphoreSlimContext> WaitContextAsync(this SemaphoreSlim semaphore,
            int millisecondsTimeout)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            var entered = await semaphore.WaitAsync(millisecondsTimeout)
                .ConfigureAwait(continueOnCapturedContext: false);
            return entered ? SemaphoreSlimContext.CreateNoVerify(semaphore) : null;
        }

        public static async Task<SemaphoreSlimContext> WaitContextAsync(this SemaphoreSlim semaphore,
            TimeSpan timeout)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            var entered = await semaphore.WaitAsync(timeout)
                .ConfigureAwait(continueOnCapturedContext: false);
            return entered ? SemaphoreSlimContext.CreateNoVerify(semaphore) : null;
        }

        public static async Task<SemaphoreSlimContext> WaitContextAsync(this SemaphoreSlim semaphore,
            CancellationToken cancelToken)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            await semaphore.WaitAsync(cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return SemaphoreSlimContext.CreateNoVerify(semaphore);
        }

        public static async Task<SemaphoreSlimContext> WaitContextAsync(this SemaphoreSlim semaphore,
            int millisecondsTimeout, CancellationToken cancelToken)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            var entered = await semaphore.WaitAsync(millisecondsTimeout, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return entered ? SemaphoreSlimContext.CreateNoVerify(semaphore) : null;
        }

        public static async Task<SemaphoreSlimContext> WaitContextAsync(this SemaphoreSlim semaphore,
            TimeSpan timeout, CancellationToken cancelToken)
        {
            if (semaphore is null)
                throw new ArgumentNullException(nameof(semaphore));
            var entered = await semaphore.WaitAsync(timeout, cancelToken)
                .ConfigureAwait(continueOnCapturedContext: false);
            return entered ? SemaphoreSlimContext.CreateNoVerify(semaphore) : null;
        }
    }
}
