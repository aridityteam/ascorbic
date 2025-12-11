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
using System.Runtime.InteropServices;

namespace AridityTeam.Platform;

/// <summary>
/// Provides information about the current platform and runtime environment.
/// </summary>
public static class PlatformInfo
{
    /// <summary>
    /// Gets a value indicating whether the current platform is Windows.
    /// </summary>
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    /// <summary>
    /// Gets a value indicating whether the current platform is Linux.
    /// </summary>
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    /// <summary>
    /// Gets a value indicating whether the current platform is macOS.
    /// </summary>
    public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    /// <summary>
    /// Gets the current operating system platform.
    /// </summary>
    public static string OperatingSystem => RuntimeInformation.OSDescription;

    /// <summary>
    /// Gets the current process architecture.
    /// </summary>
    public static Architecture ProcessArchitecture => RuntimeInformation.ProcessArchitecture;

    /// <summary>
    /// Gets the current runtime framework description.
    /// </summary>
    
    public static string FrameworkDescription => RuntimeInformation.FrameworkDescription;

    /// <summary>
    /// Gets a value indicating whether the current runtime is .NET Framework.
    /// </summary>
    public static bool IsNetFramework => FrameworkDescription.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets a value indicating whether the current runtime is .NET Core or .NET 5+.
    /// </summary>
    public static bool IsNetCoreOrNet5Plus => FrameworkDescription.StartsWith(".NET", StringComparison.OrdinalIgnoreCase) && !IsNetFramework;

    /// <summary>
    /// Gets the current runtime version.
    /// </summary>
    public static Version RuntimeVersion => Environment.Version;
}