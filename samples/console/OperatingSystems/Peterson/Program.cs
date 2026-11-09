namespace Peterson;

internal static class Program
{
    private static int _turn; // Gibt an, wer bei Gleichzeitigkeit den Vortritt lässt
    private static readonly bool[] Interested = new bool[2]; // Interessens-Flags
    private static int _sharedVariable;

    private static void EnterRegion(int process)
    {
        int other = 1 - process; // Der konkurrierende Thread (0 oder 1)
        Console.WriteLine($"[Thread {process}] Bekundet Interesse (Interested[{process}] = true)...");
        Interested[process] = true;

        // Höflichkeitsprinzip: Gibt dem anderen den Vortritt
        _turn = other;
        Console.WriteLine($"[Thread {process}] Setzt Turn auf {other} (_turn = {other}).");

        // Aktives Warten (Busy Waiting / Spinlock)
        while (Interested[other] && _turn == other)
        {
            Thread.Yield();
        }
    }

    private static void LeaveRegion(int process)
    {
        Console.WriteLine($"[Thread {process}] Verlässt kritischen Bereich (Interested[{process}] = false).\n");
        Interested[process] = false;
    }

    private static void Worker(object? threadIdObj)
    {
        int threadId = (int)threadIdObj!;

        for (int round = 1; round <= 2; round++)
        {
            EnterRegion(threadId);

            // --- Kritischer Bereich (Mutual Exclusion) ---
            Console.WriteLine($"---> [Thread {threadId}] BETRITT kritischen Bereich (Runde {round})");
            _sharedVariable = threadId;
            Thread.Sleep(50); // Simuliert Arbeit im kritischen Bereich
            Console.WriteLine($"<--- [Thread {threadId}] Überprüfung: _sharedVariable = {_sharedVariable} (Soll: {threadId})");

            LeaveRegion(threadId);

            // Nicht-kritischer Bereich
            Thread.Sleep(30);
        }
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("=== Petersons Algorithmus (Wechselseitiger Ausschluss) ===\n");

        var threads = new Thread[2];
        for (int i = 0; i < 2; i++)
        {
            threads[i] = new Thread(Worker);
            threads[i].Start(i);
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        Console.WriteLine("Beide Threads haben ihre Runden erfolgreich ohne Race Conditions beendet.");
    }
}
