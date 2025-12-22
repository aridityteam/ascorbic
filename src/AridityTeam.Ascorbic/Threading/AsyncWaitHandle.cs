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
    /// Represents an asynchronously waitable synchronization primitive.
    /// </summary>
    public abstract class AsyncWaitHandle : DisposableObject
    {
        /// <summary>
        /// Asynchronously waits until the handle is signaled.
        /// </summary>
        public abstract Task WaitAsync();

        /// <summary>
        /// Asynchronously waits until the handle is signaled or cancelled.
        /// </summary>
        public abstract Task WaitAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously waits until the handle is signaled or the timeout expires.
        /// </summary>
        public abstract Task<bool> WaitAsync(TimeSpan timeout);

        /// <summary>
        /// Asynchronously waits until the handle is signaled, cancelled, or the timeout expires.
        /// </summary>
        public abstract Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken);

        /// <summary>
        /// Gets whether the handle is currently signaled.
        /// </summary>
        public abstract bool IsSet { get; }

        /// <summary>
        /// Releases resources associated with this handle.
        /// </summary>
        protected override void DisposeManagedResources()
        {
        }
    }
}
