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

using PolyType;

namespace AridityTeam.Diagnostics
{
    /// <summary>
    /// Represents information about the completed diagnostic scope (<seealso cref="DiagnosticScope"/>.
    /// </summary>
    /// <param name="ScopeName">The name of the finished diagnostic scope.</param>
    /// <param name="Elapsed">The elapsed running time by the scope.</param>
    /// <param name="CpuTime">The amount of time used to process functions in the scope.</param>
    /// <param name="AllocatedBytes">The allocated bytes after running the scope.</param>
    /// <param name="WorkingSetDelta">The active memory over a short period.</param>
    /// <param name="GcCollections">
    /// A collection of integers that represent the amount of times that
    /// the garbage collection has occurred.
    /// </param>
    [GenerateShape]
    public partial record DiagnosticSample(
        string ScopeName,
        TimeSpan Elapsed,
        TimeSpan CpuTime,
        long AllocatedBytes,
        long WorkingSetDelta,
        int[] GcCollections
    );
}
