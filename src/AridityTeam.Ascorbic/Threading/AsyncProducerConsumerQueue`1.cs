/*
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.Threading
{
    /// <summary>
    /// A thread-safe, asynchronously awaitable producer/consumer queue.
    /// </summary>
    /// <typeparam name="T">The type of items in the queue.</typeparam>
    public sealed class AsyncProducerConsumerQueue<T> : DisposableObject
    {
        private readonly Queue<T> _queue = new();
        private readonly AsyncSemaphore _signal = new(0);
        private readonly object _lock = new();
        private bool _isCompleted;

        /// <summary>
        /// Enqueues an item to the queue.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        public void Enqueue(T item)
        {
            lock (_lock)
            {
                Verify.NotDisposed(this);
                if (_isCompleted) throw new InvalidOperationException("Queue has been completed.");

                _queue.Enqueue(item);
                _signal.Release();
            }
        }

        /// <summary>
        /// Asynchronously dequeues an item from the queue.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The dequeued item.</returns>
        public async Task<T> DequeueAsync(CancellationToken cancellationToken = default)
        {
            await _signal.WaitAsync(cancellationToken).ConfigureAwait(false);

            lock (_lock)
            {
                Verify.NotDisposed(this);
                return _queue.Dequeue();
            }
        }

        /// <summary>
        /// Marks the queue as complete, no more items can be enqueued.
        /// </summary>
        public void Complete()
        {
            lock (_lock)
            {
                Verify.NotDisposed(this);
                _isCompleted = true;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            _signal.Dispose();
            _queue.Clear();
        }
    }
}
