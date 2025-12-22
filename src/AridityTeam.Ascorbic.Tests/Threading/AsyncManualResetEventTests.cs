using System;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncManualResetEventTests
    {
        [Fact]
        public async Task WaitAsync_Completes_When_Set()
        {
            var mre = new AsyncManualResetEvent();

            var waitTask = mre.WaitAsync();
            mre.Set();

            await waitTask;
        }

        [Fact]
        public async Task WaitAsync_DoesNotComplete_When_NotSet()
        {
            var mre = new AsyncManualResetEvent();

            var waitTask = mre.WaitAsync();
            await Task.Delay(50);

            Assert.False(waitTask.IsCompleted);
        }

        [Fact]
        public async Task Set_Releases_All_Waiters()
        {
            var mre = new AsyncManualResetEvent();

            var t1 = mre.WaitAsync();
            var t2 = mre.WaitAsync();
            var t3 = mre.WaitAsync();

            mre.Set();

            await Task.WhenAll(t1, t2, t3);
        }

        [Fact]
        public async Task Reset_Blocks_After_Set()
        {
            var mre = new AsyncManualResetEvent();

            mre.Set();
            await mre.WaitAsync();

            mre.Reset();

            var waitTask = mre.WaitAsync();
            await Task.Delay(50);

            Assert.False(waitTask.IsCompleted);
        }

        [Fact]
        public async Task WaitAsync_WithTimeout_ReturnsFalse_When_NotSet()
        {
            var mre = new AsyncManualResetEvent();

            var result = await mre.WaitAsync(TimeSpan.FromMilliseconds(50));

            Assert.False(result);
        }

        [Fact]
        public async Task WaitAsync_WithTimeout_ReturnsTrue_When_Set()
        {
            var mre = new AsyncManualResetEvent();

            var waitTask = mre.WaitAsync(TimeSpan.FromSeconds(1));
            mre.Set();

            Assert.True(await waitTask);
        }

        [Fact]
        public async Task WaitAsync_Cancellation_Cancels()
        {
            var mre = new AsyncManualResetEvent();
            using var cts = new CancellationTokenSource();

            var waitTask = mre.WaitAsync(cts.Token);
            cts.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(
                () => waitTask);
        }
    }
}
