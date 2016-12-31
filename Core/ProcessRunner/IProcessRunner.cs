using System.Collections.Generic;

namespace Core.ProcessRunner
{
    public interface IProcessRunner
    {
        ProcessRunInfo Run(string process, string args, Dictionary<string, string> env = null);
    }
}