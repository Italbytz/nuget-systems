using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class TwosComplementSolver : ITwosComplementSolver
{
    public ITwosComplementSolution Solve(ITwosComplementParameters parameters)
    {
        var solution = new TwosComplementSolution
        {
            ComplementBinary = (sbyte)(-1*parameters.PositiveBinary)
        };
        return solution;
    }
}