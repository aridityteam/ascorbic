using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncReaderWriterLockTests
    {
        [Fact]
        public async Task ReaderWriterLock_AllowsMultipleReaders()
        {
            var rw = new AsyncReaderWriterLock();
            int readersInside = 0;

            var tasks = Enumerable.Range(0, 5).Select(async _ =>
            {
                using (await rw.AcquireReaderLockAsync())
                {
                    Interlocked.Increment(ref readersInside);
                    await Task.Delay(20);
                    Interlocked.Decrement(ref readersInside);
                }
            });

            await Task.WhenAll(tasks);
            Assert.Equal(0, readersInside);
        }

        [Fact]
        public async Task ReaderWriterLock_WriterExclusive()
        {
            var rw = new AsyncReaderWriterLock();
            int active = 0;

            var writer = Task.Run(async () =>
            {
                using (await rw.AcquireWriterLockAsync())
                {
                    active++;
                    await Task.Delay(50);
                    active--;
                }
            });

            await Task.Delay(10);

            var reader = Task.Run(async () =>
            {
                using (await rw.AcquireReaderLockAsync())
                {
                    Interlocked.Increment(ref active);
                    await Task.Delay(10);
                    Interlocked.Decrement(ref active);
                }
            });

            await Task.WhenAll(writer, reader);

            Assert.Equal(0, active);
        }

        [Fact]
        public async Task ReaderWriterLock_Cancellation_Works()
        {
            var rw = new AsyncReaderWriterLock();
            using (await rw.AcquireWriterLockAsync())
            {
                var cts = new CancellationTokenSource(50);
                await Assert.ThrowsAsync<TaskCanceledException>(async () =>
                {
                    await rw.AcquireReaderLockAsync(cts.Token);
                });
            }
        }
    }
}
