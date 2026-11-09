namespace ProducerConsumerMutex;

internal static class Program
{
    private const int Steps = 1000;
    private const int Capacity = 100;
    private static int _count = 50;
    private static int _step;

    private static readonly Mutex Mut = new Mutex();

    private static readonly AutoResetEvent Production = new AutoResetEvent(false);
    private static readonly AutoResetEvent Consumption = new AutoResetEvent(false);

    private static void Producer()
    {
        while (_step < Steps - 1)
        {
            if (!Mut.WaitOne()) continue;

            if (_count < Capacity)
            {
                Console.WriteLine($"Step {++_step}: Producing one item, {++_count} items in buffer.");
                Console.Out.Flush();
                Production.Set();
                Mut.ReleaseMutex();
            }
            else
            {
                Mut.ReleaseMutex();
                Consumption.WaitOne();
            }
        }
    }

    private static void Consumer()
    {
        while (_step < Steps - 1)
        {
            if (!Mut.WaitOne()) continue;

            if (_count > 0)
            {
                Console.WriteLine($"Step {++_step}: Consuming one item, {--_count} items in buffer.");
                Console.Out.Flush();
                Consumption.Set();
                Mut.ReleaseMutex();
            }
            else
            {
                Mut.ReleaseMutex();
                Production.WaitOne();
            }
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

