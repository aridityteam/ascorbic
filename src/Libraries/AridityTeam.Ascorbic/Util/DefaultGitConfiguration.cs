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
using System.IO;

namespace AridityTeam.Util;

/// <summary>
/// Default implementation of <see cref="IGitConfiguration"/>.
/// </summary>
public class DefaultGitConfiguration : IGitConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultGitConfiguration"/> class.
    /// </summary>
    /// <param name="gitExecutablePath">Path to Git executable.</param>
    public DefaultGitConfiguration(string gitExecutablePath)
    {
        GitExecutablePath = ValidateGitPath(gitExecutablePath);
    }

    /// <inheritdoc/>
    public string GitExecutablePath { get; }

    /// <inheritdoc/>
    public bool UseShellExecution => false;

    /// <summary>
    /// Verifies if Git is available in the system PATH.
    /// </summary>
    /// <returns><see langword="true"/> if Git is found in PATH.</returns>
    public static bool IsGitInPath()
    {
        try
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return process.ExitCode == 0 && output.StartsWith("git version", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Finds Git executable in common installation locations.
    /// </summary>
    /// <returns>Path to Git executable if found.</returns>
    /// <exception cref="InvalidOperationException">Thrown when Git is not found.</exception>
    public static string FindGitExecutable()
    {
        // First check if Git is in PATH
        if (IsGitInPath())
        {
            return "git";
        }

        // Check common installation locations
        var possiblePaths = new[]
        {
            @"C:\Program Files\Git\bin\git.exe",
            @"C:\Program Files (x86)\Git\bin\git.exe",
            @"C:\Program Files\Git\cmd\git.exe",
            @"C:\Program Files (x86)\Git\cmd\git.exe"
        };

        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }

        throw new InvalidOperationException(
            "Git executable not found. Please ensure Git is installed and either:\n" +
            "1. Git is added to your system PATH, or\n" +
            "2. Git is installed in one of the standard locations.");
    }

    private static string ValidateGitPath(string gitPath)
    {
        if (string.IsNullOrWhiteSpace(gitPath))
        {
            throw new ArgumentNullException(nameof(gitPath), "Git executable path cannot be null or empty.");
        }

        // If the path is just "git", verify it's in PATH
        if (gitPath.Equals("git", StringComparison.OrdinalIgnoreCase))
        {
            if (!IsGitInPath())
            {
                throw new InvalidOperationException(
                    "Git executable not found in PATH. Please ensure Git is installed and added to your system PATH.");
            }
            return gitPath;
        }

        // Verify the file exists
        if (!File.Exists(gitPath))
        {
            throw new FileNotFoundException(
                $"Git executable not found at: {gitPath}",
                gitPath);
        }

        return gitPath;
    }
} 