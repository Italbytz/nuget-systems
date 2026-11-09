namespace ProducerConsumerBare;

internal static class Program
{
    private const int Steps = 1000;
    private const int Capacity = 100;
    private static int _count = 50;
    private static int _step;

    private static void Producer()
    {
        while (_step < Steps)
        {
            if (_count >= Capacity) continue;
            Console.WriteLine($"Step {++_step}: Producing one item, {++_count} items in buffer.");
            Console.Out.Flush();
        }
    }

    private static void Consumer()
    {
        while (_step < Steps)
        {
            if (_count <= 0) continue;
            Console.WriteLine($"Step {++_step}: Consuming one item, {--_count} items in buffer.");
            Console.Out.Flush();
        }
    }

    public static void Main(string[] args)
    {
        var producer = new Thread(Producer);
        var consumer = new Thread(Consumer);
        producer.Start();
        consumer.Start();
        producer.Join();
        consumer.Join();
    }
}

