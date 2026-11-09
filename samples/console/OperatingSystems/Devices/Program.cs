using System.Runtime.InteropServices;
using System.Text;

namespace Devices;

internal static class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== UNIX Geräte-Dateien in C# (/dev/...) ===\n");

        bool isPosix = !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        if (!isPosix)
        {
            Console.WriteLine("[Hinweis] Windows nutzt keine UNIX-Gerätedateien unter /dev/.");
            Console.WriteLine("Unter Windows entsprechen CON, NUL und AUX den Gerätedateien.\n");
            return;
        }

        // 1. /dev/stdout schreiben (Everything is a file)
        Console.WriteLine("[1] Schreiben in Gerätedatei /dev/stdout:");
        using (var deviceWriter = new StreamWriter("/dev/stdout") { AutoFlush = true })
        {
            deviceWriter.WriteLine("  -> Diese Zeile wurde direkt in die Gerätedatei /dev/stdout geschrieben!");
        }

        // 2. /dev/null schreiben (Daten verschwinden spurlos)
        Console.WriteLine("\n[2] Schreiben in /dev/null (Bit-Bucket / Schwarzes Loch):");
        using (var nullWriter = new StreamWriter("/dev/null") { AutoFlush = true })
        {
            nullWriter.WriteLine("Dieser Text wird vom Betriebssystem sofort verworfen.");
        }
        Console.WriteLine("  -> Erfolgreich in /dev/null geschrieben (wird vom Kernel verworfen).");

        // 3. /dev/urandom lesen (Kernel-Zufallsgenerator)
        Console.WriteLine("\n[3] Lesen von 16 Zufallsbytes aus /dev/urandom:");
        if (File.Exists("/dev/urandom"))
        {
            using var randomStream = File.OpenRead("/dev/urandom");
            byte[] randomBytes = new byte[16];
            int read = randomStream.Read(randomBytes, 0, randomBytes.Length);
            Console.WriteLine($"  -> Gelesene Entropie ({read} Bytes Hex): {Convert.ToHexString(randomBytes)}");
        }

        // 4. Interaktive E/A über /dev/stdout und /dev/stdin
        Console.WriteLine("\n[4] Interaktive E/A über Geräte-Streams:");
        using var writer = new StreamWriter("/dev/stdout") { AutoFlush = true };
        using var reader = new StreamReader(Console.OpenStandardInput());

        writer.Write("Wie heißen Sie? ");
        string? name = reader.ReadLine();
        writer.WriteLine($"Hallo {name ?? "Gast"}! Willkommen in der Welt der UNIX-Gerätedateien.");
    }
}
