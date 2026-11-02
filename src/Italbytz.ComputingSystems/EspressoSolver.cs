using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class EspressoSolver : IEspressoSolver
{
    public IEspressoSolution Solve(IEspressoParameters parameters)
    {
        return EspressoEngine.Solve(parameters.VariableCount, parameters.TruthValues);
    }
}

