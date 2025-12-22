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

using AridityTeam.Threading.Internal;

namespace AridityTeam.Threading
{
    /// <summary>
    /// A flavor of <see cref="AutoResetEvent"/> that can be asynchronously awaited on.
    /// </summary>
    public sealed class AsyncAutoResetEvent : AsyncWaitHandle
    {
        private readonly object _lock = new();
        private readonly AsyncWaitQueue _waiters = new();

        private bool _isSet;

        /// <summary>
        /// Initializes a new instance of the <seealso cref="AsyncAutoResetEvent"/> class.
        /// </summary>
        public AsyncAutoResetEvent()
            : this(false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="AsyncAutoResetEvent"/> class.
        /// </summary>
        /// <param name="initialState">Specifies the initial state of the event.</param>
        public AsyncAutoResetEvent(bool initialState)
        {
            _isSet = initialState;
        }

        /// <summary>
        /// Gets whether the handle is currently signaled.
        /// </summary>
        public override bool IsSet => _isSet;

        /// <summary>
        /// Sets the event, releasing all waiters.
        /// </summary>
        public void Set()
        {
            lock (_lock)
            {
                if (_waiters.HasWaiters)
                {
                    _waiters.ReleaseOne();
                }
                else
                {
                    _isSet = true;
                }
            }
        }

        /// <summary>
        /// Asynchronously waits until the handle is signaled.
        /// </summary>
        public override Task WaitAsync()
        {
            lock (_lock)
            {
                if (_isSet)
                {
                    _isSet = false;
                    return Task.CompletedTask;
                }

                return _waiters.Enqueue(CancellationToken.None);
            }
        }

        /// <summary>
        /// Asynchronously waits until the handle is signaled or cancelled.
        /// </summary>
        public override Task WaitAsync(CancellationToken cancellationToken)
        {
            lock (_lock)
            {
                if (_isSet)
                {
                    _isSet = false;
                    return Task.CompletedTask;
                }

                return _waiters.Enqueue(cancellationToken);
            }
        }

        /// <summary>
        /// Asynchronously waits until the handle is signaled or the timeout expires.
        /// </summary>
        public override async Task<bool> WaitAsync(TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);
            try
            {
                await WaitAsync(cts.Token).ConfigureAwait(false);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchronously waits until the handle is signaled, cancelled, or the timeout expires.
        /// </summary>
        public override async Task<bool> WaitAsync(
            TimeSpan timeout,
            CancellationToken cancellationToken)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken);

            cts.CancelAfter(timeout);

            try
            {
                await WaitAsync(cts.Token).ConfigureAwait(false);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }
    }
}
