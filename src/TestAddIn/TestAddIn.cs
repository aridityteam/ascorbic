using System;
using System.Composition;

using AridityTeam.Addin;

namespace MyAddIn
{
    [Export(typeof(IAddInBase))]
    public class TestAddIn : IAddInBase
    {
        public string Name => "TestAddIn";
        public string Prefix => "TA";
        public string Author => "UnitTest";
        public string Description => "A simple test add-in.";
        public Version Version => new(1, 0, 0, 0);
        public bool IsDisposed { get; private set; }

        public bool Initialize() => true;

        public void Dispose()
        {
            IsDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
