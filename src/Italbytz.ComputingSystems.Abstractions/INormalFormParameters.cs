namespace Italbytz.ComputingSystems.Abstractions;

public interface INormalFormParameters
{
    int VariableCount { get; set; }
    int[] TruthValues { get; set; }
}