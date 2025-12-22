using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncSemaphoreTests
    {
        [Fact]
        public async Task WaitAsync_Succeeds_When_Count_Available()
        {
            var sem = new AsyncSemaphore(initialCount: 1);

            await sem.WaitAsync();

            // Should not block
            Assert.True(true);
        }

        [Fact]
        public async Task WaitAsync_Blocks_When_Count_Zero()
        {
            var sem = new AsyncSemaphore(initialCount: 0);

            var waitTask = sem.WaitAsync();
            await Task.Delay(50);

            Assert.False(waitTask.IsCompleted);
        }

        [Fact]
        public async Task Release_Allows_Waiter_To_Proceed()
        {
            var sem = new AsyncSemaphore(initialCount: 0);

            var waitTask = sem.WaitAsync();
            sem.Release();

            await waitTask;
        }

        [Fact]
        public async Task Release_Releases_Only_One_Waiter()
        {
            var sem = new AsyncSemaphore(initialCount: 0);

            var t1 = sem.WaitAsync();
            var t2 = sem.WaitAsync();

            sem.Release();
            await Task.Delay(50);

            Assert.True(t1.IsCompleted ^ t2.IsCompleted);
        }

        [Fact]
        public async Task Multiple_Releases_Release_Multiple_Waiters()
        {
            var sem = new AsyncSemaphore(initialCount: 0);

            var t1 = sem.WaitAsync();
            var t2 = sem.WaitAsync();

            sem.Release(2);

            await Task.WhenAll(t1, t2);
        }

        [Fact]
        public async Task WaitAsync_WithTimeout_ReturnsFalse_When_Unavailable()
        {
            var sem = new AsyncSemaphore(initialCount: 0);

            var result = await sem.WaitAsync(TimeSpan.FromMilliseconds(50));

            Assert.False(result);
        }

        [Fact]
        public async Task WaitAsync_WithTimeout_ReturnsTrue_When_Released()
        {
            var sem = new AsyncSemaphore(initialCount: 0);

            var waitTask = sem.WaitAsync(TimeSpan.FromSeconds(1));
            sem.Release();

            Assert.True(await waitTask);
        }

        [Fact]
        public async Task WaitAsync_Cancellation_Cancels()
        {
            var sem = new AsyncSemaphore(initialCount: 0);
            using var cts = new CancellationTokenSource();

            var waitTask = sem.WaitAsync(cts.Token);
            cts.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(
                () => waitTask);
        }

        [Fact]
        public async Task Semaphore_Respects_Max_Count()
        {
            var sem = new AsyncSemaphore(1, 1);

            await sem.WaitAsync();
            sem.Release();

            Assert.Throws<SemaphoreFullException>(sem.Release);
        }

        [Fact]
        public async Task Multiple_Waiters_Are_Served_In_Order()
        {
            var sem = new AsyncSemaphore(initialCount: 1);

            await sem.WaitAsync();

            var t1 = sem.WaitAsync();
            var t2 = sem.WaitAsync();

            sem.Release();
            await t1;

            sem.Release();
            await t2;
        }
    }
}
