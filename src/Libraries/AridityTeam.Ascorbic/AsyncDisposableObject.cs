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
using System.Threading.Tasks;

using AridityTeam.Util;

namespace AridityTeam
{
    /// <summary>
    /// Provides a base class for objects that implement the <see cref="IAsyncDisposable"/> pattern, with support for
    /// observing the disposed state asynchronously.
    /// </summary>
    public abstract class AsyncDisposableObject : IAsyncDisposable, IDisposableObservable
    {
        private bool _disposed;

        /// <summary>
        /// Checks whether the object has been disposed or not.
        /// </summary>
        public bool IsDisposed => _disposed;

        /// <summary>
        /// Disposes and releases managed resources used by the object.
        /// </summary>
        protected virtual Task DisposeManagedResourcesAsync() =>
            Task.CompletedTask;

        /// <summary>
        /// Disposes and releases unmanaged resources used by the object.
        /// </summary>
        protected virtual unsafe Task DisposeUnmanagedResourcesAsync() =>
            Task.CompletedTask;

        private async Task DisposeAsync(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    await DisposeManagedResourcesAsync();
                }

                await DisposeUnmanagedResourcesAsync();

                _disposed = true;
            }
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            // Do not change this code. Put cleanup code in 'DisposeAsync(bool disposing)' method
            await DisposeAsync(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
