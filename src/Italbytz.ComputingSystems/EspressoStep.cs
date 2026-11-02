using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class EspressoStep : IEspressoStep
{
    public string Phase { get; set; } = string.Empty;
    public int Iteration { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] CurrentCover { get; set; } = [];
    public string ActionDetail { get; set; } = string.Empty;
}

