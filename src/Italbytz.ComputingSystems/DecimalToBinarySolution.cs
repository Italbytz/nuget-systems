using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class DecimalToBinarySolution : IDecimalToBinarySolution
{
    public int Binary { get; set; }
    public List<string> Steps { get; set; } = new();
}