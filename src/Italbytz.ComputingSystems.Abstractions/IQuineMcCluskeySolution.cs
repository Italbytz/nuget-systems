using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public interface IQuineMcCluskeySolution : ITracedSolution
{
    string MinimalDnf { get; set; }
    string[] PrimeImplicants { get; set; }
    string[] EssentialPrimeImplicants { get; set; }
    string[] SelectedPrimeImplicants { get; set; }
}