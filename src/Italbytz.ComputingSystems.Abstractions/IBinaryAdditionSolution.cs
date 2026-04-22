using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public interface IBinaryAdditionSolution : ITracedSolution
{
    ushort Sum { get; set; }
}