namespace DiningPhilosophersBare;

internal static class Program
{
    private const int NumPhilosophers = 5;

    private enum State
    {
        Think,
        Wait,
        Eat
    }

    private static readonly State[] PhilosopherStates = new State[NumPhilosophers];
    private static readonly int[] Forks = new int[NumPhilosophers];
    private static readonly object TableLock = new();
    private static bool _useAsymmetricStrategy = false;

    private static void DoPhilosophy(object? obj)
    {
        if (obj is not int id) return;

        var firstFork = id;
        var secondFork = (id + 1) % NumPhilosophers;

        // In asymmetric (Linkshänder) strategy, the last philosopher picks up right fork first
        if (_useAsymmetricStrategy && id == NumPhilosophers - 1)
        {
            (firstFork, secondFork) = (secondFork, firstFork);
        }

        while (true)
        {
            // 1. Thinking
            lock (TableLock)
            {
                PhilosopherStates[id] = State.Think;
                Console.WriteLine($"Philosoph {id + 1} denkt nach.");
            }
            Thread.Sleep(Random.Shared.Next(100, 300));

            // 2. Waiting for first fork
            lock (TableLock)
            {
                PhilosopherStates[id] = State.Wait;
            }

            while (true)
            {
                lock (TableLock)
                {
                    if (Forks[firstFork] == 0)
                    {
                        Forks[firstFork] = id + 1;
                        Console.WriteLine($"Philosoph {id + 1} nimmt Gabel {firstFork + 1}.");
                        break;
                    }
                }
                Thread.Sleep(50);
            }

            // Artificial small pause to make deadlock virtually guaranteed in bare mode
            Thread.Sleep(Random.Shared.Next(50, 150));

            // 3. Waiting for second fork
            while (true)
            {
                lock (TableLock)
                {
                    if (Forks[secondFork] == 0)
                    {
                        Forks[secondFork] = id + 1;
                        Console.WriteLine($"Philosoph {id + 1} nimmt Gabel {secondFork + 1}.");
                        break;
                    }

                    // Check for circular wait deadlock in bare mode
                    if (!_useAsymmetricStrategy && Forks.All(f => f != 0) && PhilosopherStates.All(s => s == State.Wait))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"[DEADLOCK] Alle 5 Philosophen halten eine Gabel und warten zirkulär auf die nächste!");
                        Console.ResetColor();
                    }
                }
                Thread.Sleep(50);
            }

            // 4. Eating
            lock (TableLock)
            {
                PhilosopherStates[id] = State.Eat;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Philosoph {id + 1} isst Spaghetti mit Gabeln {firstFork + 1} und {secondFork + 1}.");
                Console.ResetColor();
            }
            Thread.Sleep(Random.Shared.Next(200, 400));

            // 5. Put down forks
            lock (TableLock)
            {
                Forks[firstFork] = 0;
                Forks[secondFork] = 0;
                Console.WriteLine($"Philosoph {id + 1} legt Gabeln {firstFork + 1} und {secondFork + 1} zurück.");
            }
        }
    }

    public static void Main(string[] args)
    {
        if (args.Length > 0 && args[0].Equals("--asymmetric", StringComparison.OrdinalIgnoreCase))
        {
            _useAsymmetricStrategy = true;
            Console.WriteLine("Starte Philosophenproblem mit ASYMMETRISCHER Strategie (Linkshänder P5, deadlock-frei)...");
        }
        else
        {
            Console.WriteLine("Starte Philosophenproblem im BARE-Modus (symmetrisch, Deadlock vorprogrammiert)...");
            Console.WriteLine("Tipp: Mit '--asymmetric' starten, um die Deadlock-freie Linkshänder-Strategie zu sehen.");
        }

        for (var i = 0; i < NumPhilosophers; i++)
        {
            PhilosopherStates[i] = State.Think;
            var thread = new Thread(DoPhilosophy)
            {
                IsBackground = true
            };
            thread.Start(i);
        }

        Console.WriteLine("Simulation läuft. Beliebige Taste zum Beenden drücken...");
        Console.ReadKey();
    }
}
