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
    /// Manages registered application services.
    /// </summary>
    public interface IServiceManager : IServiceProvider, IDisposable
    {
        /// <summary>
        /// Occurs when a service has been added into the manager.
        /// </summary>
        event EventHandler<ServiceChangedEventArgs> ServiceAdded;

        /// <summary>
        /// Occurs when a service has been removed from the manager.
        /// </summary>
        event EventHandler<ServiceChangedEventArgs> ServiceRemoved;

        /// <summary>
        /// Adds/registers a service using the specified interface type (<typeparamref name="TService"/>)'s instance.
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <param name="instance">The value of the service instance.</param>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        void AddService<TService>(TService instance) where TService : class;

        /// <summary>
        /// Adds/registers a service using a boxed factory instance.
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <param name="factory">The value of the boxed factory action.</param>
        /// <param name="singleton">Specifies if it's a singleton instance or not.</param>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        void AddService<TService>(Func<IServiceProvider, TService> factory, bool singleton = true) where TService : class;

        /// <summary>
        /// Removes a service from the manager by trying to find and remove it using the specified interface type (<typeparamref name="TService"/>).
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <returns><see langword="true"/> if it has successfully removed the service from the manager; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        bool RemoveService<TService>() where TService : class;

        /// <summary>
        /// Tries to get an registered service using the specified interface type (<typeparamref name="TService"/>) and returns it if found.
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <param name="service">The output of the found service.</param>
        /// <returns><see langword="true"/> and returns the registered service instance if found; otherwise <see langword="true"/> and <see langword="null"/>.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        bool TryGetService<TService>(out TService? service) where TService : class;

        /// <summary>
        /// Tries to get an registered service using the specified interface type (<typeparamref name="TService"/>) and returns it if found.
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <returns>The registered service instance if found; otherwise <see langword="null"/>.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        TService? GetService<TService>() where TService : class;
    }
}
