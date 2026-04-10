using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.OperatingSystems.PageReplacement;
using Italbytz.OperatingSystems.Resources.PageReplacement;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class FIFOSolver : IPageReplacementSolver
    {
        public IPageReplacementSolution Solve(IPageReplacementParameters parameters)
        {
            var simResult = new Fifo(parameters.ReferenceRequests, parameters.MemorySize).Simulate();
            var steps = simResult.Select(sim => sim.ToStep()).ToList();
            return new PageReplacementSolution()
            {
                Steps = steps
            };

        }
    }
}
