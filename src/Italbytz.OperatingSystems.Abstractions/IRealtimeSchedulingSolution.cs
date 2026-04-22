using System;
using Italbytz.Systems.Abstractions;
namespace Italbytz.OperatingSystems.Abstractions
{
    public interface IRealtimeSchedulingSolution : ITracedSolution
    {
        int[] Processes { get; set; }
    }
}
