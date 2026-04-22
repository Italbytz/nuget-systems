using System;
using System.Collections.Generic;
using System.Linq;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class NormalFormSolver : INormalFormSolver
{
    public INormalFormSolution Solve(INormalFormParameters parameters)
    {
        Validate(parameters);

        var steps = new List<string>();
        var dnfTerms = new List<string>();
        var cnfTerms = new List<string>();

        for (var row = 0; row < parameters.TruthValues.Length; row++)
        {
            var bits = ToBits(row, parameters.VariableCount);
            var value = parameters.TruthValues[row];
            steps.Add($"Row {row}: assignment {FormatAssignment(bits)} -> y={value}.");

            if (value == 1)
            {
                var minterm = BuildMinterm(bits);
                dnfTerms.Add(minterm);
                steps.Add($"Row {row} contributes minterm {minterm} to DNF.");
            }
            else
            {
                var maxterm = BuildMaxterm(bits);
                cnfTerms.Add(maxterm);
                steps.Add($"Row {row} contributes maxterm {maxterm} to CNF.");
            }
        }

        var dnf = dnfTerms.Count > 0 ? string.Join(" | ", dnfTerms) : "0";
        var cnf = cnfTerms.Count > 0 ? string.Join(" & ", cnfTerms) : "1";

        steps.Add($"Final DNF: {dnf}.");
        steps.Add($"Final CNF: {cnf}.");

        return new NormalFormSolution
        {
            Dnf = dnf,
            Cnf = cnf,
            Steps = steps
        };
    }

    private static void Validate(INormalFormParameters parameters)
    {
        if (parameters.VariableCount < 1 || parameters.VariableCount > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(parameters.VariableCount), "VariableCount must stay between 1 and 8.");
        }

        var expectedRowCount = 1 << parameters.VariableCount;
        if (parameters.TruthValues.Length != expectedRowCount)
        {
            throw new ArgumentException($"TruthValues must contain exactly {expectedRowCount} entries for {parameters.VariableCount} variables.", nameof(parameters.TruthValues));
        }

        if (parameters.TruthValues.Any(value => value is not (0 or 1)))
        {
            throw new ArgumentException("TruthValues may only contain 0 or 1 entries.", nameof(parameters.TruthValues));
        }
    }

    private static bool[] ToBits(int row, int variableCount)
    {
        var bits = new bool[variableCount];
        for (var bit = 0; bit < variableCount; bit++)
        {
            var shift = variableCount - bit - 1;
            bits[bit] = ((row >> shift) & 0x1) == 1;
        }

        return bits;
    }

    private static string FormatAssignment(IReadOnlyList<bool> bits) =>
        string.Join(", ", bits.Select((value, index) => $"x{bits.Count - index - 1}={(value ? 1 : 0)}"));

    private static string BuildMinterm(IReadOnlyList<bool> bits) =>
        "(" + string.Join(" & ", bits.Select((value, index) => value ? $"x{bits.Count - index - 1}" : $"!x{bits.Count - index - 1}")) + ")";

    private static string BuildMaxterm(IReadOnlyList<bool> bits) =>
        "(" + string.Join(" | ", bits.Select((value, index) => value ? $"!x{bits.Count - index - 1}" : $"x{bits.Count - index - 1}")) + ")";
}