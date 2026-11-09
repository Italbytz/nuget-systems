namespace NetMQExample;

using NetMQ;
using NetMQ.Sockets;

internal static class Program
{
    private const string ServerAddress = "@tcp://127.0.0.1:5556";
    private const string ClientAddress = ">tcp://127.0.0.1:5556";

    public static void Main(string[] args)
    {
        Console.WriteLine("=== ZeroMQ / NetMQ Message Queue Beispiel in C# ===");

        // ResponseSocket fungiert als Server (wartet auf Anfragen)
        using var server = new ResponseSocket(ServerAddress);

        // RequestSocket fungiert als Client (sendet Anfragen)
        using var client = new RequestSocket(ClientAddress);

        // 1. Nachricht vom Client senden
        const string clientMessage = "Hallo aus dem NetMQ-Client!";
        Console.WriteLine($"[Client] Sende Frame: '{clientMessage}'");
        client.SendFrame(clientMessage);

        // 2. Nachricht am Server empfangen
        var receivedByServer = server.ReceiveFrameString();
        Console.WriteLine($"[Server] Empfangen: '{receivedByServer}'");

        // 3. Antwort vom Server zurücksenden
        const string serverResponse = "Antwort vom NetMQ-Server: Nachricht erhalten!";
        Console.WriteLine($"[Server] Sende Antwort-Frame: '{serverResponse}'");
        server.SendFrame(serverResponse);

        // 4. Antwort am Client empfangen
        var receivedByClient = client.ReceiveFrameString();
        Console.WriteLine($"[Client] Empfangen: '{receivedByClient}'");

        Console.WriteLine("=== Kommunikation erfolgreich abgeschlossen ===");
        NetMQConfig.Cleanup(false);
    }
}
