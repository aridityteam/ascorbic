using System;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncAutoResetEventTests
    {
        [Fact]
        public async Task WaitAsync_Completes_When_Set()
        {
            var are = new AsyncAutoResetEvent();

            var waitTask = are.WaitAsync();
            are.Set();

            await waitTask;
        }

        [Fact]
        public async Task Set_Releases_Only_One_Waiter()
        {
            var are = new AsyncAutoResetEvent();

            var t1 = are.WaitAsync();
            var t2 = are.WaitAsync();

            are.Set();
            await Task.Delay(50);

            Assert.True(t1.IsCompleted ^ t2.IsCompleted);
        }

        [Fact]
        public async Task AutoReset_Consumes_Signal()
        {
            var are = new AsyncAutoResetEvent();

            are.Set();
            await are.WaitAsync();

            var secondWait = are.WaitAsync();
            await Task.Delay(50);

            Assert.False(secondWait.IsCompleted);
        }

        [Fact]
        public async Task Multiple_Set_Releases_Multiple_Waiters()
        {
            var are = new AsyncAutoResetEvent();

            var t1 = are.WaitAsync();
            var t2 = are.WaitAsync();

            are.Set();
            are.Set();

            await Task.WhenAll(t1, t2);
        }

        [Fact]
        public async Task WaitAsync_WithTimeout_ReturnsFalse_When_NotSet()
        {
            var are = new AsyncAutoResetEvent();

            var result = await are.WaitAsync(TimeSpan.FromMilliseconds(50));

            Assert.False(result);
        }

        [Fact]
        public async Task WaitAsync_WithTimeout_ReturnsTrue_When_Set()
        {
            var are = new AsyncAutoResetEvent();

            var waitTask = are.WaitAsync(TimeSpan.FromSeconds(1));
            are.Set();

            Assert.True(await waitTask);
        }

        [Fact]
        public async Task WaitAsync_Cancellation_Cancels()
        {
            var are = new AsyncAutoResetEvent();
            using var cts = new CancellationTokenSource();

            var waitTask = are.WaitAsync(cts.Token);
            cts.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(
                () => waitTask);
        }
    }
}
