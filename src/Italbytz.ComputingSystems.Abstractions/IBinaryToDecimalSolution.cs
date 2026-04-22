using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public interface IBinaryToDecimalSolution : ITracedSolution
{
    int Decimal { get; set; }
}