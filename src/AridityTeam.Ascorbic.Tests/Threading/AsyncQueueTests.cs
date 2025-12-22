using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncQueueTests
    {
        [Fact]
        public async Task EnqueueDequeue_UnboundedQueue_Works()
        {
            var queue = new AsyncQueue<int>();
            await queue.EnqueueAsync(42);
            int result = await queue.DequeueAsync();
            Assert.Equal(42, result);
        }

        [Fact]
        public async Task EnqueueDequeue_BoundedQueue_WaitsWhenFull()
        {
            var queue = new AsyncQueue<int>(1);

            _ = queue.EnqueueAsync(1);
            var t2 = queue.EnqueueAsync(2);

            Assert.False(t2.IsCompleted);

            int item = await queue.DequeueAsync(); // releases t2
            await t2;

            int next = await queue.DequeueAsync();

            Assert.Equal(1, item);
            Assert.Equal(2, next);
        }

        [Fact]
        public async Task DequeueAsync_WaitsWhenEmpty()
        {
            var queue = new AsyncQueue<int>();
            var task = queue.DequeueAsync();

            Assert.False(task.IsCompleted);

            await queue.EnqueueAsync(99);
            int result = await task;

            Assert.Equal(99, result);
        }

        [Fact]
        public async Task EnqueueDequeue_CancellationWorks()
        {
            var queue = new AsyncQueue<int>();
            var cts = new CancellationTokenSource();

            var dequeueTask = queue.DequeueAsync(cts.Token);
            cts.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => dequeueTask);
        }
    }
}
