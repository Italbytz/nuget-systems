using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class RMSSolver : IRealtimeSchedulingSolver
    {
        public RMSSolver()
        {
        }

        public IRealtimeSchedulingSolution Solve(IRealtimeSchedulingParameters parameters)
        {
            var result = new int[32];
            var waiting = new List<(int, int)>();
            var steps = new List<string>();
            steps.Add("RMS uses fixed priorities by shorter period.");
            for (int i = 0; i < 32; i++)
            {
                var index = 0;
                foreach (var process in parameters.Requests)
                {
                    if (i % process.Item2 < process.Item1)
                    {
                        waiting.Add((index, process.Item2));
                    }
                    index++;
                }

                if (waiting.Count == 0)
                {
                    result[i] = -1;
                    steps.Add($"Slot {i}: no ready task -> idle.");
                }
                else
                {
                    var winner = waiting.OrderBy(e => e.Item2).ThenBy(e => e.GetHashCode()).First();
                    result[i] = winner.Item1;
                    waiting.Remove(winner);
                    steps.Add($"Slot {i}: run T{winner.Item1 + 1} (period {winner.Item2}), ready queue size after dispatch {waiting.Count}.");
                }

            }
            return new RealtimeSchedulingSolution()
            {
                Processes = result,
                Steps = steps
            };
        }
    }
}
