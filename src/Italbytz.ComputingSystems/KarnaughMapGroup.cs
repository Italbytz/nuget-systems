using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class KarnaughMapGroup : IKarnaughMapGroup
{
    public string Term { get; set; } = string.Empty;

    public int[] CoveredMintermIndices { get; set; } = [];

    public int[] CoveredCellIndices { get; set; } = [];

    public bool IsEssential { get; set; }
}