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
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AridityTeam.Addin
{
    /// <summary>
    /// Just a simple add-in manager. :D
    /// </summary>
    /// <typeparam name="TInterface">The value of the add-in interface to use.</typeparam>
    public class AddInManager<TInterface> : DisposableObject, IAddInManager<TInterface> where TInterface : class, IAddInBase
    {
        private readonly List<TInterface> _addins = [];
        private CompositionHost? _compositionHost;

        /// <summary>
        /// Gets a read-only collection of loaded add-ins.
        /// </summary>
        public IReadOnlyList<TInterface> Addins => _addins;

        /// <summary>
        /// Discovers add-ins from a folder containing assemblies and loads them using MEF.
        /// </summary>
        /// <param name="folderPath">The folder path containing add-in assemblies.</param>
        public void LoadAddInsFromFolder(string folderPath)
        {
            Verify.NotDisposed(this);
            Requires.DirectoryExists(folderPath);

            var assemblies = Directory.GetFiles(folderPath, "*.dll")
                                      .Select(Assembly.LoadFrom)
                                      .ToList();

            var configuration = new ContainerConfiguration()
                .WithAssemblies(assemblies);

            _compositionHost?.Dispose();
            _compositionHost = configuration.CreateContainer();

            var exports = _compositionHost.GetExports<TInterface>();

            foreach (var addin in exports)
            {
                LoadAddIn(addin);
            }
        }

        /// <summary>
        /// Gets an add-in by its name (case-insensitive).
        /// </summary>
        public TInterface? GetAddInFromName(string name)
        {
            Verify.NotDisposed(this);
            return _addins.FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets an add-in by its prefix (case-insensitive).
        /// </summary>
        public TInterface? GetAddInFromPrefix(string prefix)
        {
            Verify.NotDisposed(this);
            return _addins.FirstOrDefault(a => a.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Loads an existing add-in instance.
        /// </summary>
        /// <param name="addin">The add-in instance to load.</param>
        /// <exception cref="AddInException">Thrown if the add-in fails to initialize or already exists.</exception>
        public void LoadAddIn(TInterface addin)
        {
            Verify.NotDisposed(this);
            Requires.NotNull(addin);

            if (_addins.Contains(addin))
                throw new AddInException($"Add-in '{addin.Name}' is already loaded.");

            if (!addin.Initialize())
                throw new AddInException($"Add-in '{addin.Name}' failed to initialize.");

            _addins.Add(addin);
        }

        /// <summary>
        /// Unloads a loaded add-in instance.
        /// </summary>
        /// <param name="addin">The add-in to unload.</param>
        public void UnloadAddIn(TInterface addin)
        {
            Verify.NotDisposed(this);
            Requires.NotNull(addin);

            if (!_addins.Contains(addin) || addin.IsDisposed)
                return;

            addin.Dispose();
            _addins.Remove(addin);
        }

        /// <summary>
        /// Performs disposal of loaded add-ins and the MEF container.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            foreach (var addin in _addins.ToArray())
            {
                try { addin.Dispose(); } catch { }
            }

            _addins.Clear();

            _compositionHost?.Dispose();
            _compositionHost = null;
        }
    }
}
