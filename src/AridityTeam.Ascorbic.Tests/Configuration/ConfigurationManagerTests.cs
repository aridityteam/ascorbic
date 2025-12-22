using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AridityTeam.Configuration;

namespace AridityTeam.Ascorbic.Tests.Configuration
{
    public class ConfigurationManagerTests
    {
        public class TestConfig
        {
            public string Name { get; set; } = "Default";
            public int Value { get; set; } = 42;
        }

        [Fact]
        public async Task LoadConfigAsync_CreatesDefaultConfig_WhenFileDoesNotExist()
        {
            var tempFile = Path.GetTempFileName();
            File.Delete(tempFile); // ensure file does not exist
            var manager = new ConfigurationManager<TestConfig>(tempFile);

            await manager.LoadConfigAsync();

            Assert.NotNull(manager.CurrentConfig);
            Assert.IsType<TestConfig>(manager.CurrentConfig);
            Assert.Equal("Default", manager.CurrentConfig.Name);
        }

        [Fact]
        public async Task SaveConfigAsync_SavesAndLoadsCorrectly()
        {
            var tempFile = Path.GetTempFileName();
            var manager = new ConfigurationManager<TestConfig>(tempFile);
            var config = new TestConfig { Name = "TestName", Value = 123 };

            await manager.SaveConfigAsync(config);
            await manager.LoadConfigAsync();

            Assert.NotNull(manager.CurrentConfig);
            Assert.Equal("TestName", manager.CurrentConfig.Name);
            Assert.Equal(123, manager.CurrentConfig.Value);
        }

        [Fact]
        public async Task LoadConfigAsync_Throws_WhenCancelled()
        {
            var tempFile = Path.GetTempFileName();
            var manager = new ConfigurationManager<TestConfig>(tempFile);
            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(() => manager.LoadConfigAsync(cts.Token));
        }

        [Fact]
        public async Task SaveConfigAsync_Throws_WhenCancelled()
        {
            var tempFile = Path.GetTempFileName();
            var manager = new ConfigurationManager<TestConfig>(tempFile);
            var config = new TestConfig();
            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(() => manager.SaveConfigAsync(config, cts.Token));
        }

        [Fact]
        public async Task LoadAndSaveConfig_Stream_WorksCorrectly()
        {
            var manager = new ConfigurationManager<TestConfig>();
            var config = new TestConfig { Name = "StreamTest", Value = 999 };

            using var memStream = new MemoryStream();
            await manager.SaveConfigAsync(config, memStream);
            memStream.Position = 0; // rewind

            await manager.LoadConfigAsync(memStream);
            Assert.Equal("StreamTest", manager.CurrentConfig.Name);
            Assert.Equal(999, manager.CurrentConfig.Value);
        }
    }
}
