using System;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncCountdownEventTests
    {
        [Fact]
        public async Task CountdownEvent_Completes_WhenCountReachesZero()
        {
            var cde = new AsyncCountdownEvent(3);

            var task = cde.WaitAsync();

            cde.Signal();
            Assert.False(task.IsCompleted);

            cde.Signal();
            Assert.False(task.IsCompleted);

            cde.Signal();
            await task;
        }

        [Fact]
        public async Task CountdownEvent_Cancellation_Works()
        {
            var cde = new AsyncCountdownEvent(1);
            var cts = new CancellationTokenSource(50);

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await cde.WaitAsync(cts.Token);
            });
        }

        [Fact]
        public void CountdownEvent_AddCount_Succeeds_WhenZero()
        {
            var cde = new AsyncCountdownEvent(1);
            cde.Signal();
            cde.AddCount();
            Assert.Equal(1, cde.CurrentCount);
        }
    }
}
