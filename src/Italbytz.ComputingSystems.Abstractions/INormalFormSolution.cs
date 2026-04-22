using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public interface INormalFormSolution : ITracedSolution
{
    string Dnf { get; set; }
    string Cnf { get; set; }
}