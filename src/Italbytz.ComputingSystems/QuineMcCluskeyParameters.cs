using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class QuineMcCluskeyParameters(int variableCount, int[] truthValues) : IQuineMcCluskeyParameters
{
    public int VariableCount { get; set; } = variableCount;

    public int[] TruthValues { get; set; } = truthValues;
}