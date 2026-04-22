using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class NormalFormParameters(int variableCount, int[] truthValues) : INormalFormParameters
{
    public int VariableCount { get; set; } = variableCount;

    public int[] TruthValues { get; set; } = truthValues;
}