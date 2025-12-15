using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using AridityTeam.Pak;

namespace AridityTeam.Ascorbic.Tests.Pak
{
    public class PakArchiveTests
    {
        [Fact]
        public void Pak_Save_And_Load()
        {
            var pak = new PakArchive();

            pak.AddFile("textures/grass.txt", Encoding.UTF8.GetBytes("Hello World"));
            pak.AddFile("config/settings.json", Encoding.UTF8.GetBytes("{ \"ok\": true }"));

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetTempFileName() + ".pak");
            pak.Save(path);

            var loaded = PakArchive.Load(path);

            Assert.Equal(2, loaded.Chunks.Count);
            Assert.Equal("Hello World",
                Encoding.UTF8.GetString(loaded.Chunks[0].Data));
        }
        [Fact]
        public async Task Pak_SaveAsync_And_LoadAsync()
        {
            var pak = new PakArchive();

            pak.AddFile("textures/grass.txt", Encoding.UTF8.GetBytes("Hello World"));
            pak.AddFile("config/settings.json", Encoding.UTF8.GetBytes("{ \"ok\": true }"));

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetTempFileName() + ".pak");
            await pak.SaveAsync(path);

            var loaded = await PakArchive.LoadAsync(path);

            Assert.Equal(2, loaded.Chunks.Count);
            Assert.Equal("Hello World",
                Encoding.UTF8.GetString(loaded.Chunks[0].Data));
        }
    }
}
