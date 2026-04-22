using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class NormalFormSolution : INormalFormSolution
{
    public string Dnf { get; set; } = string.Empty;

    public string Cnf { get; set; } = string.Empty;

    public List<string> Steps { get; set; } = new();
}