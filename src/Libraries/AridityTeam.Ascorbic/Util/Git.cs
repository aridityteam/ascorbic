/*
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
using System.Text;

namespace AridityTeam.Util;

/// <summary>
/// Provides functionality to interact with the Git command-line tool.
/// </summary>
/// <remarks>
/// This class allows users to execute Git commands by specifying the path to the Git executable. It
/// initializes a process configured to run Git commands with standard output and error redirection.
/// </remarks>
public class Git : IGit
{
    private readonly IGitConfiguration _config;

    /// <summary>
    /// Initializes a new <seealso cref="Git"/> instance.
    /// </summary>
    /// <param name="config">Git configuration.</param>
    public Git(IGitConfiguration? config)
    {
        Requires.NotNull(config);
        _config = config;
    }

    /// <summary>
    /// Initializes a new <seealso cref="Git"/> instance with default configuration.
    /// </summary>
    /// <param name="gitFilePath">Path to Git executable.</param>
    public Git(string gitFilePath) : this(new DefaultGitConfiguration(gitFilePath))
    {
    }

    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? CloneProgressChanged;
    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? CheckoutProgressChanged;
    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? FetchProgressChanged;
    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? PullProgressChanged;
    /// <inheritdoc/>
    public event EventHandler<ProgressChangedEventArgs>? PushProgressChanged;

    private Process CreateProcess(string arguments, string workingDirectory, bool captureOutput = false)
    {
        return new Process
        {
            StartInfo = new ProcessStartInfo(_config.GitExecutablePath)
            {
                Arguments = arguments,
                UseShellExecute = _config.UseShellExecution,
                CreateNoWindow = true,
                RedirectStandardOutput = captureOutput || !_config.UseShellExecution,
                RedirectStandardError = !_config.UseShellExecution,
                WorkingDirectory = workingDirectory
            }
        };
    }

    /// <summary>
    /// Executes a Git command and returns whether it was successful.
    /// </summary>
    /// <param name="arguments">The Git command arguments.</param>
    /// <param name="workingDirectory">The working directory to execute the command in.</param>
    /// <param name="captureOutput">Whether to capture and return the command output.</param>
    /// <returns>True if the command was successful, false otherwise.</returns>
    public bool ExecuteGitCommand(string arguments, string workingDirectory, bool captureOutput = false)
    {
        using var process = CreateProcess(arguments, workingDirectory, captureOutput);
        var errorOutput = new StringBuilder();
        
        if (!_config.UseShellExecution)
        {
            process.OutputDataReceived += OnOutputDataReceived;
            process.ErrorDataReceived += (s, e) => 
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    errorOutput.AppendLine(e.Data);
                    OnErrorDataReceived(s, e);
                }
            };
        }

        if (!process.Start())
            throw new GitException("Could not start \"git.exe\".");

        if (!_config.UseShellExecution)
        {
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }

        process.WaitForExit();

        if (!_config.UseShellExecution)
        {
            process.OutputDataReceived -= OnOutputDataReceived;
            process.ErrorDataReceived -= OnErrorDataReceived;
        }

        if (process.ExitCode != 0)
        {
            throw new GitException($"Git command failed: {arguments}\nError: {errorOutput}");
        }

        return true;
    }

    /// <summary>
    /// Executes a Git command and returns its output.
    /// </summary>
    /// <param name="arguments">The Git command arguments.</param>
    /// <param name="workingDirectory">The working directory to execute the command in.</param>
    /// <returns>The command output as a string.</returns>
    public string ExecuteGitCommandWithOutput(string arguments, string workingDirectory)
    {
        using var process = CreateProcess(arguments, workingDirectory, true);
        var errorOutput = new StringBuilder();
        
        if (!process.Start())
            throw new GitException("Could not start \"git.exe\".");

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new GitException($"Git command failed: {arguments}\nError: {error}");
        }

        return output;
    }

    /// <inheritdoc/>
    public bool Add(string repoPath, string filePath)
    {
        return ExecuteGitCommand($"add \"{filePath}\"", repoPath);
    }

    /// <inheritdoc/>
    public bool Clone(Uri remoteUri, string path, int depth = 0)
    {
        var depthArg = depth > 0 ? $"--depth {depth}" : string.Empty;
        return ExecuteGitCommand($"clone --progress {depthArg} \"{remoteUri.AbsoluteUri}\" \"{path}\"", Path.GetDirectoryName(path)!);
    }

    /// <inheritdoc/>
    public bool Commit(string repoPath, string message)
    {
        return ExecuteGitCommand($"commit -m \"{message}\"", repoPath);
    }

    /// <inheritdoc/>
    public bool Fetch(string repoPath, int depth = 0)
    {
        var depthArg = depth > 0 ? $"--depth {depth}" : string.Empty;
        return ExecuteGitCommand($"fetch --progress {depthArg}", repoPath);
    }

    /// <inheritdoc/>
    public bool Pull(string repoPath, string remote = "origin", string branch = "master")
    {
        return ExecuteGitCommand($"pull --progress {remote} {branch}", repoPath);
    }

    /// <inheritdoc/>
    public bool Push(string repoPath, string remote = "origin", string branch = "master")
    {
        return ExecuteGitCommand($"push --progress {remote} {branch}", repoPath);
    }

    /// <inheritdoc/>
    public bool StashPop(string repoPath)
    {
        return ExecuteGitCommand("stash pop", repoPath);
    }

    /// <inheritdoc/>
    public bool CreateBranch(string repoPath, string branchName)
    {
        return ExecuteGitCommand($"branch {branchName}", repoPath);
    }

    /// <inheritdoc/>
    public bool Checkout(string repoPath, string branchName)
    {
        return ExecuteGitCommand($"checkout {branchName}", repoPath);
    }

    /// <inheritdoc/>
    public string GetCurrentBranch(string repoPath)
    {
        return ExecuteGitCommandWithOutput("rev-parse --abbrev-ref HEAD", repoPath).Trim();
    }

    /// <inheritdoc/>
    public string GetStatus(string repoPath)
    {
        return ExecuteGitCommandWithOutput("status", repoPath);
    }

    /// <inheritdoc/>
    public bool Stash(string repoPath, string? message = null)
    {
        var args = message != null ? $"stash save \"{message}\"" : "stash";
        return ExecuteGitCommand(args, repoPath);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Data)) return;
        HandleProgressLine(e.Data);
    }

    private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Data)) return;
        HandleProgressLine(e.Data);
    }

    private void HandleProgressLine(string line)
    {
        var progress = GitProgressParser.ParseProgress(line);
        if (!progress.HasValue) return;

        var (operation, percentage) = progress.Value;
        var args = new ProgressChangedEventArgs(percentage);

        switch (operation.ToLowerInvariant())
        {
            case string s when 
            s.Contains("receiving") || 
            s.Contains("resolving"):
                CloneProgressChanged?.Invoke(this, args);
                break;

            case string s when s.Contains("updating files"):
                CheckoutProgressChanged?.Invoke(this, args);
                break;

            case string s when s.Contains("fetch"):
                FetchProgressChanged?.Invoke(this, args);
                break;

            case string s when s.Contains("push"):
                PushProgressChanged?.Invoke(this, args);
                break;

            case string s when s.Contains("pull"):
                PullProgressChanged?.Invoke(this, args);
                break;
        }
    }
}
