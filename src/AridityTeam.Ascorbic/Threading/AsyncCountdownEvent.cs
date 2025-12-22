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
    /// An asynchronously awaitable countdown event.
    /// </summary>
    public sealed class AsyncCountdownEvent : DisposableObject
    {
        private readonly object _lock = new();
        private readonly AsyncManualResetEvent _event = new(false);
        private int _count;

        /// <summary>
        /// Initializes a new instance with the specified initial count.
        /// </summary>
        /// <param name="initialCount">The initial count.</param>
        public AsyncCountdownEvent(int initialCount)
        {
            Requires.InRange(initialCount, 0, int.MaxValue);
            _count = initialCount;
            if (initialCount == 0) _event.Set();
        }

        /// <summary>
        /// Gets the current count.
        /// </summary>
        public int CurrentCount { get { lock (_lock) return _count; } }

        /// <summary>
        /// Decrements the count by one.
        /// </summary>
        public void Signal()
        {
            lock (_lock)
            {
                Verify.NotDisposed(this);
                Verify.Operation(_count > 0, SR.Threading_CountdownAlreadyReachedZero);
                if (--_count == 0) _event.Set();
            }
        }

        /// <summary>
        /// Increments the count by the specified value.
        /// </summary>
        /// <param name="value">The value to add (default 1).</param>
        public void AddCount(int value = 1)
        {
            Requires.InRange(value, 1, int.MaxValue);
            lock (_lock)
            {
                Verify.NotDisposed(this);
                Verify.Operation(_count == 0, SR.Threading_CannotAddCountAfterZero);
                _count += value;
            }
        }

        /// <summary>
        /// Asynchronously waits until the count reaches zero.
        /// </summary>
        public Task WaitAsync() => _event.WaitAsync();

        /// <summary>
        /// Asynchronously waits until the count reaches zero, observing a cancellation token.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the wait.</param>
        public Task WaitAsync(CancellationToken cancellationToken) => _event.WaitAsync(cancellationToken);

        /// <inheritdoc/>
        protected override void DisposeManagedResources() => _event.Dispose();
    }
}
