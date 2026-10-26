namespace Italbytz.ComputingSystems.Abstractions;

public interface IAlgebraicMinimizationParameters
{
    int VariableCount { get; set; }
    int[] TruthValues { get; set; }
}
