using System.Collections.Generic;
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
            Dictionary<string, string> envVars = null;

            if (depth.HasValue)
            {
                argsBuilder.Append($" --depth={depth.Value}");
            }

            argsBuilder.Append(" " + _pathToWorkspace);

            if (repoInfo.SshDeployKeyPath != null)
            {
                var runner = $"ssh -i {repoInfo.SshDeployKeyPath}";
                envVars = new Dictionary<string, string> {{"GIT_SSH_COMMAND", runner}};
            }

            var result = _processRunner.Run(_pathToGit, argsBuilder.ToString(), envVars);
            return result.ExitCode == 0;
        }
    }

    public class RepositoryInfo
    {
        public string Uri { get; set; }
        public string SshDeployKeyPath { get; set; }
    }
}
