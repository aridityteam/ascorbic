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
{/// <summary>
 /// An asynchronously awaitable barrier for multiple participants.
 /// </summary>
    public sealed class AsyncBarrier : DisposableObject
    {
        private readonly object _lock = new();
        private readonly int _participantCount;
        private readonly Action<int>? _postPhaseAction;

        private int _remaining;
        private int _phase;
        private AsyncManualResetEvent _phaseEvent = new(false);

        /// <summary>
        /// Initializes a new instance of <see cref="AsyncBarrier"/>.
        /// </summary>
        /// <param name="participantCount">Number of participants.</param>
        /// <param name="postPhaseAction">Optional action invoked after each phase.</param>
        public AsyncBarrier(int participantCount, Action<int>? postPhaseAction = null)
        {
            Requires.InRange(participantCount, 1, int.MaxValue);
            _participantCount = participantCount;
            _remaining = participantCount;
            _postPhaseAction = postPhaseAction;
        }

        /// <summary>
        /// Gets the current phase number.
        /// </summary>
        public int CurrentPhaseNumber { get { lock (_lock) return _phase; } }

        /// <summary>
        /// Signals arrival at the barrier and asynchronously waits for all participants.
        /// </summary>
        public Task SignalAndWaitAsync() => SignalAndWaitAsync(CancellationToken.None);

        /// <summary>
        /// Signals arrival at the barrier and asynchronously waits for all participants, observing a cancellation token.
        /// </summary>
        public async Task SignalAndWaitAsync(CancellationToken cancellationToken)
        {
            AsyncManualResetEvent toWait;
            int phase;

            lock (_lock)
            {
                Verify.NotDisposed(this);

                phase = _phase;

                if (--_remaining == 0)
                {
                    _postPhaseAction?.Invoke(_phase);
                    _remaining = _participantCount;
                    _phase++;
                    _phaseEvent.Set();
                    _phaseEvent = new AsyncManualResetEvent(false);
                    return;
                }

                toWait = _phaseEvent;
            }

            using (cancellationToken.Register(() => toWait.Set()))
            {
                await toWait.WaitAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources() => _phaseEvent.Dispose();
    }
}
