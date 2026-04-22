using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class TwosComplementSolution : ITwosComplementSolution
{
    public sbyte ComplementBinary { get; set; }
    public List<string> Steps { get; set; } = new();
}