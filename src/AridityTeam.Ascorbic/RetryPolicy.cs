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

using PolyType;

namespace AridityTeam
{
    /// <summary>
    /// A policy that applies to application errors/failures where it is set
    /// to retry the operation by the amount of times it is specified.
    /// </summary>
    [Serializable]
    [GenerateShape]
    public partial class RetryPolicy
    {
        /// <summary>
        /// Gets the default maximum amount of attempts.
        /// </summary>
        public const int DEFAULT_MAX_ATTEMPTS = 5;

        /// <summary>
        /// Initializes a new default instance of <seealso cref="RetryPolicy"/>.
        /// </summary>
        public static readonly RetryPolicy Default = new(DEFAULT_MAX_ATTEMPTS);

        private readonly int _attempts;
        private volatile int _attempted;

        /// <summary>
        /// The maximum amount of tries on trying to retry the operation.
        /// </summary>
        public int Attempts => _attempts;

        /// <summary>
        /// The current amount of tries on trying to retry the operation.
        /// </summary>
        public int Attempted => _attempted;

        /// <summary>
        /// Gets the <seealso cref="TimeSpan"/> delay.
        /// </summary>
        public TimeSpan? Delay { get; }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="RetryPolicy"/> class.
        /// </summary>
        /// <param name="attempts">The amount of tries until the operation indicates an success.</param>
        private RetryPolicy(int attempts)
        {
            Requires.InRange(attempts, 0, int.MaxValue);

            _attempts = attempts;
            _attempted = 0;
            Delay = TimeSpan.FromSeconds(2);
        }

        /// <summary>
        /// Asynchronously executes the operation by the amount of tries until it indicates an success.
        /// </summary>
        /// <param name="operation">The operation to execute.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        public void Execute(Action operation, CancellationToken cancellationToken = default)
        {
            Requires.NotNull(operation);

            for (int attempt = 1; attempt <= Attempts; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                _attempted++;

                if (_attempted >= _attempts)
                    throw new MaxAttemptsExceededException();

                try
                {
                    operation.Invoke();
                    return;
                }
                catch (Exception) when (attempt < Attempts)
                {
                    if (Delay is not null)
                        Thread.Sleep(Delay.Value.Milliseconds);
                }
            }
        }
    }
}
