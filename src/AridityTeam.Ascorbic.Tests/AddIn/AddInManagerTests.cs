using System;

using AridityTeam.Addin;

using MyAddIn;

namespace AridityTeam.Ascorbic.Tests.AddIn
{
    public class AddInManagerTests
    {
        [Fact]
        public void LoadAddInsFromFolder_FindsTestAddIn()
        {
            var manager = new AddInManager<IAddInBase>();

            var folder = AppDomain.CurrentDomain.BaseDirectory;
            manager.LoadAddInsFromFolder(folder);

            var addin = manager.GetAddInFromName("TestAddIn");

            Assert.NotNull(addin);
            Assert.Equal("TestAddIn", addin!.Name);
            Assert.True(addin.Initialize());
        }

        [Fact]
        public void LoadAddIn_AddsInstanceManually()
        {
            var manager = new AddInManager<IAddInBase>();
            var addin = new TestAddIn();

            manager.LoadAddIn(addin);

            Assert.Contains(addin, manager.Addins);
            Assert.Equal("TA", addin.Prefix);
        }

        [Fact]
        public void UnloadAddIn_DisposesAndRemoves()
        {
            var manager = new AddInManager<IAddInBase>();
            var addin = new TestAddIn();
            manager.LoadAddIn(addin);

            manager.UnloadAddIn(addin);

            Assert.DoesNotContain(addin, manager.Addins);
            Assert.True(addin.IsDisposed);
        }

        [Fact]
        public void Dispose_DisposesAllAddIns()
        {
            var manager = new AddInManager<IAddInBase>();
            var addin1 = new TestAddIn();
            var addin2 = new TestAddIn();

            manager.LoadAddIn(addin1);
            manager.LoadAddIn(addin2);

            manager.Dispose();

            Assert.True(addin1.IsDisposed);
            Assert.True(addin2.IsDisposed);
        }
    }
}
