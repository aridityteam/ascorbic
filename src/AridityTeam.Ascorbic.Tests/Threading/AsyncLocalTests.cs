using System;
using System.Threading.Tasks;

using AridityTeam.Threading;

namespace AridityTeam.Ascorbic.Tests.Threading
{
    public class AsyncLocalTests
    {
        [Fact]
        public async Task Value_IsolatedBetweenAsyncContexts()
        {
            var local = new AsyncLocal<int>();
            local.Value = 1;

            int innerValue = 0;

            await Task.Run(async () =>
            {
                local.Value = 42;
                await Task.Delay(50);
                innerValue = local.Value;
            });

            Assert.Equal(42, innerValue);
            Assert.Equal(1, local.Value);
        }

        [Fact]
        public void Value_PersistsWithinContext()
        {
            var local = new AsyncLocal<string>();
            local.Value = "test";
            Assert.Equal("test", local.Value);
        }

        [Fact]
        public void Dispose_ResetsValue()
        {
            var local = new AsyncLocal<int>();
            local.Value = 123;
            local.Dispose();
            Assert.Equal(default, local.Value);
        }
    }
}
