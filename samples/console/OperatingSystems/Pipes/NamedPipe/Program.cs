namespace NamedPipe;

using System.IO.Pipes;

internal static class Program
{
    private const string PipeName = "ItalbytzSystemsNamedPipe";

    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== FIFO / Benannte Pipe (Named Pipe) in C# ===");

        using var cts = new CancellationTokenSource();

        // Server im Hintergrund starten
        var serverTask = Task.Run(async () =>
        {
            try
            {
                using var server = new NamedPipeServerStream(
                    PipeName,
                    PipeDirection.In,
                    1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous);

                Console.WriteLine("[Server] Warte auf Verbindung eines Clients...");
                await server.WaitForConnectionAsync(cts.Token);
                Console.WriteLine("[Server] Client verbunden!");

                using var reader = new StreamReader(server);
                while (!cts.Token.IsCancellationRequested)
                {
                    var line = await reader.ReadLineAsync(cts.Token);
                    if (line == null) break;
                    Console.WriteLine($"[Server] Nachricht empfangen: '{line}'");
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Server Fehler] {ex.Message}");
            }
        });

        // Kurze Pause, damit der Server bereitsteht
        await Task.Delay(300);

        // Client starten und Testnachrichten senden
        try
        {
            using var client = new NamedPipeClientStream(".", PipeName, PipeDirection.Out, PipeOptions.Asynchronous);
            Console.WriteLine("[Client] Verbinde mit benannter Pipe...");
            await client.ConnectAsync(3000, cts.Token);
            Console.WriteLine("[Client] Verbunden!");

            using var writer = new StreamWriter(client) { AutoFlush = true };

            var messages = new[]
            {
                "Hallo aus dem Client!",
                "Interprozesskommunikation über Named Pipe",
                "End of transmission"
            };

            foreach (var msg in messages)
            {
                Console.WriteLine($"[Client] Sende: '{msg}'");
                await writer.WriteLineAsync(msg);
                await Task.Delay(200);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[Client Fehler] {ex.Message}");
        }

        cts.CancelAfter(500);
        await Task.WhenAny(serverTask, Task.Delay(1000));
        Console.WriteLine("=== Übertragung beendet ===");
    }
}
