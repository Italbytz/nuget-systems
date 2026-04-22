using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class BinaryAdditionSolution : IBinaryAdditionSolution
{
    public ushort Sum { get; set; }
    public List<string> Steps { get; set; } = new();
}