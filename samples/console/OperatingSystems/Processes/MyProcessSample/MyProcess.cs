namespace MyProcessSample;

using System.Diagnostics;

public class MyProcess
{
    public void BindToRunningProcesses()
    {
        Console.WriteLine("=== Prozessinformationen in C# (System.Diagnostics.Process) ===");

        // 1. Laufender Prozess (entspricht POSIX getpid())
        var current = Process.GetCurrentProcess();
        Console.WriteLine($"Aktueller Prozess:   {current.ProcessName} (PID: {current.Id})");
        Console.WriteLine($"Prioritätsklasse:     {current.PriorityClass}");
        Console.WriteLine($"Arbeitsspeicher (WS): {current.WorkingSet64 / 1024 / 1024} MB");
        Console.WriteLine($"Anzahl Threads:       {current.Threads.Count}");
        try
        {
            Console.WriteLine($"Startzeit:            {current.StartTime:HH:mm:ss.fff}");
        }
        catch (Exception)
        {
            // Auf manchen Plattformen eingeschränkt
        }

        Console.WriteLine();

        // 2. Alle laufenden Prozesse auf dem Rechner (entspricht 'ps aux' oder 'tasklist')
        var localAll = Process.GetProcesses();
        Console.WriteLine($"Gesamtzahl laufender Prozesse: {localAll.Length}");

        // Top 5 Prozesse nach Arbeitsspeicher ausgeben
        Console.WriteLine("Top 5 Prozesse nach Speicherverbrauch:");
        foreach (var p in localAll.OrderByDescending(p => p.WorkingSet64).Take(5))
        {
            try
            {
                Console.WriteLine($"  - PID {p.Id,6}: {p.ProcessName,-25} ({p.WorkingSet64 / 1024 / 1024,5} MB)");
            }
            catch
            {
                // Manche Systemprozesse verweigern Abfrage
            }
        }

        Console.WriteLine();

        // 3. Prozesse nach Namen filtern
        var dotnetProcesses = Process.GetProcessesByName("dotnet");
        Console.WriteLine($"Laufende 'dotnet'-Prozesse: {dotnetProcesses.Length}");

        // 4. Prozess anhand der PID abrufen
        try
        {
            var initProcess = Process.GetProcessById(1);
            Console.WriteLine($"Prozess mit PID 1 (Init/Launchd/Systemd): {initProcess.ProcessName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Prozess mit PID 1 konnte nicht abgefragt werden: {ex.Message}");
        }
    }
}
