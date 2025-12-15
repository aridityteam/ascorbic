using System;
using System.Runtime.InteropServices;

namespace AridityTeam.Ascorbic.Tests
{
    public class VerifyTests
    {
        [Fact]
        public void Operation()
        {
            Verify.Operation(true, "Should not throw");

            Assert.Throws<InvalidOperationException>(() => Verify.Operation(false, "throw"));
        }

        [Fact]
        public void Operation_InterpolatedString()
        {
            int formatCount = 0;
            string FormattingMethod()
            {
                formatCount++;
                return "generated string";
            }

            Verify.Operation(true, $"Some {FormattingMethod()} method.");
            Assert.Equal(1, formatCount);

            InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => Verify.Operation(false, $"Some {FormattingMethod()} method."));
            Assert.Equal(2, formatCount);
            Assert.StartsWith("Some generated string method.", ex.Message);
        }

        [Fact]
        public void NotDisposed()
        {
            var observable = new Disposable();
            Verify.NotDisposed(observable);
            observable.Dispose();
            Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(observable));
            Assert.Throws<ObjectDisposedException>(() => Verify.NotDisposed(observable, "message"));
        }

        [Fact]
        public void HResult()
        {
            const int E_INVALIDARG = unchecked((int)0x80070057);
            const int E_FAIL = unchecked((int)0x80004005);
            Verify.HResult(0);
            Assert.Throws<ArgumentException>(() => Verify.HResult(E_INVALIDARG));
            Assert.Throws<COMException>(() => Verify.HResult(E_FAIL));
            Assert.Throws<ArgumentException>(() => Verify.HResult(E_INVALIDARG, ignorePreviousComCalls: true));
            Assert.Throws<COMException>(() => Verify.HResult(E_FAIL, ignorePreviousComCalls: true));
        }

        private class Disposable : DisposableObject
        {
            protected override void DisposeManagedResources()
            {
            }
        }
    }
}
