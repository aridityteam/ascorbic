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
using AridityTeam.Diagnostics;
using System.Diagnostics;

namespace AridityTeam.Platform.Tests.Diagnostics;

public class StackTraceAnalyzerTests
{
    [Fact]
    public void GetFormattedStackTrace_ShouldNotBeEmpty()
    {
        var stackTrace = StackTraceAnalyzer.GetFormattedStackTrace();

        Assert.NotNull(stackTrace);
        Assert.NotEmpty(stackTrace);
    }

    [Fact]
    public void GetFormattedExceptionStackTrace_ShouldFormatExceptionStackTrace()
    {
        var exception = new Exception("Test exception");

        var stackTrace = StackTraceAnalyzer.GetFormattedExceptionStackTrace(exception);

        Assert.NotNull(stackTrace);
        Assert.NotEmpty(stackTrace);
        Assert.Contains("System.Exception", stackTrace);
    }

    [Fact]
    public void GetFormattedExceptionStackTrace_WithNullException_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StackTraceAnalyzer.GetFormattedExceptionStackTrace(null!));
    }

    [Fact]
    public void GetCallingMethodInfo_ShouldReturnValidInfo()
    {
        var (methodName, filePath, lineNumber) = GetCallingMethodInfo();

        Assert.NotNull(methodName);
        Assert.NotEmpty(methodName);
        Assert.NotNull(filePath);
        Assert.True(lineNumber > 0);
    }

    [Fact]
    public void GetMethodCallStack_ShouldReturnValidStack()
    {
        var methodCalls = StackTraceAnalyzer.GetMethodCallStack();

        Assert.NotNull(methodCalls);
        Assert.NotEmpty(methodCalls);
        Assert.All(methodCalls, call =>
        {
            Assert.NotNull(call.MethodName);
            Assert.NotNull(call.FilePath);
            Assert.True(call.LineNumber > 0);
        });
    }

    [Fact]
    public void GetMethodCallStack_ShouldIncludeCurrentMethod()
    {
        var methodCalls = StackTraceAnalyzer.GetMethodCallStack();
        var currentMethod = GetCurrentMethodName();

        var allMethodNames = string.Join(", ", methodCalls.Select(c => c.MethodName));

        Assert.True(methodCalls.Any(call =>
            call.MethodName != null &&
            call.MethodName.Contains(currentMethod, StringComparison.Ordinal)), 
            $"Current method: {currentMethod}, All methods in stack: {allMethodNames}");
    }

    private static (string MethodName, string FilePath, int LineNumber) GetCallingMethodInfo()
    {
        return StackTraceAnalyzer.GetCallingMethodInfo();
    }

    private static string GetCurrentMethodName()
    {
        var stackTrace = new StackTrace();
        return stackTrace.GetFrame(1)?.GetMethod()?.Name ?? string.Empty;
    }
} 