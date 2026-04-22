using System;
using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class TwosComplementSolver : ITwosComplementSolver
{
    public ITwosComplementSolution Solve(ITwosComplementParameters parameters)
    {
        var input = parameters.PositiveBinary;
        var inverted = (byte)~input;
        var plusOne = (byte)(inverted + 1);

        var steps = new List<string>
        {
            $"Input bits: {Convert.ToString(input, 2).PadLeft(8, '0')} ({input}).",
            $"Invert bits: {Convert.ToString(inverted, 2).PadLeft(8, '0')}.",
            $"Add one: {Convert.ToString(plusOne, 2).PadLeft(8, '0')} -> signed value {unchecked((sbyte)plusOne)}."
        };

        var solution = new TwosComplementSolution
        {
            ComplementBinary = unchecked((sbyte)plusOne),
            Steps = steps
        };
        return solution;
    }
}