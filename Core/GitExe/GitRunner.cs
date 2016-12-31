using System.Text;
using Core.ProcessRunner;

namespace Core.GitExe
{
    public class GitRunner
    {
        private readonly IProcessRunner _processRunner;
        private readonly string _pathToGit;
        private readonly string _pathToWorkspace;

        public GitRunner(IProcessRunner processRunner, string pathToGit, string pathToWorkspace)
        {
            _processRunner = processRunner;
            _pathToGit = pathToGit;
            _pathToWorkspace = pathToWorkspace;
        }

        public bool Clone(RepositoryInfo repoInfo, int? depth = null)
        {
            var argsBuilder = new StringBuilder(repoInfo.Uri);

            if (depth.HasValue)
            {
                argsBuilder.Append($" --depth={depth.Value}");
            }

            argsBuilder.Append(" " + _pathToWorkspace);

            var result = _processRunner.Run(_pathToGit, argsBuilder.ToString());
            return result.ExitCode == 0;
        }
    }

    public class RepositoryInfo
    {
        public string Uri { get; set; }
        public string SshDeployKey { get; set; }
    }
}
