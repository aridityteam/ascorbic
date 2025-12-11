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
using AridityTeam.Platform;

namespace AridityTeam.Platform.Tests.Platform;

public class PlatformInfoTests
{
    [Fact]
    public void OperatingSystem_ShouldNotBeEmpty()
    {
        var os = PlatformInfo.OperatingSystem;

        Assert.NotNull(os);
        Assert.NotEmpty(os);
    }

    [Fact]
    public void ProcessArchitecture_ShouldBeValid()
    {
        var architecture = PlatformInfo.ProcessArchitecture;

        Assert.True(Enum.IsDefined(typeof(Architecture), architecture));
    }

    [Fact]
    public void FrameworkDescription_ShouldNotBeEmpty()
    {
        var framework = PlatformInfo.FrameworkDescription;

        Assert.NotNull(framework);
        Assert.NotEmpty(framework);
    }

    [Fact]
    public void RuntimeVersion_ShouldNotBeNull()
    {
        var version = PlatformInfo.RuntimeVersion;

        Assert.NotNull(version);
    }

    [Fact]
    public void PlatformDetection_ShouldBeConsistent()
    {
        var isWindows = PlatformInfo.IsWindows;
        var isLinux = PlatformInfo.IsLinux;
        var isMacOS = PlatformInfo.IsMacOS;

        var platformCount = (isWindows ? 1 : 0) + (isLinux ? 1 : 0) + (isMacOS ? 1 : 0);
        Assert.Equal(1, platformCount);
    }

    [Fact]
    public void FrameworkDetection_ShouldBeConsistent()
    {
        var isNetFramework = PlatformInfo.IsNetFramework;
        var isNetCoreOrNet5Plus = PlatformInfo.IsNetCoreOrNet5Plus;

        Assert.True(isNetFramework ^ isNetCoreOrNet5Plus);
    }
} 