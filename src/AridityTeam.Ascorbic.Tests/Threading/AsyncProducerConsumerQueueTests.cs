using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncProducerConsumerQueueTests
    {
        [Fact]
        public async Task Queue_Enqueue_And_Dequeue_Items()
        {
            var queue = new AsyncProducerConsumerQueue<int>();

            queue.Enqueue(10);
            queue.Enqueue(20);

            var first = await queue.DequeueAsync();
            var second = await queue.DequeueAsync();

            Assert.Equal(10, first);
            Assert.Equal(20, second);
        }

        [Fact]
        public async Task Queue_DequeueAsync_Cancels_Correctly()
        {
            var queue = new AsyncProducerConsumerQueue<int>();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<TaskCanceledException>(() => queue.DequeueAsync(cts.Token));
        }

        [Fact]
        public async Task Queue_Multiple_Producers_Consumers()
        {
            var queue = new AsyncProducerConsumerQueue<int>();
            var produced = new List<int>();
            var consumed = new List<int>();

            var producer = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    queue.Enqueue(i);
                    produced.Add(i);
                    await Task.Delay(1);
                }
            });

            var consumer = Task.Run(async () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    var item = await queue.DequeueAsync();
                    consumed.Add(item);
                }
            });

            await Task.WhenAll(producer, consumer);
            Assert.Equal(produced, consumed);
        }
    }
}
