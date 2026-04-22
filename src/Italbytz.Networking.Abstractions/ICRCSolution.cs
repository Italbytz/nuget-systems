using System;
using Italbytz.Systems.Abstractions;
namespace Italbytz.Networking.Abstractions
{
    public interface ICRCSolution : ITracedSolution
    {
        string Calculation { get; set; }
        string Check { get; set; }
    }
}
