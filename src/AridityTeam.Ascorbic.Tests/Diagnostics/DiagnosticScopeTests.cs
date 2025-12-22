using System;
using System.Threading;

using AridityTeam.Diagnostics;

namespace AridityTeam.Ascorbic.Tests.Diagnostics
{
    public class DiagnosticScopeTests
    {
        [Fact]
        public void Scope_Returns_Valid_Sample()
        {
            using var scope = DiagnosticScope.Start();

            Thread.Sleep(10);

            var sample = scope.Complete();

            Assert.NotNull(sample);
            Assert.True(sample.Elapsed > TimeSpan.Zero);
            Assert.True(sample.CpuTime >= TimeSpan.Zero);
            Assert.NotNull(sample.GcCollections);
            Assert.Equal(3, sample.GcCollections.Length);
        }

        [Fact]
        public void CpuTime_Increases_When_Work_Is_Done()
        {
            using var idle = DiagnosticScope.Start("idle");
            Thread.Sleep(20);
            var idleSample = idle.Complete();

            using var busy = DiagnosticScope.Start("busy");
            double x = 0;
            for (int i = 0; i < 20_000_000; i++)
                x += Math.Sqrt(i);
            var busySample = busy.Complete();

            Assert.True(
                busySample.CpuTime >= idleSample.CpuTime,
                $"Expected busy CPU >= idle CPU, but got idle={idleSample.CpuTime}, busy={busySample.CpuTime}"
            );
        }

        [Fact]
        public void AllocatedBytes_Is_NonNegative_When_Supported()
        {
            using var scope = DiagnosticScope.Start("alloc");

            var list = new int[10_000];

            var sample = scope.Complete();

            if (sample.AllocatedBytes >= 0)
            {
                Assert.True(sample.AllocatedBytes > 0);
            }
            else
            {
                Assert.Equal(-1, sample.AllocatedBytes);
            }
        }

        [Fact]
        public void GcCollections_Are_NonNegative()
        {
            using var scope = DiagnosticScope.Start("gc");

            for (int i = 0; i < 10_000; i++)
                _ = new object();

            var sample = scope.Complete();

            foreach (var count in sample.GcCollections)
            {
                Assert.True(count >= 0);
            }
        }

        [Fact]
        public void WorkingSetDelta_Is_Not_Invalid()
        {
            using var scope = DiagnosticScope.Start("memory");

            var data = new byte[1024 * 1024];

            var sample = scope.Complete();

            Assert.NotEqual(long.MinValue, sample.WorkingSetDelta);
        }

        [Fact]
        public void Nested_Scopes_Work_Correctly()
        {
            using var outer = DiagnosticScope.Start("outer");

            Thread.Sleep(5);

            using var inner = DiagnosticScope.Start("inner");
            Thread.Sleep(5);
            var innerSample = inner.Complete();

            var outerSample = outer.Complete();

            Assert.True(outerSample.Elapsed >= innerSample.Elapsed);
        }

        [Fact]
        public void Dispose_Does_Not_Throw()
        {
            DiagnosticSample? sample = null;

            using (var scope = DiagnosticScope.Start("dispose"))
            {
                Thread.Sleep(5);
                sample = scope.Complete();
            }

            Assert.NotNull(sample);
        }
    }
}
