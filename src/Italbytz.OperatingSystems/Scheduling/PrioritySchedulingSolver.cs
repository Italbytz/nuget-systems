using System;
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
            foreach (var priority in new string[] { "sehr hoch", "hoch", "mittel", "niedrig", "sehr niedrig" })
            {
                var index = Array.FindIndex(parameters.Priorities, e => e == priority);
                result += (double)remainingProcesses * (double)parameters.Values[index] / (double)processes;
                remainingProcesses--;
            }
            return new SchedulingSolution()
            {
                Time = result
            };
        }
    }
}
