using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class KarnaughMapParameters(int variableCount, int[] truthValues) : IKarnaughMapParameters
{
    public int VariableCount { get; set; } = variableCount;

    public int[] TruthValues { get; set; } = truthValues;
}