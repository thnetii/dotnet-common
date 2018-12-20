using System;
using System.Diagnostics;
using System.Threading;

namespace THNETII.Common.Threading
{
    public class SemaphoreSlimContext : IDisposable
    {
        private readonly SemaphoreSlim semaphore;

        protected int ReleaseCount { get; }

        private SemaphoreSlimContext(SemaphoreSlim semaphore, int releaseCount, bool verify)
        {
            Debug.Assert(!verify, nameof(verify) + " is not false.");
            this.semaphore = semaphore;
            ReleaseCount = releaseCount;
        }

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

        #region IDisposable Support
        private int disposedValue = 0; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.Exchange(ref disposedValue, 1) == 0)
            {
                if (disposing)
                {
                    semaphore?.Release(ReleaseCount);
                }
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~SemaphoreSlimContext()
        {
            Dispose(false);
        }
        #endregion
    }
}
