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
using System.Diagnostics;
using System.Text;

namespace AridityTeam.Diagnostics;

/// <summary>
/// Provides utilities for analyzing and formatting stack traces.
/// </summary>
public static class StackTraceAnalyzer
{
    /// <summary>
    /// Gets a formatted string representation of the current stack trace.
    /// </summary>
    /// <param name="skipFrames">The number of frames to skip from the top of the stack.</param>
    /// <returns>A formatted string representation of the stack trace.</returns>
    public static string GetFormattedStackTrace(int skipFrames = 0)
    {
        var stackTrace = new StackTrace(skipFrames + 1, true);
        return FormatStackTrace(stackTrace);
    }

    /// <summary>
    /// Gets a formatted string representation of an exception's stack trace.
    /// </summary>
    /// <param name="exception">The exception to analyze.</param>
    /// <returns>A formatted string representation of the exception's stack trace.</returns>
    public static string GetFormattedExceptionStackTrace(Exception exception)
    {
        Requires.NotNull(exception);

        var sb = new StringBuilder();
        sb.AppendLine($"Exception: {exception.GetType().FullName}");
        sb.AppendLine($"Message: {exception.Message}");

        var stackTrace = new StackTrace(exception, true);
        var formattedTrace = FormatStackTrace(stackTrace);
        
        if (!string.IsNullOrEmpty(formattedTrace))
        {
            sb.AppendLine("Stack Trace:");
            sb.Append(formattedTrace);
        }
        else
        {
            sb.AppendLine("Stack Trace:");
            sb.Append(exception.StackTrace ?? string.Empty);
        }
        
        return sb.ToString();
    }

    /// <summary>
    /// Gets the calling method information.
    /// </summary>
    /// <param name="skipFrames">The number of frames to skip from the top of the stack.</param>
    /// <returns>A tuple containing the method name, file path, and line number.</returns>
    public static (string MethodName, string FilePath, int LineNumber) GetCallingMethodInfo(int skipFrames = 0)
    {
        var stackTrace = new StackTrace(skipFrames + 1, true);
        var frame = stackTrace.GetFrame(0);
        
        if (frame == null)
            return (string.Empty, string.Empty, 0);

        var method = frame.GetMethod();
        if (method == null)
            return (string.Empty, string.Empty, 0);

        var methodName = method.Name;
        var filePath = frame.GetFileName() ?? string.Empty;
        var lineNumber = frame.GetFileLineNumber();

        // If we don't have file information, try to get it from the declaring type
        if (string.IsNullOrEmpty(filePath) && method.DeclaringType != null)
        {
            filePath = method.DeclaringType.Assembly.Location;
        }

        return (methodName, filePath, lineNumber);
    }

    /// <summary>
    /// Gets a list of method calls in the current stack trace.
    /// </summary>
    /// <param name="skipFrames">The number of frames to skip from the top of the stack.</param>
    /// <returns>A list of method call information.</returns>
    public static List<MethodCallInfo> GetMethodCallStack(int skipFrames = 0)
    {
        var stackTrace = new StackTrace(skipFrames + 1, true);
        var frames = stackTrace.GetFrames();
        
        if (frames == null)
            return new List<MethodCallInfo>();

        return frames
            .Select(frame =>
            {
                var method = frame.GetMethod();
                if (method == null)
                    return null;

                var methodName = method.Name;
                var filePath = frame.GetFileName() ?? string.Empty;
                var lineNumber = frame.GetFileLineNumber();
                var columnNumber = frame.GetFileColumnNumber();

                // If we don't have file information, try to get it from the declaring type
                if (string.IsNullOrEmpty(filePath) && method.DeclaringType != null)
                {
                    filePath = method.DeclaringType.Assembly.Location;
                }

                // If we still don't have a line number, try to get it from the stack frame
                if (lineNumber == 0)
                {
                    lineNumber = frame.GetNativeOffset();
                }

                return new MethodCallInfo
                {
                    MethodName = methodName,
                    FilePath = filePath,
                    LineNumber = lineNumber > 0 ? lineNumber : 1, // Ensure we have a valid line number
                    ColumnNumber = columnNumber
                };
            })
            .Where(info => info != null && !string.IsNullOrEmpty(info.MethodName)) // Filter out null frames and frames without method names
            .Select(info => info!) // Assert non-null after filtering
            .ToList();
    }

    private static string FormatStackTrace(StackTrace stackTrace)
    {
        var sb = new StringBuilder();
        var frames = stackTrace.GetFrames();

        if (frames == null)
            return string.Empty;

        foreach (var frame in frames)
        {
            var method = frame.GetMethod();
            if (method == null)
                continue;

            var declaringType = method.DeclaringType?.FullName ?? "<unknown>";
            var methodName = method.Name;
            var filePath = frame.GetFileName() ?? "<unknown>";
            var lineNumber = frame.GetFileLineNumber();

            sb.AppendLine($"at {declaringType}.{methodName}");
            if (lineNumber > 0)
            {
                sb.AppendLine($"   in {filePath}:line {lineNumber}");
            }
        }

        return sb.ToString();
    }
}

/// <summary>
/// Represents information about a method call in a stack trace.
/// </summary>
public class MethodCallInfo
{
    /// <summary>
    /// Gets or sets the name of the method.
    /// </summary>
    public string? MethodName { get; set; }

    /// <summary>
    /// Gets or sets the file path where the method is defined.
    /// </summary>
    public string? FilePath { get; set; }

    /// <summary>
    /// Gets or sets the line number in the file.
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Gets or sets the column number in the file.
    /// </summary>
    public int ColumnNumber { get; set; }
}
