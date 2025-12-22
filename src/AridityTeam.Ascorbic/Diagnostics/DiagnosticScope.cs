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
using System.Diagnostics;

namespace AridityTeam.Diagnostics
{
    /// <summary>
    /// Provides a set of utilities for creating a scope for measuring
    /// the performance used in the running operation.
    /// </summary>
    public class DiagnosticScope : DisposableObject
    {
        #region Properties
        private readonly Stopwatch _stopwatch;
        private readonly Process _process;

        private readonly TimeSpan _cpuStart;
        private readonly long _workingSetStart;
        private readonly long _allocatedStart;
        private readonly int[] _gcStart;
        private readonly string _scopeName;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <seealso cref="DiagnosticScope"/> class.
        /// </summary>
        private DiagnosticScope(string scopeName)
        {
            _scopeName = scopeName;
            _process = Process.GetCurrentProcess();

            _cpuStart = _process.TotalProcessorTime;
            _workingSetStart = _process.WorkingSet64;

            _allocatedStart =
#if NET8_0_OR_GREATER
                GC.GetAllocatedBytesForCurrentThread();
#else
                -1;
#endif

            _gcStart =
            [
                GC.CollectionCount(0),
                GC.CollectionCount(1),
                GC.CollectionCount(2)
            ];

            _stopwatch = new();
            _stopwatch.Start();
        }
        #endregion

        #region Static Methods/Helpers
        /// <summary>
        /// Initializes a new instance of the <seealso cref="DiagnosticScope"/> class.
        /// </summary>
        public static DiagnosticScope Start(string scopeName = "Scope") => new(scopeName);
        #endregion

        #region Methods/Helpers
        /// <summary>
        /// Marks the running operation/scope as completed.
        /// </summary>
        public DiagnosticSample Complete()
        {
            _stopwatch.Stop();

            _process.Refresh();

            var cpuEnd = _process.TotalProcessorTime;
            var workingSetEnd = _process.WorkingSet64;

            var allocatedEnd =
#if NET8_0_OR_GREATER
                GC.GetAllocatedBytesForCurrentThread();
#else
                -1;
#endif

            var gcEnd = new[]
            {
                GC.CollectionCount(0),
                GC.CollectionCount(1),
                GC.CollectionCount(2)
            };

            return new DiagnosticSample(
                ScopeName: _scopeName,
                Elapsed: _stopwatch.Elapsed,
                CpuTime: cpuEnd - _cpuStart,
                AllocatedBytes: allocatedEnd >= 0 ? allocatedEnd - _allocatedStart : -1,
                WorkingSetDelta: workingSetEnd - _workingSetStart,
                GcCollections:
                [
                    gcEnd[0] - _gcStart[0],
                    gcEnd[1] - _gcStart[1],
                    gcEnd[2] - _gcStart[2]
                ]
            );
        }
        #endregion

        #region Overridden Common Methods
        /// <inheritdoc/>
        protected override void DisposeManagedResources() =>
            Complete();
        #endregion
    }
}
