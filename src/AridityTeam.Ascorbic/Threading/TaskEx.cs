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
    /// Custom <seealso cref="Task"/> extensions.
    /// </summary>
    public static class TaskEx
    {
        /// <summary>
        /// Runs an asynchronous operation with proper cancellation.
        /// </summary>
        /// <remarks>
        /// Cancellation is cooperative. If the underlying I/O operation has
        /// already started, cancellation will only stop waiting for completion
        /// and will not abort the operation.
        /// </remarks>
        /// <param name="task">The asynchronous operation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that is running until it finishes, unless it's been canceled.</returns>
        /// <exception cref="TaskCanceledException">Thrown if the operation has been canceled by <paramref name="cancellationToken"/>.</exception>
        public static async Task<T> WithCancellation<T>(
            this Task<T> task,
            CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
                return await task.ConfigureAwait(false);

            var tcs = new TaskCompletionSource<object?>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(
                static s => ((TaskCompletionSource<object?>)s!).TrySetResult(null),
                tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                    throw new TaskCanceledException(task);
            }

            return await task.ConfigureAwait(false);
        }

        /// <summary>
        /// Runs an asynchronous operation with proper cancellation.
        /// </summary>
        /// <remarks>
        /// Cancellation is cooperative. If the underlying I/O operation has
        /// already started, cancellation will only stop waiting for completion
        /// and will not abort the operation.
        /// </remarks>
        /// <param name="task">The asynchronous operation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that is running until it finishes, unless it's been canceled.</returns>
        /// <exception cref="TaskCanceledException">Thrown if the operation has been canceled by <paramref name="cancellationToken"/>.</exception>
        public static async Task WithCancellation(
            this Task task,
            CancellationToken cancellationToken)
        {
            if (!cancellationToken.CanBeCanceled)
            {
                await task.ConfigureAwait(false);
                return;
            }

            var tcs = new TaskCompletionSource<object?>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(
                static s => ((TaskCompletionSource<object?>)s!).TrySetResult(null),
                tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                    throw new TaskCanceledException(task);
            }

            await task.ConfigureAwait(false);
        }
    }
}
