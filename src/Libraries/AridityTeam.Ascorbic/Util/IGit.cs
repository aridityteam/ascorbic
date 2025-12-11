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

namespace AridityTeam.Util;

/// <summary>
/// Configuration for Git operations.
/// </summary>
public interface IGitConfiguration
{
    /// <summary>
    /// Gets the path to the Git executable.
    /// </summary>
    string GitExecutablePath { get; }

    /// <summary>
    /// Gets whether to use shell execution for Git commands.
    /// </summary>
    bool UseShellExecution { get; }
}

/// <summary>
/// Wraps Git functions.
/// </summary>
public interface IGit : IDisposable
{
    /// <summary>
    /// An event to be raised when the progress on cloning a repository has changed.
    /// </summary>
    event EventHandler<ProgressChangedEventArgs>? CloneProgressChanged;

    /// <summary>
    /// An event to be raised when the progress on checking out to a branch has changed.
    /// </summary>
    event EventHandler<ProgressChangedEventArgs>? CheckoutProgressChanged;

    /// <summary>
    /// An event to be raised when the progress on fetching the latest changes has changed.
    /// </summary>
    event EventHandler<ProgressChangedEventArgs>? FetchProgressChanged;

    /// <summary>
    /// An event to be raised when the progress on pulling the latest changes has changed.
    /// </summary>
    event EventHandler<ProgressChangedEventArgs>? PullProgressChanged;

    /// <summary>
    /// An event to be raised when the progress on pushing the staged commit has changed.
    /// </summary>
    event EventHandler<ProgressChangedEventArgs>? PushProgressChanged;

    /// <summary>
    /// Stages a commit locally to the repository.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <param name="message">Commit message.</param>
    /// <returns><see langword="true"/> on success.</returns>
    bool Commit(string repoPath, string message);

    /// <summary>
    /// Fetches the latest changes from the remote server.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <param name="depth">Depth.</param>
    /// <returns><see langword="true"/> on success.</returns>
    bool Fetch(string repoPath, int depth = 0);

    /// <summary>
    /// Clones a remote repository into the specified path.
    /// </summary>
    /// <param name="remoteUri">Remote URL.</param>
    /// <param name="path">Path.</param>
    /// <param name="depth">Depth.</param>
    /// <returns><see langword="true"/> on success.</returns>
    bool Clone(Uri remoteUri, string path, int depth = 0);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <param name="remote"></param>
    /// <param name="branch"></param>
    /// <returns><see langword="true"/> on success.</returns>
    bool Push(string repoPath, string remote = "origin", string branch = "master");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <param name="remote"></param>
    /// <param name="branch"></param>
    /// <returns><see langword="true"/> on success.</returns>
    bool Pull(string repoPath, string remote = "origin", string branch = "master");

    /// <summary>
    /// Stage a file until you made a commit.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <param name="filePath">Path to file to be updated.</param>
    /// <returns><see langword="true"/> on success.</returns>
    bool Add(string repoPath, string filePath);

    /// <summary>
    /// Creates a new branch in the repository.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <param name="branchName">Name of the new branch.</param>
    /// <returns><see langword="true"/> on success.</returns>
    bool CreateBranch(string repoPath, string branchName);

    /// <summary>
    /// Switches to the specified branch.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <param name="branchName">Name of the branch to switch to.</param>
    /// <returns><see langword="true"/> on success.</returns>
    bool Checkout(string repoPath, string branchName);

    /// <summary>
    /// Gets the current branch name.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <returns>The name of the current branch.</returns>
    string GetCurrentBranch(string repoPath);

    /// <summary>
    /// Gets the status of the repository.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <returns>A string containing the status information.</returns>
    string GetStatus(string repoPath);

    /// <summary>
    /// Stashes changes in the working directory.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <param name="message">Optional stash message.</param>
    /// <returns><see langword="true"/> on success.</returns>
    bool Stash(string repoPath, string? message = null);

    /// <summary>
    /// Applies the most recent stash.
    /// </summary>
    /// <param name="repoPath">Path to local repository.</param>
    /// <returns><see langword="true"/> on success.</returns>
    bool StashPop(string repoPath);
}
