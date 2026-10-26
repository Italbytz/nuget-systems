namespace Italbytz.ComputingSystems.Abstractions;

public class IdempotenceUsage
{
    public int MintermIndex { get; set; }
    public string MintermName { get; set; } = string.Empty;
    public int[] PairedWithMinterms { get; set; } = [];
    public string Explanation { get; set; } = string.Empty;
}
