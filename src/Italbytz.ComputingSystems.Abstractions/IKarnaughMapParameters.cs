namespace Italbytz.ComputingSystems.Abstractions;

public interface IKarnaughMapParameters
{
    int VariableCount { get; set; }
    int[] TruthValues { get; set; }
}