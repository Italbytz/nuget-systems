using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class AlgebraicMinimizationSolution : IAlgebraicMinimizationSolution
{
    public string Kdnf { get; set; } = string.Empty;
    public string MinimalDnf { get; set; } = string.Empty;
    public int[] MintermIndices { get; set; } = [];
    public List<AlgebraicPairing> FirstOrderPairings { get; set; } = new();
    public List<IdempotenceUsage> IdempotenceUsages { get; set; } = new();
    public List<AlgebraicPairing> SecondOrderPairings { get; set; } = new();
    public int InitialLiteralCount { get; set; }
    public int MinimalLiteralCount { get; set; }
    public int InitialGateCount { get; set; }
    public int MinimalGateCount { get; set; }
    public List<string> Steps { get; set; } = new();
}
