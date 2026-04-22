using Italbytz.Systems.Abstractions;

namespace Italbytz.Networking.Abstractions
{
    public interface IBitencodingSolution : ITracedSolution
    {
        string[] NRZ { get; set; }
        string[] NRZI { get; set; }
        string[] MLT3 { get; set; }
    }
}