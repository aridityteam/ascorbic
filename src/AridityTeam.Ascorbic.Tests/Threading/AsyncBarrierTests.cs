using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncBarrierTests
    {
        [Fact]
        public async Task Barrier_AllParticipantsMustSignal()
        {
            int participants = 3;
            int phaseActionCalls = 0;

            var barrier = new AsyncBarrier(participants, _ => phaseActionCalls++);

            var tasks = Enumerable.Range(0, participants).Select(_ => barrier.SignalAndWaitAsync());
            await Task.WhenAll(tasks);

            Assert.Equal(1, phaseActionCalls);
            Assert.Equal(1, barrier.CurrentPhaseNumber);
        }

        [Fact]
        public async Task Barrier_Cancellation_Works()
        {
            var barrier = new AsyncBarrier(2);
            var cts = new CancellationTokenSource(50);

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await barrier.SignalAndWaitAsync(cts.Token);
            });
        }
    }
}
