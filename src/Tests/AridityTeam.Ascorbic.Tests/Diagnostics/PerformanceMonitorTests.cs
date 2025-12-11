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
using System.Threading;
using AridityTeam.Diagnostics;
using System.Collections.Generic;

namespace AridityTeam.Platform.Tests.Diagnostics;

public class PerformanceMonitorTests
{
    [Fact]
    public void MeasureExecutionTime_ShouldMeasureCorrectly()
    {
        const int expectedDelay = 100;

        var executionTime = PerformanceMonitor.MeasureExecutionTime(() =>
        {
            Thread.Sleep(expectedDelay);
        });

        Assert.True(executionTime >= expectedDelay, $"Execution time {executionTime}ms should be at least {expectedDelay}ms");
    }

    [Fact]
    public void MeasureExecutionTime_WithReturnValue_ShouldMeasureCorrectly()
    {
        const int expectedDelay = 100;
        const int expectedResult = 42;

        var result = PerformanceMonitor.MeasureExecutionTime(() =>
        {
            Thread.Sleep(expectedDelay);
            return expectedResult;
        }, out var executionTime);

        Assert.Equal(expectedResult, result);
        Assert.True(executionTime >= expectedDelay, $"Execution time {executionTime}ms should be at least {expectedDelay}ms");
    }

    [Fact]
    public void GetCurrentMemoryUsage_ShouldReturnPositiveValue()
    {
        var memoryUsage = PerformanceMonitor.GetCurrentMemoryUsage();

        Assert.True(memoryUsage > 0, "Memory usage should be greater than 0");
    }

    [Fact]
    public void GetPeakMemoryUsage_ShouldReturnPositiveValue()
    {
        var peakMemoryUsage = PerformanceMonitor.GetPeakMemoryUsage();

        Assert.True(peakMemoryUsage > 0, "Peak memory usage should be greater than 0");
    }

    [Fact]
    public void MeasureMemoryUsage_ShouldDetectMemoryAllocation()
    {
        const int arraySize = 10000000; // 10MB per array
        var arrays = new List<byte[]>();
        var random = new Random();
        
        var peakBefore = PerformanceMonitor.GetPeakMemoryUsage();

        var (initialMemory, finalMemory) = PerformanceMonitor.MeasureMemoryUsage(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                var array = new byte[arraySize];
                random.NextBytes(array);
                arrays.Add(array);
            }
            GC.KeepAlive(arrays);
        });

        var peakAfter = PerformanceMonitor.GetPeakMemoryUsage();

        Assert.True(peakAfter > peakBefore, $"Peak memory after allocation ({peakAfter:F2}MB) should be greater than before ({peakBefore:F2}MB)");
    }

    [Fact]
    public void CreatePerformanceScope_ShouldMeasureExecution()
    {
        const int expectedDelay = 100;
        var scope = PerformanceMonitor.CreatePerformanceScope();

        Thread.Sleep(expectedDelay);
        scope.Dispose();
    }
} 