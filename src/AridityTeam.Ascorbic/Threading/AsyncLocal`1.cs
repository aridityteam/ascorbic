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

namespace AridityTeam.Threading
{
    /// <summary>
    /// Provides asynchronous-local storage of a value.
    /// Each asynchronous context can have its own value.
    /// </summary>
    /// <typeparam name="T">The type of the stored value.</typeparam>
    public sealed class AsyncLocal<T> : DisposableObject
    {
        private readonly System.Threading.AsyncLocal<T?> _internal = new();

        /// <summary>
        /// Gets or sets the value for the current asynchronous context.
        /// </summary>
        public T? Value
        {
            get => _internal.Value;
            set
            {
                Verify.NotDisposed(this);
                _internal.Value = value;
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            _internal.Value = default;
        }
    }
}
