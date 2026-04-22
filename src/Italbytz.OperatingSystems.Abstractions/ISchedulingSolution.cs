using System;
using Italbytz.Systems.Abstractions;
namespace Italbytz.OperatingSystems.Abstractions
{
    public interface ISchedulingSolution : ITracedSolution
    {
        double Time { get; set; }
    }
}
