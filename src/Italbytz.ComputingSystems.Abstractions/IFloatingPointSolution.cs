using System.Collections.Generic;

namespace Italbytz.ComputingSystems.Abstractions;

public interface IFloatingPointSolution
{
    int Sign { get; set; }
    byte ExponentRaw { get; set; }
    int ExponentUnbiased { get; set; }
    uint MantissaRaw { get; set; }
    double MantissaFraction { get; set; }
    double MantissaWithImplicitOne { get; set; }
    double Value { get; set; }
    List<string> Steps { get; set; }
}
