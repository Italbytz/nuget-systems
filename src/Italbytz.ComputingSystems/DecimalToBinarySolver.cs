using System;
using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class DecimalToBinarySolver : IDecimalToBinarySolver
{
    public IDecimalToBinarySolution Solve(IDecimalToBinaryParameters parameters)
    {
        var steps = new List<string>();
        var current = (int)parameters.Decimal;

        if (current == 0)
        {
            steps.Add("Input is 0, therefore binary representation is 0.");
            return new DecimalToBinarySolution
            {
                Binary = 0,
                Steps = steps
            };
        }

        var digits = new List<char>();
        while (current > 0)
        {
            var quotient = current / 2;
            var remainder = current % 2;
            digits.Add((char)('0' + remainder));
            steps.Add($"{current} / 2 = {quotient}, remainder {remainder}.");
            current = quotient;
        }

        digits.Reverse();
        var binaryString = new string(digits.ToArray());

        var solution = new DecimalToBinarySolution
        {
            Binary = Convert.ToInt32(binaryString),
            Steps = steps
        };
        return solution;
    }
}