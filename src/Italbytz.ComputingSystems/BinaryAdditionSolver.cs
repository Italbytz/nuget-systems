using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class BinaryAdditionSolver : IBinaryAdditionSolver
{

    public IBinaryAdditionSolution Solve(IBinaryAdditionParameters parameters)
    {
        var steps = new List<string>();
        ushort sum = 0;
        var carry = 0;

        for (var bit = 0; bit < 8; bit++)
        {
            var leftBit = (parameters.Summand1 >> bit) & 0x1;
            var rightBit = (parameters.Summand2 >> bit) & 0x1;
            var partial = leftBit + rightBit + carry;
            var resultBit = partial & 0x1;
            var carryOut = (partial >> 1) & 0x1;

            sum |= (ushort)(resultBit << bit);
            steps.Add($"Bit {bit}: {leftBit} + {rightBit} + carryIn {carry} = {partial} -> result {resultBit}, carryOut {carryOut}");
            carry = carryOut;
        }

        if (carry == 1)
        {
            sum |= (ushort)(1 << 8);
            steps.Add("Final carry produces result bit 8 = 1.");
        }

        var solution = new BinaryAdditionSolution
        {
            Sum = sum,
            Steps = steps
        };
        return solution;
    }
}