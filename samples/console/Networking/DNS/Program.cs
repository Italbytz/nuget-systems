using System.Net;
using System.Net.Sockets;

namespace DNS;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        string host = args.Length > 0 ? args[0] : "www.google.de";

        Console.WriteLine("=== DNS-Namensauflösung in C# (System.Net.Dns) ===\n");
        Console.WriteLine($"Abzufragender Host: {host}");

        try
        {
            // 1. Forward Lookup: Hostname -> IP-Adressen (A- & AAAA-Records)
            var hostEntry = await Dns.GetHostEntryAsync(host);
            Console.WriteLine($"Kanonischer Name (HostName): {hostEntry.HostName}");

            if (hostEntry.Aliases.Length > 0)
            {
                Console.WriteLine($"Aliase (CNAME): {string.Join(", ", hostEntry.Aliases)}");
            }

            Console.WriteLine($"Gefundene IP-Adressen ({hostEntry.AddressList.Length}):\n");

            foreach (var curAdd in hostEntry.AddressList)
            {
                Console.WriteLine($"  - Adresse:       {curAdd}");
                Console.WriteLine($"    AddressFamily: {curAdd.AddressFamily} ({(curAdd.AddressFamily == AddressFamily.InterNetwork ? "IPv4 / A" : "IPv6 / AAAA")})");

                if (curAdd.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    Console.WriteLine($"    ScopeId:       {curAdd.ScopeId}");
                }

                Console.WriteLine();
            }

            // 2. Lokaler Hostname
            string localHostName = Dns.GetHostName();
            Console.WriteLine($"Eigener lokaler Rechnername: {localHostName}");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"[!] DNS-Fehler beim Auflösen von '{host}': {ex.Message}");
        }
    }
}
