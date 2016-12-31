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

            mock.Verify(x => x.Run(GitDotExe, $"{repositoryInfo.Uri} {Workspace}"));
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
            mock.Verify(x => x.Run(GitDotExe, $"{repositoryInfo.Uri} --depth={depth} {Workspace}"));
        }

        private RepositoryInfo GetRepoInfo()
        {
            return new RepositoryInfo
            {
                SshDeployKey = null,
                Uri = "git@github.com:BaconSoap/DotNetCommitParser.git"
            };
        }

        private Mock<IProcessRunner> GetMockRunner()
        {
            var mock = new Mock<IProcessRunner>();
            mock.Setup(x => x.Run(It.IsAny<string>(), It.IsAny<string>())).Returns(() => new ProcessRunInfo {ExitCode = 0});
            return mock;
        }
    }
}
