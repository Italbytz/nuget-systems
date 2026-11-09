using System.Net;
using System.Net.Sockets;

namespace TcpServer;

internal static class Program
{
    private const int Port = 2055;
    private const int MaxConcurrentClients = 5;

    private static readonly Dictionary<string, string> Employees = new(StringComparer.OrdinalIgnoreCase)
    {
        { "jane", "manager" },
        { "jim", "clerk" },
        { "jack", "salesman" },
        { "john", "steno" }
    };

    public static async Task Main(string[] args)
    {
        var localAddr = IPAddress.Parse("127.0.0.1");
        var listener = new TcpListener(localAddr, Port);
        listener.Start();

        Console.WriteLine($"Server mounted, listening to port {Port}");
        Console.WriteLine($"Verfügbare Mitarbeiter: {string.Join(", ", Employees.Keys)}");
        Console.WriteLine("Drücken Sie Strg+C zum Beenden.\n");

        var semaphore = new SemaphoreSlim(MaxConcurrentClients, MaxConcurrentClients);

        while (true)
        {
            var tcpClient = await listener.AcceptTcpClientAsync();
            await semaphore.WaitAsync();

            _ = Task.Run(async () =>
            {
                var endpoint = tcpClient.Client.RemoteEndPoint;
                Console.WriteLine($"[+] Connected: {endpoint}");

                try
                {
                    using (tcpClient)
                    using (var stream = tcpClient.GetStream())
                    using (var reader = new StreamReader(stream))
                    await using (var writer = new StreamWriter(stream) { AutoFlush = true })
                    {
                        await writer.WriteLineAsync($"{Employees.Count} Employees available");

                        while (true)
                        {
                            var name = await reader.ReadLineAsync();
                            if (string.IsNullOrEmpty(name))
                            {
                                break;
                            }

                            Console.WriteLine($"[?] Anfrage von {endpoint}: '{name}'");
                            var response = Employees.TryGetValue(name, out var job) ? job : "No such employee";
                            await writer.WriteLineAsync(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[!] Fehler bei {endpoint}: {ex.Message}");
                }
                finally
                {
                    Console.WriteLine($"[-] Disconnected: {endpoint}");
                    semaphore.Release();
                }
            });
        }
    }
}
