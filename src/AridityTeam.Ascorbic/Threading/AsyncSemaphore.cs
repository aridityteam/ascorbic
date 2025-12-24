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
    /// An asynchronously awaitable semaphore.
    /// </summary>
    public sealed class AsyncSemaphore : DisposableObject
    {
        private readonly object _lock = new();
        private readonly Queue<TaskCompletionSource<bool>> _waiters = new();

        private int _currentCount;
        private readonly int _maximumCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSemaphore"/> class.
        /// </summary>
        /// <param name="initialCount">The initial number of permits.</param>
        public AsyncSemaphore(int initialCount)
            : this(initialCount, int.MaxValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSemaphore"/> class.
        /// </summary>
        /// <param name="initialCount">The initial number of permits.</param>
        /// <param name="maximumCount">The maximum number of permits.</param>
        public AsyncSemaphore(int initialCount, int maximumCount)
        {
            Requires.InRange(maximumCount, 1, int.MaxValue);
            Requires.InRange(initialCount, 0, maximumCount);

            _maximumCount = maximumCount;
            _currentCount = initialCount;
        }

        /// <summary>
        /// Gets the current number of available permits.
        /// </summary>
        public int CurrentCount
        {
            get
            {
                lock (_lock)
                    return _currentCount;
            }
        }

        /// <summary>
        /// Asynchronously waits to enter the semaphore.
        /// </summary>
        public Task WaitAsync() =>
            WaitAsync(CancellationToken.None);

        /// <summary>
        /// Asynchronously waits to enter the semaphore, observing cancellation.
        /// </summary>
        public Task WaitAsync(CancellationToken cancellationToken)
        {
            lock (_lock)
            {
                Verify.NotDisposed(this);

                if (_currentCount > 0)
                {
                    _currentCount--;
                    return Task.CompletedTask;
                }

                var tcs = new TaskCompletionSource<bool>(
                    TaskCreationOptions.RunContinuationsAsynchronously);

                if (cancellationToken.CanBeCanceled)
                {
                    cancellationToken.Register(
                        static state => ((TaskCompletionSource<bool>)state!).TrySetCanceled(),
                        tcs);
                }

                _waiters.Enqueue(tcs);
                return tcs.Task;
            }
        }

        /// <summary>
        /// Asynchronously waits to enter the semaphore, using a <seealso cref="TimeSpan"/> measure.
        /// </summary>
        public async Task<bool> WaitAsync(TimeSpan timeout)
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
        /// Asynchronously waits to enter the semaphore, observing cancellation, 
        /// and using a <seealso cref="TimeSpan"/> measure.
        /// </summary>
        public async Task<bool> WaitAsync(TimeSpan timeout, CancellationToken cancellationToken)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
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

        /// <summary>
        /// Releases one permit back to the semaphore.
        /// </summary>
        public void Release() =>
            Release(1);

        /// <summary>
        /// Releases multiple permits back to the semaphore.
        /// </summary>
        public void Release(int count)
        {
            Requires.InRange(count, 1, int.MaxValue);

            List<TaskCompletionSource<bool>>? toRelease = null;

            lock (_lock)
            {
                Verify.NotDisposed(this);

                for (int i = 0; i < count; i++)
                {
                    while (_waiters.Count > 0)
                    {
                        var waiter = _waiters.Dequeue();
                        if (!waiter.Task.IsCanceled)
                        {
                            toRelease ??= [];
                            toRelease.Add(waiter);
                            goto next;
                        }
                    }

                    if (_currentCount == _maximumCount)
                        throw new SemaphoreFullException();

                    _currentCount++;

                    next:;
                }
            }

            if (toRelease != null)
            {
                foreach (var w in toRelease)
                    w.TrySetResult(true);
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            lock (_lock)
            {
                while (_waiters.Count > 0)
                    _waiters.Dequeue().TrySetCanceled();
            }
        }
    }
}
