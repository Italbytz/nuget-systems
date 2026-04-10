using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class BinaryToDecimalSolver : IBinaryToDecimalSolver
{
    // Doing nothing, because for now it is possible to solve the problem in the transfer to the ViewModel
    public IBinaryToDecimalSolution Solve(IBinaryToDecimalParameters parameters)
    {
        var solution = new BinaryToDecimalSolution
        {
            Decimal = parameters.Binary
        };
        return solution;
    }
}