using System.Collections.Generic;
using System.Diagnostics;

namespace Core.ProcessRunner
{
    public class ProcessRunner: IProcessRunner
    {
        public ProcessRunInfo Run(string process, string args, Dictionary<string, string> env = null)
        {
            var proc = new Process();
            proc.StartInfo.FileName = process;
            proc.StartInfo.Arguments = args;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            if (env != null)
            {
                foreach (var kvp in env)
                {
                    proc.StartInfo.EnvironmentVariables.Add(kvp.Key, kvp.Value);
                }
            }

            proc.Start();

            var stdOut = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            
            return new ProcessRunInfo
            {
                StdOut = stdOut,
                ExitCode = proc.ExitCode
            };
        }
    }
}
