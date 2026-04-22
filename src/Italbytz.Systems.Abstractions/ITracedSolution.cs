using System.Collections.Generic;

namespace Italbytz.Systems.Abstractions;

public interface ITracedSolution
{
    List<string> Steps { get; set; }
}
