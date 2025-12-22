using System;
using System.Collections.Generic;
using System.Text;

namespace AridityTeam.Ascorbic.Tests
{
    public class DisposableObjectTests
    {
        [Fact]
        public void DisposableObject_Disposed()
        {
            var disposable = new Disposable();
            Assert.False(disposable.IsDisposed);
            disposable.Dispose();
            Assert.True(disposable.IsDisposed);
        }

        private class Disposable : DisposableObject
        {
            protected override void DisposeManagedResources()
            {
            }
        }
    }
}
