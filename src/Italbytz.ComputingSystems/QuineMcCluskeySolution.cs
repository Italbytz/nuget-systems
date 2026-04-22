using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class QuineMcCluskeySolution : IQuineMcCluskeySolution
{
    public string MinimalDnf { get; set; } = string.Empty;

    public string[] PrimeImplicants { get; set; } = [];

    public string[] EssentialPrimeImplicants { get; set; } = [];

    public string[] SelectedPrimeImplicants { get; set; } = [];

    public List<string> Steps { get; set; } = new();
}