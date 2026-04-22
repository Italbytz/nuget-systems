using System;
using System.Collections.Generic;
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
            var orderedValues = parameters.Values.OrderBy(s => s).ToArray();
            var result = 0.0;
            var processes = parameters.Values.Length;
            var remainingProcesses = processes;
            var sharedTime = 0;
            var steps = new List<string>();
            steps.Add($"RoundRobin score uses ascending burst phases: {string.Join(", ", orderedValues)}.");
            foreach (var value in orderedValues)
            {
                var delta = (double)value - (double)sharedTime;
                var contribution = (double)remainingProcesses * delta / (double)processes * (double)remainingProcesses;
                result += contribution;
                steps.Add($"remaining={remainingProcesses}, phaseDelta={delta:0.###} -> contribution {contribution:0.###}, running score {result:0.###}.");
                sharedTime = value;
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
