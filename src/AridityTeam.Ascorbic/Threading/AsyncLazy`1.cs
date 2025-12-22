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

namespace AridityTeam.Threading
{
    /// <summary>
    /// Represents a lazily-initialized asynchronous value.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    public sealed class AsyncLazy<T> : DisposableObject
    {
        private readonly Func<Task<T>> _factory;
        private readonly object _lock = new();
        private Task<T>? _task;

        /// <summary>
        /// Initializes a new instance of <see cref="AsyncLazy{T}"/>.
        /// </summary>
        /// <param name="factory">The asynchronous factory method to produce the value.</param>
        public AsyncLazy(Func<Task<T>> factory)
        {
            _factory = Requires.NotNull(factory);
        }

        /// <summary>
        /// Gets the value asynchronously, initializing it on first access.
        /// </summary>
        public Task<T> Value
        {
            get
            {
                lock (_lock)
                {
                    Verify.NotDisposed(this);
                    _task ??= _factory();
                    return _task;
                }
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            _task = null;
        }
    }
}
