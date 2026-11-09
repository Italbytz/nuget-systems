using System.Net.Sockets;

namespace TcpClient;

internal static class Program
{
    private const string ServerAddress = "127.0.0.1";
    private const int Port = 2055;

    public static async Task Main(string[] args)
    {
        Console.WriteLine($"Verbinde zu Server {ServerAddress}:{Port}...");

        try
        {
            using var client = new System.Net.Sockets.TcpClient();
            await client.ConnectAsync(ServerAddress, Port);
            Console.WriteLine("Erfolgreich verbunden!\n");

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream);
            await using var writer = new StreamWriter(stream) { AutoFlush = true };

            // Begrüßung des Servers empfangen
            var greeting = await reader.ReadLineAsync();
            Console.WriteLine(greeting);

            while (true)
            {
                Console.Write("Name: ");
                var name = Console.ReadLine();

                await writer.WriteLineAsync(name ?? string.Empty);

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Beende Verbindung...");
                    break;
                }

                var job = await reader.ReadLineAsync();
                Console.WriteLine($"Job: {job}");
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"[!] Socket-Fehler: {ex.Message}");
            Console.WriteLine("Stellen Sie sicher, dass der TcpServer gestartet ist.");
        }
    }
}
