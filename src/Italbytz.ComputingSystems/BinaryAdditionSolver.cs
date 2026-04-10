using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class BinaryAdditionSolver : IBinaryAdditionSolver
{

    public IBinaryAdditionSolution Solve(IBinaryAdditionParameters parameters)
    {
        var solution = new BinaryAdditionSolution
        {
            Sum = (ushort)(parameters.Summand1 + parameters.Summand2)
        };
        return solution;
    }
}