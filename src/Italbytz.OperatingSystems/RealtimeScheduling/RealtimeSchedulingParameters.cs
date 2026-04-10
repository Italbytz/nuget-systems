using System;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class RealtimeSchedulingParameters : IRealtimeSchedulingParameters
    {
        public (int,int)[] Requests { get; set; }

        public RealtimeSchedulingParameters() 
        {
            var rnd = new Random();
            (int, int)[] configuration;
            int sum;
            do
            {
                configuration = new (int, int)[] { (rnd.Next(1, 5), rnd.Next(5, 15)), (rnd.Next(1, 5), rnd.Next(5, 15)), (rnd.Next(1, 5), rnd.Next(5, 15)) };
                sum = 0;
                foreach (var config in configuration)
                {
                    sum += (32 / config.Item2) * config.Item1;
                }

            } while (sum > 32 || sum < 25);

            Requests = configuration;
        }

    }
}
