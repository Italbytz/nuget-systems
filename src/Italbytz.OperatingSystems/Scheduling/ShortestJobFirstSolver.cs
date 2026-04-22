using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class ShortestJobFirstSolver : ISchedulingSolver
    {
        public ShortestJobFirstSolver()
        {
        }

        public ISchedulingSolution Solve(ISchedulingParameters parameters)
        {
            var orderedValues = parameters.Values.OrderBy(s => s).ToArray();
            var result = 0.0;
            var processes = parameters.Values.Length;
            var remainingProcesses = processes;
            var steps = new List<string>();
            steps.Add($"SJF order: {string.Join(", ", orderedValues)}.");
            foreach (var value in orderedValues)
            {
                var contribution = (double)remainingProcesses * (double)value / (double)processes;
                result += contribution;
                steps.Add($"Remaining={remainingProcesses}, burst={value} -> contribution {contribution:0.###}, running score {result:0.###}.");
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
