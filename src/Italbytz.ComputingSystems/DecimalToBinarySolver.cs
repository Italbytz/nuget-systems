using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class DecimalToBinarySolver : IDecimalToBinarySolver
{
    // Doing nothing, because for now it is possible to solve the problem in the transfer to the ViewModel
    public IDecimalToBinarySolution Solve(IDecimalToBinaryParameters parameters)
    {
        var solution = new DecimalToBinarySolution
        {
            Binary = parameters.Decimal
        };
        return solution;
    }
}