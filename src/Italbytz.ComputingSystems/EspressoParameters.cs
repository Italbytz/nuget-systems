using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class EspressoParameters(int variableCount, int[] truthValues) : IEspressoParameters
{
    public int VariableCount { get; set; } = variableCount;
    public int[] TruthValues { get; set; } = truthValues;
}

