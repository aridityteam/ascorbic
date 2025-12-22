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

namespace AridityTeam.Services
{
    /// <summary>
    /// Event data for whenever the service in the <seealso cref="IServiceManager"/> has been changed.
    /// </summary>
    /// <remarks>
    /// Creates a new instance of the event data.
    /// </remarks>
    /// <param name="serviceType">The value of the changed service's type.</param>
    /// <param name="instance">The value of the changed service's instance</param>
    public sealed class ServiceChangedEventArgs(Type serviceType, object? instance) : EventArgs
    {
        /// <summary>
        /// The changed service's type.
        /// </summary>
        public Type ServiceType { get; } = serviceType;

        /// <summary>
        /// The change service's instance.
        /// </summary>
        public object? ServiceInstance { get; } = instance;
    }
}
