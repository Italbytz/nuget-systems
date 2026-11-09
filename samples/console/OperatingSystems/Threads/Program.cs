namespace Threads;

internal static class Program
{
    private const int NumberOfThreads = 10;

    private static void StaticMethod(object? threadId)
    {
        Console.WriteLine($"Hello World. Greetings from thread {threadId}");
        Thread.Sleep(1000);
        Console.WriteLine($"Leaving thread {threadId}");
    }

    public static void Main(string[] args)
    {
        for (var i = 0; i < NumberOfThreads; i++)
        {
            Console.WriteLine($"Main here. Creating thread {i}");
            var newThread = new Thread(StaticMethod);
            newThread.Start(i);
        }
    }
}
