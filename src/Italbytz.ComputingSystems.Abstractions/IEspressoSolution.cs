using System.Collections.Generic;
using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public interface IEspressoSolution : ITracedSolution
{
    string MinimalDnf { get; set; }
    string[] FinalCover { get; set; }
    int IterationCount { get; set; }
    int InitialCubeCount { get; set; }
    int FinalCubeCount { get; set; }
    int InitialLiteralCount { get; set; }
    int FinalLiteralCount { get; set; }
    List<IEspressoStep> DetailedSteps { get; set; }
}

