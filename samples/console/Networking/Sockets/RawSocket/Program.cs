using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RawSocket;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        string host = args.Length > 0 ? args[0] : "www.google.de";
        int port = 80;

        Console.WriteLine($"=== Low-Level Socket Demo ===");
        Console.WriteLine($"Ermittle IP-Adressen für {host} (DNS)...");

        try
        {
            var hostEntry = await Dns.GetHostEntryAsync(host);
            Socket? socket = null;

            foreach (var address in hostEntry.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    var endpoint = new IPEndPoint(address, port);
                    var s = new Socket(endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    Console.WriteLine($"Verbinde zu {endpoint} (POSIX socket() + connect())...");
                    await s.ConnectAsync(endpoint);

                    if (s.Connected)
                    {
                        socket = s;
                        Console.WriteLine("Socket erfolgreich verbunden!\n");
                        break;
                    }
                }
            }

            if (socket == null)
            {
                Console.WriteLine("Keine Verbindung möglich.");
                return;
            }

            using (socket)
            {
                // HTTP GET Request
                string request = $"GET / HTTP/1.1\r\nHost: {host}\r\nConnection: close\r\n\r\n";
                byte[] requestBytes = Encoding.ASCII.GetBytes(request);

                Console.WriteLine($"Sende {requestBytes.Length} Bytes (POSIX send())...");
                await socket.SendAsync(requestBytes, SocketFlags.None);

                Console.WriteLine("Empfange Serverantwort (POSIX recv())...\n");
                byte[] buffer = new byte[1024];
                var responseBuilder = new StringBuilder();

                int bytesRead;
                while ((bytesRead = await socket.ReceiveAsync(buffer, SocketFlags.None)) > 0)
                {
                    responseBuilder.Append(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                    if (responseBuilder.Length > 2000)
                    {
                        // Nur die Header und ersten Zeilen für Demozwecke
                        break;
                    }
                }

                Console.WriteLine("--- HTTP Response (Auszug) ---");
                var lines = responseBuilder.ToString().Split("\r\n");
                foreach (var line in lines.Take(12))
                {
                    Console.WriteLine(line);
                }

                Console.WriteLine("\nSchließe Socket (POSIX close()).");
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Fehler: {ex.Message}");
        }
    }
}
