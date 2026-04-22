using System;
using System.Collections.Generic;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class PrioritySchedulingSolver : ISchedulingSolver
    {
        public PrioritySchedulingSolver()
        {
        }

        public ISchedulingSolution Solve(ISchedulingParameters parameters)
        {
            var result = 0.0;
            var processes = parameters.Values.Length;
            var remainingProcesses = processes;
            var steps = new List<string>();
            var priorityLadder = new string[] { "sehr hoch", "hoch", "mittel", "niedrig", "sehr niedrig" };
            steps.Add($"Priority ladder: {string.Join(" > ", priorityLadder)}.");
            foreach (var priority in priorityLadder)
            {
                var index = Array.FindIndex(parameters.Priorities, e => e == priority);
                if (index < 0)
                {
                    steps.Add($"Priority '{priority}' not present in input and is skipped.");
                    continue;
                }

                var burst = parameters.Values[index];
                var contribution = (double)remainingProcesses * (double)burst / (double)processes;
                result += contribution;
                steps.Add($"Priority '{priority}' maps to index {index} with burst={burst}; remaining={remainingProcesses} -> contribution {contribution:0.###}, running score {result:0.###}.");
                remainingProcesses--;
            }
            return new SchedulingSolution()
            {
                Time = result,
                Steps = steps
            };
        }
    }
}
