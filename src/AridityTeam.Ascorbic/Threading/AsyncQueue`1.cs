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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AridityTeam.Threading
{
    /// <summary>
    /// A thread-safe asynchronously awaitable queue.
    /// </summary>
    /// <typeparam name="T">The type of items in the queue.</typeparam>
    public sealed class AsyncQueue<T> : DisposableObject
    {
        private readonly object _lock = new();
        private readonly Queue<T> _queue = new();
        private readonly AsyncSemaphore _itemsAvailable = new(0);
        private readonly AsyncSemaphore? _spaceAvailable;
        private readonly int? _boundedCapacity;

        /// <summary>
        /// Initializes a new instance of <see cref="AsyncQueue{T}"/>.
        /// </summary>
        /// <param name="boundedCapacity">Optional maximum number of items the queue can hold. Null for unbounded.</param>
        public AsyncQueue(int? boundedCapacity = null)
        {
            if (boundedCapacity.HasValue)
                Requires.InRange(boundedCapacity.Value, 1, int.MaxValue);

            _boundedCapacity = boundedCapacity;
            if (boundedCapacity.HasValue)
                _spaceAvailable = new AsyncSemaphore(boundedCapacity.Value, boundedCapacity.Value);
        }

        /// <summary>
        /// Asynchronously enqueues an item into the queue.
        /// Waits if the queue is full (for bounded queues).
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        public async Task EnqueueAsync(T item, CancellationToken cancellationToken = default)
        {
            if (_boundedCapacity.HasValue)
                await _spaceAvailable!.WaitAsync(cancellationToken).ConfigureAwait(false);

            lock (_lock)
            {
                Verify.NotDisposed(this);
                _queue.Enqueue(item);
            }

            _itemsAvailable.Release();
        }

        /// <summary>
        /// Asynchronously dequeues an item from the queue.
        /// Waits if the queue is empty.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <returns>The dequeued item.</returns>
        public async Task<T> DequeueAsync(CancellationToken cancellationToken = default)
        {
            await _itemsAvailable.WaitAsync(cancellationToken).ConfigureAwait(false);

            T item;
            lock (_lock)
            {
                Verify.NotDisposed(this);
                item = _queue.Dequeue();
            }

            _spaceAvailable?.Release();

            return item;
        }

        /// <summary>
        /// Gets the current number of items in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    Verify.NotDisposed(this);
                    return _queue.Count;
                }
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            _itemsAvailable.Dispose();
            _spaceAvailable?.Dispose();
        }
    }
}
