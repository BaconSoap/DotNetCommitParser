namespace Core.ProcessRunner
{
    public interface IProcessRunner
    {
        ProcessRunInfo Run(string process, string args);
    }
}