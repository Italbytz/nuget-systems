namespace MyProcessSample;

internal static class Program
{
    private static void Main(string[] args)
    {
        var myProcess = new MyProcess();
        myProcess.BindToRunningProcesses();
    }
}
