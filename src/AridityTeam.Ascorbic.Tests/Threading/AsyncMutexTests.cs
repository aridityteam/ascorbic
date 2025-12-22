using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncMutexTests
    {
        [Fact]
        public async Task Mutex_EnsuresExclusiveAccess()
        {
            var mutex = new AsyncMutex();
            int counter = 0;

            var tasks = Enumerable.Range(0, 10).Select(async _ =>
            {
                using (await mutex.LockAsync())
                {
                    int local = counter;
                    await Task.Delay(10);
                    counter = local + 1;
                }
            });

            await Task.WhenAll(tasks);

            Assert.Equal(10, counter);
        }

        [Fact]
        public async Task Mutex_Cancellation_Works()
        {
            var mutex = new AsyncMutex();
            using (await mutex.LockAsync())
            {
                var cts = new CancellationTokenSource(50);
                await Assert.ThrowsAsync<TaskCanceledException>(async () =>
                {
                    await mutex.LockAsync(cts.Token);
                });
            }
        }
    }
}
