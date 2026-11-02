using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class EspressoSolution : IEspressoSolution
{
    public string MinimalDnf { get; set; } = string.Empty;
    public string[] FinalCover { get; set; } = [];
    public int IterationCount { get; set; }
    public int InitialCubeCount { get; set; }
    public int FinalCubeCount { get; set; }
    public int InitialLiteralCount { get; set; }
    public int FinalLiteralCount { get; set; }
    public List<IEspressoStep> DetailedSteps { get; set; } = [];
    public List<string> Steps { get; set; } = [];
}

