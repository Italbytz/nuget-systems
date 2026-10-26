using System.Collections.Generic;
using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public interface IAlgebraicMinimizationSolution : ITracedSolution
{
    string Kdnf { get; set; }
    string MinimalDnf { get; set; }
    int[] MintermIndices { get; set; }
    List<AlgebraicPairing> FirstOrderPairings { get; set; }
    List<IdempotenceUsage> IdempotenceUsages { get; set; }
    List<AlgebraicPairing> SecondOrderPairings { get; set; }
    int InitialLiteralCount { get; set; }
    int MinimalLiteralCount { get; set; }
    int InitialGateCount { get; set; }
    int MinimalGateCount { get; set; }
}
