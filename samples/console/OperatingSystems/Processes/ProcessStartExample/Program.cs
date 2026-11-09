using System.Runtime.InteropServices;

namespace ProcessStartExample;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Process.Start() Demo ===");
        var manager = new ProcessManager();

        // 1. Cross-platform command
        string command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "uname";
        string arguments = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "/c ver" : "-a";

        Console.WriteLine($"\n[1] Starte Befehl mit E/A-Umleitung: {command} {arguments}");
        var (exitCode, output, error) = await manager.RunWithOutputAsync(command, arguments);
        Console.WriteLine($"ExitCode: {exitCode}");
        Console.WriteLine($"StandardOutput:\n{output.Trim()}");
        if (!string.IsNullOrEmpty(error))
        {
            Console.WriteLine($"StandardError:\n{error.Trim()}");
        }

        // 2. dotnet --version
        Console.WriteLine($"\n[2] Starte .NET CLI: dotnet --info");
        var dotnetResult = await manager.RunWithOutputAsync("dotnet", "--version");
        Console.WriteLine($"ExitCode: {dotnetResult.ExitCode}");
        Console.WriteLine($".NET Version: {dotnetResult.Output.Trim()}");
    }
}
