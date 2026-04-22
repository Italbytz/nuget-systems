namespace Italbytz.ComputingSystems.Abstractions;

public interface IQuineMcCluskeyParameters
{
    int VariableCount { get; set; }
    int[] TruthValues { get; set; }
}