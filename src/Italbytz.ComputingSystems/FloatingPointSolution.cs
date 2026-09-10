using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class FloatingPointSolution : IFloatingPointSolution
{
    public int Sign { get; set; }
    public byte ExponentRaw { get; set; }
    public int ExponentUnbiased { get; set; }
    public uint MantissaRaw { get; set; }
    public double MantissaFraction { get; set; }
    public double MantissaWithImplicitOne { get; set; }
    public double Value { get; set; }
    public List<string> Steps { get; set; } = new();
}
