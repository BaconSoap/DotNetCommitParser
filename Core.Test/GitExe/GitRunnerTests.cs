using System.Collections.Generic;
using Core.GitExe;
using Core.ProcessRunner;
using FluentAssertions;
using Moq;
using Xunit;

namespace Core.Test.GitExe
{
    public class GitRunnerTests
    {
        private const string GitDotExe = "git.exe";
        private const string Workspace = "./testing";
        private const string SshDeployKeyPath = "./privateKey";

        [Fact]
        public void Clone_with_minimal_args_works()
        {
            var mock = GetMockRunner();
            var sut = new GitRunner(mock.Object, GitDotExe, Workspace);

            var result = sut.Clone(GetRepoInfo());

            result.Should().BeTrue();
        }

        [Fact]
        public void Clone_with_minimal_args_passes_info_to_proc()
        {
            var mock = GetMockRunner();
            var sut = new GitRunner(mock.Object, GitDotExe, Workspace);

            var repositoryInfo = GetRepoInfo();
            sut.Clone(repositoryInfo);

            mock.Verify(x => x.Run(GitDotExe, $"{repositoryInfo.Uri} {Workspace}", null));
        }

        [Fact]
        public void Clone_with_depth_passes_info_to_proc()
        {
            const int depth = 5;
            var mock = GetMockRunner();
            var sut = new GitRunner(mock.Object, GitDotExe, Workspace);

            var repositoryInfo = GetRepoInfo();
            var res = sut.Clone(repositoryInfo, depth);

            res.Should().BeTrue();
            mock.Verify(x => x.Run(GitDotExe, $"{repositoryInfo.Uri} --depth={depth} {Workspace}", null));
        }

        [Fact]
        public void Clone_with_SshDeployKeyPath_sets_env_variable()
        {
            var mock = GetMockRunner();
            var sut = new GitRunner(mock.Object, GitDotExe, Workspace);

            var repositoryInfo = GetRepoInfo();
            repositoryInfo.SshDeployKeyPath = SshDeployKeyPath;
            sut.Clone(repositoryInfo);

            var envVars = new Dictionary<string, string> {{"GIT_SSH_COMMAND", $"ssh -i {SshDeployKeyPath}"}};
            mock.Verify(x => x.Run(GitDotExe, $"{repositoryInfo.Uri} {Workspace}", envVars));
        }

        private RepositoryInfo GetRepoInfo()
        {
            return new RepositoryInfo
            {
                SshDeployKeyPath = null,
                Uri = "git@github.com:BaconSoap/DotNetCommitParser.git"
            };
        }

        private Mock<IProcessRunner> GetMockRunner()
        {
            var mock = new Mock<IProcessRunner>();
            mock.Setup(x => x.Run(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(() => new ProcessRunInfo {ExitCode = 0});
            return mock;
        }
    }
}
