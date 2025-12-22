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
using System.Collections.Concurrent;
using AridityTeam.Util;

namespace AridityTeam.Services
{
    /// <summary>
    /// A class that manages registered services. Useful for getting singleton objects.
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        private readonly ConcurrentDictionary<Type, ServiceEntry> _services = new ConcurrentDictionary<Type, ServiceEntry>();
        private bool _disposed;

        /// <summary>
        /// Occurs when a service has been registered.
        /// </summary>
        public event EventHandler<ServiceChangedEventArgs>? ServiceAdded;

        /// <summary>
        /// Occurs when a service has been unregistered.
        /// </summary>
        public event EventHandler<ServiceChangedEventArgs>? ServiceRemoved;

        /// <summary>
        /// Adds/registers a service using the specified interface type (<typeparamref name="TService"/>)'s instance.
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <param name="instance">The value of the service instance.</param>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        public void AddService<TService>(TService instance) where TService : class
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(ServiceManager).FullName);

            var type = typeof(TService);
            var entry = new ServiceEntry(type, instance);
            _services.AddOrUpdate(type, entry, (_, old) =>
            {
                if (old.Instance is DisposableObject d && !ReferenceEquals(d, instance) && !d.IsDisposed)
                    d.Dispose();
                return entry;
            });

            ServiceAdded?.Invoke(this, new ServiceChangedEventArgs(type, instance));
        }

        /// <summary>
        /// Adds/registers a service using a boxed factory instance.
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <param name="factory">The value of the boxed factory action.</param>
        /// <param name="singleton">Specifies if it's a singleton instance or not.</param>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        public void AddService<TService>(Func<IServiceProvider, TService> factory, bool singleton = true) where TService : class
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(ServiceManager).FullName);

            var type = typeof(TService);
            Func<IServiceProvider, object> boxedFactory = sp => factory(sp);
            var entry = new ServiceEntry(type, boxedFactory, singleton);
            _services.AddOrUpdate(type, entry, (_, old) =>
            {
                if (old.Instance is DisposableObject d && !d.IsDisposed)
                    d.Dispose();
                return entry;
            });

            ServiceAdded?.Invoke(this, new ServiceChangedEventArgs(type, null));
        }

        /// <summary>
        /// Removes a service from the manager by trying to find and remove it using the specified interface type (<typeparamref name="TService"/>).
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <returns><see langword="true"/> if it has successfully removed the service from the manager; otherwise <see langword="false"/>.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        public bool RemoveService<TService>() where TService : class
        {
            var type = typeof(TService);
            if (_services.TryRemove(type, out var entry))
            {
                if (entry.Instance is DisposableObject d && !d.IsDisposed)
                    d.Dispose();
                ServiceRemoved?.Invoke(this, new ServiceChangedEventArgs(type, entry.Instance));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Tries to get an registered service using the specified interface type (<typeparamref name="TService"/>) and returns it if found.
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <param name="service">The output of the found service.</param>
        /// <returns><see langword="true"/> and returns the registered service instance if found; otherwise <see langword="true"/> and <see langword="null"/>.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        public bool TryGetService<TService>(out TService? service) where TService : class
        {
            var type = typeof(TService);
            var obj = GetService(type);
            if (obj is TService s)
            {
                service = s;
                return true;
            }
            service = null;
            return false;
        }

        /// <summary>
        /// Tries to get an registered service using the specified interface type (<typeparamref name="TService"/>) and returns it if found.
        /// </summary>
        /// <typeparam name="TService">The value of the interface type.</typeparam>
        /// <returns>The registered service instance if found; otherwise <see langword="null"/>.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        public TService? GetService<TService>() where TService : class
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(ServiceManager).FullName);

            if (_services.TryGetValue(typeof(TService), out var entry))
            {
                entry.Lock.EnterUpgradeableReadLock();
                try
                {
                    if (entry.Instance != null)
                        return entry.Instance as TService;


                    if (entry.Factory == null)
                        return null;


                    entry.Lock.EnterWriteLock();
                    try
                    {
                        var created = entry.Factory(this);
                        if (entry.IsSingleton)
                        {
                            entry.Instance = created;
                            return entry.Instance as TService;
                        }
                        else
                        {
                            return created as TService;
                        }
                    }
                    finally
                    {
                        entry.Lock.ExitWriteLock();
                    }
                }
                finally
                {
                    entry.Lock.ExitUpgradeableReadLock();
                }
            }
            return null;
        }

        /// <summary>
        /// Tries to get an registered service using the specified interface type (<paramref name="serviceType"/>) and returns it if found.
        /// </summary>
        /// <param name="serviceType">The value of the service type.</param>
        /// <returns>The registered service instance if found; otherwise <see langword="null"/>.</returns>
        /// <exception cref="ObjectDisposedException">Thrown when the <seealso cref="IServiceManager"/> has been disposed.</exception>
        public object? GetService(Type serviceType)
        {
            if (_disposed)
                throw new ObjectDisposedException(typeof(ServiceManager).FullName);

            if (_services.TryGetValue(serviceType, out var entry))
            {
                entry.Lock.EnterUpgradeableReadLock();
                try
                {
                    if (entry.Instance != null)
                        return entry.Instance;


                    if (entry.Factory == null)
                        return null;


                    entry.Lock.EnterWriteLock();
                    try
                    {
                        var created = entry.Factory(this);
                        if (entry.IsSingleton)
                        {
                            entry.Instance = created;
                            return entry.Instance;
                        }
                        else
                        {
                            return created;
                        }
                    }
                    finally
                    {
                        entry.Lock.ExitWriteLock();
                    }
                }
                finally
                {
                    entry.Lock.ExitUpgradeableReadLock();
                }
            }
            return null;
        }

        /// <summary>
        /// Disposes and releases every managed and unmanaged resources that the services uses.
        /// </summary>
        /// <param name="disposing">Specifies whether to dispose manage resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                foreach (var kv in _services)
                {
                    var entry = kv.Value;
                    if (entry.Instance is DisposableObject d)
                    {
                        try { d.Dispose(); } catch { }
                    }
                    entry.Lock.Dispose();
                }
            }

            _services.Clear();

            _disposed = true;
        }

        /// <summary>
        /// Disposes and releases every managed and unmanaged resources that the services uses.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
