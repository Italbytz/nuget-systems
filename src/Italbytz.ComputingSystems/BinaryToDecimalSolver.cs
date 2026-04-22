using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class BinaryToDecimalSolver : IBinaryToDecimalSolver
{
    public IBinaryToDecimalSolution Solve(IBinaryToDecimalParameters parameters)
    {
        var steps = new List<string>();
        var decimalValue = 0;

        for (var bit = 0; bit < 8; bit++)
        {
            var bitValue = (parameters.Binary >> bit) & 0x1;
            var contribution = bitValue == 1 ? (1 << bit) : 0;
            decimalValue += contribution;
            steps.Add($"Bit {bit} is {bitValue} -> contribution {contribution}, running sum {decimalValue}.");
        }

        var solution = new BinaryToDecimalSolution
        {
            Decimal = decimalValue,
            Steps = steps
        };
        return solution;
    }
}