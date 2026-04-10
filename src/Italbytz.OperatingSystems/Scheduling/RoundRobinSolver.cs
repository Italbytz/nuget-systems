using System;
using System.Linq;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class RoundRobinSolver : ISchedulingSolver
    {
        public RoundRobinSolver()
        {
        }

        public ISchedulingSolution Solve(ISchedulingParameters parameters)
        {
            var orderedValues = parameters.Values.OrderBy(s => s);
            var result = 0.0;
            var processes = parameters.Values.Length;
            var remainingProcesses = processes;
            var sharedTime = 0;
            foreach (var value in orderedValues)
            {
                result += (double)remainingProcesses * ((double)value - (double)sharedTime) / (double)processes * (double)remainingProcesses;
                sharedTime = value;
                remainingProcesses--;
            }
            return new SchedulingSolution()
            {
                Time = result
            };
        }
    }
}
