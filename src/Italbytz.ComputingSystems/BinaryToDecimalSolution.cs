using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class BinaryToDecimalSolution : IBinaryToDecimalSolution
{
    public int Decimal { get; set; }
    public List<string> Steps { get; set; } = new();
}