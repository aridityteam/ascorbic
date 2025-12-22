using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncLazyTests
    {
        [Fact]
        public async Task Value_InitializesOnce()
        {
            int counter = 0;
            var lazy = new AsyncLazy<int>(async () =>
            {
                await Task.Delay(50);
                return Interlocked.Increment(ref counter);
            });

            int first = await lazy.Value;
            int second = await lazy.Value;

            Assert.Equal(1, first);
            Assert.Equal(1, second);
            Assert.Equal(1, counter);
        }

        [Fact]
        public async Task MultipleAwaiters_SeeSameValue()
        {
            int counter = 0;
            var lazy = new AsyncLazy<int>(async () =>
            {
                await Task.Delay(50);
                return Interlocked.Increment(ref counter);
            });

            var tasks = Enumerable.Range(0, 5).Select(_ => lazy.Value).ToArray();
            int[] results = await Task.WhenAll(tasks);

            Assert.All(results, val => Assert.Equal(1, val));
            Assert.Equal(1, counter);
        }

        [Fact]
        public async Task Exception_Propagates()
        {
            var lazy = new AsyncLazy<int>(() => throw new InvalidOperationException("fail"));

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () => await lazy.Value);
            Assert.Equal("fail", ex.Message);
        }
    }
}
