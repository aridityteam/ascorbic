﻿/*
 * Copyright (c) 2025 The Aridity Team
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.Threading
{
    /// <summary>
    /// An asynchronously awaitable reader-writer lock.
    /// </summary>
    public sealed class AsyncReaderWriterLock : DisposableObject
    {
        private readonly AsyncSemaphore _mutex = new(1, 1);
        private readonly AsyncSemaphore _writeLock = new(1, 1);
        private int _readers;

        /// <summary>
        /// Acquires a reader lock asynchronously.
        /// </summary>
        public Task<IDisposable> AcquireReaderLockAsync() => AcquireReaderLockAsync(CancellationToken.None);

        /// <summary>
        /// Acquires a reader lock asynchronously, observing a cancellation token.
        /// </summary>
        public async Task<IDisposable> AcquireReaderLockAsync(CancellationToken cancellationToken)
        {
            await _mutex.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                if (++_readers == 1)
                    await _writeLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                _mutex.Release();
            }

            return new ReaderReleaser(this);
        }

        /// <summary>
        /// Acquires a writer lock asynchronously.
        /// </summary>
        public Task<IDisposable> AcquireWriterLockAsync() => AcquireWriterLockAsync(CancellationToken.None);

        /// <summary>
        /// Acquires a writer lock asynchronously, observing a cancellation token.
        /// </summary>
        public async Task<IDisposable> AcquireWriterLockAsync(CancellationToken cancellationToken)
        {
            await _writeLock.WaitAsync(cancellationToken).ConfigureAwait(false);
            return new WriterReleaser(_writeLock);
        }

        private sealed class ReaderReleaser(AsyncReaderWriterLock owner) : IDisposable
        {
            private AsyncReaderWriterLock? _owner = owner;

            public void Dispose()
            {
                var owner = _owner;
                if (owner == null) return;

                owner._mutex.WaitAsync().GetAwaiter().GetResult();
                try
                {
                    if (--owner._readers == 0)
                        owner._writeLock.Release();
                }
                finally
                {
                    owner._mutex.Release();
                    _owner = null;
                }
            }
        }

        private sealed class WriterReleaser(AsyncSemaphore writeLock) : IDisposable
        {
            private AsyncSemaphore? _lock = writeLock;

            public void Dispose()
            {
                _lock?.Release();
                _lock = null;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            _mutex.Dispose();
            _writeLock.Dispose();
        }
    }

}
