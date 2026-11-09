namespace AnonymousPipe;

using System.Diagnostics;
using System.IO.Pipes;

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Unbenannte Pipe (Anonymous Pipe) in C# ===");

        // Server-Stream (Schreibende Seite, erbt Handle an Kindprozess/Client)
        using var pipeServer = new AnonymousPipeServerStream(
            PipeDirection.Out,
            HandleInheritability.Inheritable);

        // Client-Stream (Lesende Seite, verbindet sich über das Server-SafePipeHandle)
        using var pipeClient = new AnonymousPipeClientStream(
            PipeDirection.In,
            pipeServer.ClientSafePipeHandle);

        try
        {
            using var sw = new StreamWriter(pipeServer);
            sw.AutoFlush = true;

            const string message = "Hallo Welt durch eine unbenannte Pipe!";
            Console.WriteLine($"[Writer/Server] Sende: '{message}'");
            sw.WriteLine(message);

            using var sr = new StreamReader(pipeClient);
            var received = sr.ReadLine();
            Console.WriteLine($"[Reader/Client] Empfangen: '{received}'");
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            Console.Error.WriteLine($"Fehler: {e.Message}");
        }
    }
}
