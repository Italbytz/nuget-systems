using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class AlgebraicMinimizationParameters(int variableCount, int[] truthValues) : IAlgebraicMinimizationParameters
{
    public int VariableCount { get; set; } = variableCount;
    public int[] TruthValues { get; set; } = truthValues;
}
