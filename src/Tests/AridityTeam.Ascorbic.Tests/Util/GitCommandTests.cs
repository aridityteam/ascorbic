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
using System.IO;

using AridityTeam.Util;

using Moq;

namespace AridityTeam.Platform.Tests.Util;

[Collection("GitTests")] // This ensures tests run sequentially
public class GitCommandTests : IDisposable
{
    private readonly string _testRepoPath;
    private readonly Git _git;
    private const string TestBranchName = "test-branch";
    private readonly Mock<IGitConfiguration> _mockConfig;
    private const string GIT_REPO_URL = "https://github.com/AasishPokhrel/shit.git"; // use the 1Bth repo on GitHub

    public GitCommandTests()
    {
        _testRepoPath = Path.Combine(Path.GetTempPath(), $"git-test-repo-{Guid.NewGuid()}");
        if (Directory.Exists(_testRepoPath))
        {
            try
            {
                Directory.Delete(_testRepoPath, true);
            }
            catch (UnauthorizedAccessException)
            {
                // If we can't delete it, try to use a different directory
                _testRepoPath = Path.Combine(Path.GetTempPath(), $"git-test-repo-{Guid.NewGuid()}-{DateTime.Now.Ticks}");
            }
        }
        Directory.CreateDirectory(_testRepoPath);

        _mockConfig = new Mock<IGitConfiguration>();
        _mockConfig.Setup(x => x.GitExecutablePath).Returns(DefaultGitConfiguration.FindGitExecutable());
        _mockConfig.Setup(x => x.UseShellExecution).Returns(false);

        _git = new Git(_mockConfig.Object);
        _git.Clone(new Uri(GIT_REPO_URL), _testRepoPath, 1);
    }

    [Fact]
    public void GitExecutable_IsInPath()
    {
        Assert.True(DefaultGitConfiguration.IsGitInPath(), "Git executable not found in PATH");
    }

    [Fact]
    public void GitExecutable_CanBeFound()
    {
        var gitPath = DefaultGitConfiguration.FindGitExecutable();
        Assert.NotNull(gitPath);
        Assert.True(File.Exists(gitPath) || gitPath.Equals("git", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Clone_ValidRepository_Success()
    {
        var repoPath = Path.Combine(Path.GetTempPath(), $"git-test-repo-clone-{Guid.NewGuid()}");
        if (Directory.Exists(repoPath))
        {
            Directory.Delete(repoPath, true);
        }
        Directory.CreateDirectory(repoPath);

        var result = _git.Clone(new Uri(GIT_REPO_URL), repoPath, 1);
        Assert.True(result);
        Assert.True(Directory.Exists(Path.Combine(repoPath, ".git")));

        try
        {
            Directory.Delete(repoPath, true);
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    [Fact]
    public void Add_ValidFile_Success()
    {
        var testFile = Path.Combine(_testRepoPath, "test.txt");
        File.WriteAllText(testFile, "test content");
        var result = _git.Add(_testRepoPath, "test.txt");
        Assert.True(result);
    }

    [Fact]
    public void Commit_ValidChanges_Success()
    {
        var testFile = Path.Combine(_testRepoPath, "test.txt");
        File.WriteAllText(testFile, "test content");
        _git.Add(_testRepoPath, "test.txt");
        var result = _git.Commit(_testRepoPath, "test commit");
        Assert.True(result);
    }

    [Fact]
    public void CreateBranch_ValidName_Success()
    {
        var result = _git.CreateBranch(_testRepoPath, TestBranchName);
        Assert.True(result);
        
        var branches = _git.ExecuteGitCommandWithOutput("branch", _testRepoPath);
        Assert.Contains(TestBranchName, branches);
    }

    [Fact]
    public void Checkout_ValidBranch_Success()
    {
        _git.CreateBranch(_testRepoPath, TestBranchName);
        var result = _git.Checkout(_testRepoPath, TestBranchName);
        Assert.True(result);
        var currentBranch = _git.GetCurrentBranch(_testRepoPath);
        Assert.Equal(TestBranchName, currentBranch);
    }

    [Fact]
    public void GetStatus_ReturnsValidStatus()
    {
        var status = _git.GetStatus(_testRepoPath);
        Assert.NotNull(status);
        Assert.Contains("On branch", status);
    }

    [Fact]
    public void Stash_ValidChanges_Success()
    {
        var testFile = Path.Combine(_testRepoPath, "test.txt");
        File.WriteAllText(testFile, "test content");
        _git.Add(_testRepoPath, "test.txt");
        var result = _git.Stash(_testRepoPath);
        Assert.True(result);
    }

    [Fact]
    public void StashPop_ValidStash_Success()
    {
        var testFile = Path.Combine(_testRepoPath, "test.txt");
        File.WriteAllText(testFile, "test content");
        _git.Add(_testRepoPath, "test.txt");
        _git.Stash(_testRepoPath);
        var result = _git.StashPop(_testRepoPath);
        Assert.True(result);
    }

    [Fact]
    public void Pull_ValidRepository_Success()
    {
        // the "shit" repository's main branch is named "main".
        var result = _git.Pull(_testRepoPath, "origin", "main");
        Assert.True(result);
    }

    [Fact]
    public void Push_ValidRepository_Success()
    {
        // Create a local repository for push testing
        var localRepoPath = Path.Combine(Path.GetTempPath(), $"git-test-repo-push-{Guid.NewGuid()}");
        Directory.CreateDirectory(localRepoPath);
        
        try
        {
            _git.ExecuteGitCommand("init", localRepoPath);
            
            var testFile = Path.Combine(localRepoPath, "test.txt");
            File.WriteAllText(testFile, "test content");
            _git.Add(localRepoPath, "test.txt");
            _git.Commit(localRepoPath, "test commit");
            
            _git.ExecuteGitCommand($"remote add origin {_testRepoPath}", localRepoPath);
            
            var result = _git.Push(localRepoPath, "origin", "master");
            Assert.True(result);
        }
        finally
        {
            try
            {
                Directory.Delete(localRepoPath, true);
            }
            catch (UnauthorizedAccessException)
            {
            }
        }
    }

    [Fact]
    public void GetCurrentBranch_ReturnsValidBranch()
    {
        var branch = _git.GetCurrentBranch(_testRepoPath);
        Assert.NotNull(branch);
        Assert.NotEmpty(branch);
    }

    public void Dispose()
    {
        try
        {
            if (Directory.Exists(_testRepoPath))
            {
                foreach (var file in Directory.GetFiles(_testRepoPath, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                    }
                    catch
                    {
                    }
                }

                Directory.Delete(_testRepoPath, true);
            }
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}
