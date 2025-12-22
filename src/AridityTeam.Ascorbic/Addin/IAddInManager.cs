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

namespace AridityTeam.Addin
{   
    /// <summary>
    /// Just a simple add-in manager. :D
    /// </summary>
    /// <typeparam name="TInterface">The value of the add-in interface to use.</typeparam>
    public interface IAddInManager<TInterface> where TInterface : class, IAddInBase
    {
        /// <summary>
        /// Gets a collection of loaded add-ins.
        /// </summary>
        IReadOnlyList<TInterface> Addins { get; }

        /// <summary>
        /// Gets an loaded add-in by its name. (Not case-sensitive)
        /// </summary>
        /// <param name="name">The value of the add-in name to find.</param>
        /// <returns>The found add-in.</returns>
        TInterface? GetAddInFromName(string name);

        /// <summary>
        /// Gets an loaded add-in by its prefix. (Not case-sensitive)
        /// </summary>
        /// <param name="prefix">The value of the add-in prefix to find.</param>
        /// <returns>The found add-in.</returns>
        TInterface? GetAddInFromPrefix(string prefix);

        /// <summary>
        /// Discovers add-ins from a folder containing assemblies and loads them using MEF.
        /// </summary>
        /// <param name="folderPath">The folder path containing add-in assemblies.</param>
        void LoadAddInsFromFolder(string folderPath);

        /// <summary>
        /// Loads an add-in from an existing <seealso cref="IAddInBase"/>, whether
        /// from an DLL or whatever.
        /// </summary>
        /// <param name="addin">The value of the add-in class.</param>
        /// <exception cref="AddInException">Thrown when an add-in failed to initialize.</exception>
        void LoadAddIn(TInterface addin);

        /// <summary>
        /// Unloads an add-in if it's found from the loaded add-ins list.
        /// </summary>
        /// <param name="addin">The value of the add-in class.</param>
        void UnloadAddIn(TInterface addin);
    }
}
