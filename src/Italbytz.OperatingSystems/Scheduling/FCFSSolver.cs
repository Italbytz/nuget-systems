using System;
using System.Collections.Generic;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class FCFSSolver : ISchedulingSolver
    {
        public FCFSSolver()
        {
        }

        public ISchedulingSolution Solve(ISchedulingParameters parameters)
        {
            var result = 0.0;
            var processes = parameters.Values.Length;
            var remainingProcesses = processes;
            var steps = new List<string>();
            steps.Add("FCFS uses input order without reordering.");
            for (var index = 0; index < parameters.Values.Length; index++)
            {
                steps.Add($"Queue position {index + 1}: burst={parameters.Values[index]}.");
            }

            foreach (var value in parameters.Values)
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
