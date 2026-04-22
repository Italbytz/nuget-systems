using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public interface ITwosComplementSolution : ITracedSolution
{
    sbyte ComplementBinary { get; set; }
}