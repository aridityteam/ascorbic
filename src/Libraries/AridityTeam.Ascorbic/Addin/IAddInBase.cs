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

using AridityTeam.Util;

namespace AridityTeam.Addin
{
    /// <summary>
    /// Represents the base of the add-in that can be initialized or disposed by <seealso cref="AddInManager{TInterface}"/>.
    /// </summary>
    public interface IAddInBase : IDisposable, IDisposableObservable
    {
        /// <summary>
        /// The name of the add-in.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The prefix of the add-in.
        /// </summary>
        string Prefix { get; }

        /// <summary>
        /// Author of the add-in.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Optional description of the add-in.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The version of the add-in.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Where most add-in stuff are initialized.
        /// </summary>
        /// <returns></returns>
        bool Initialize();
    }
}
