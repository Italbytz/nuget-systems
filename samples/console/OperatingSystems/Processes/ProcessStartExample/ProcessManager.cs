using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ProcessStartExample;

public class ProcessManager
{
    public Process StartProcess(string command, string args, bool showWindow = false, bool redirectOutput = false)
    {
        var psi = new ProcessStartInfo(command, args)
        {
            RedirectStandardOutput = redirectOutput,
            RedirectStandardError = redirectOutput,
            UseShellExecute = !redirectOutput && showWindow,
            CreateNoWindow = !showWindow
        };

        var process = new Process { StartInfo = psi };
        process.Start();
        return process;
    }

    public async Task<(int ExitCode, string Output, string Error)> RunWithOutputAsync(string command, string args)
    {
        var psi = new ProcessStartInfo(command, args)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await process.WaitForExitAsync();

        var output = await outputTask;
        var error = await errorTask;

        return (process.ExitCode, output, error);
    }
}
