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
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace AridityTeam.Addin
{
    /// <summary>
    /// Just a simple add-in manager. :D
    /// </summary>
    /// <typeparam name="TInterface">The value of the add-in interface to use.</typeparam>
    public class AddInManager<TInterface> : DisposableObject where TInterface : class, IAddInBase
    {
        private readonly List<TInterface> _addins = new List<TInterface>();

        /// <summary>
        /// Gets a collection of loaded add-ins.
        /// </summary>
        public List<TInterface> Addins => _addins;

        /// <summary>
        /// Gets an loaded add-in by its name. (Not case-sensitive)
        /// </summary>
        /// <param name="name">The value of the add-in name to find.</param>
        /// <returns>The found add-in.</returns>
        public TInterface? GetAddInFromName(string name)
        {
            foreach (var addin in _addins)
            {
                if (!addin.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    continue;

                return addin;
            }
            return null;
        }

        /// <summary>
        /// Gets an loaded add-in by its prefix. (Not case-sensitive)
        /// </summary>
        /// <param name="prefix">The value of the add-in prefix to find.</param>
        /// <returns>The found add-in.</returns>
        public TInterface? GetAddInFromPrefix(string prefix)
        {
            foreach (var addin in _addins)
            {
                if (!addin.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                return addin;
            }
            return null;
        }

        /// <summary>
        /// Loads an add-in from a class library (*.dll) file.
        /// </summary>
        /// <param name="dllPath">The value of the DLL path.</param>
        public void LoadAddInFromFile(string dllPath)
        {
            Requires.FileExists(dllPath);
            var dll = Assembly.LoadFrom(dllPath);
            Requires.NotNull(dll);

            var type = dll.GetTypes()
                .First(t =>
                typeof(TInterface).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            var addin = Activator.CreateInstance(type) as TInterface;
            Requires.NotNull(addin);

            LoadAddIn(addin);
        }

        /// <summary>
        /// Loads an add-in from an existing <seealso cref="IAddInBase"/>, whether
        /// from an DLL or whatever.
        /// </summary>
        /// <param name="addin">The value of the add-in class.</param>
        /// <exception cref="AddInException">Thrown when an add-in failed to initialize.</exception>
        public void LoadAddIn(TInterface addin)
        {
            Requires.NotNull(addin);

            if (_addins.Contains(addin))
                throw new AddInException(
                    PrivateErrorHelpers.Format(SR.AddIn_FailedToInitialize, addin.Name));

            if (!addin.Initialize())
                throw new AddInException(
                    PrivateErrorHelpers.Format(SR.AddIn_FailedToInitialize, addin.Name));

            _addins.Add(addin);
        }

        /// <summary>
        /// Unloads an add-in if it's found from the loaded add-ins list.
        /// </summary>
        /// <param name="addin">The value of the add-in class.</param>
        public void UnloadAddIn(TInterface addin)
        {
            Requires.NotNull(addin);

            if (!_addins.Contains(addin))
                return;

            if (addin.IsDisposed)
                return;

            addin.Dispose();
            _addins.Remove(addin);
        }

        /// <summary>
        /// Performs a task that disposes every resources used by the loaded add-ins and this manager.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            var addinsCache = new List<TInterface>(_addins);
            foreach (var addin in addinsCache)
                UnloadAddIn(addin);

            addinsCache.Clear();

            _addins.Clear();
        }
    }
}
