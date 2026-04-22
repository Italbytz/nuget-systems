namespace Italbytz.ComputingSystems.Abstractions;

public interface IKarnaughMapGroup
{
    string Term { get; set; }
    int[] CoveredMintermIndices { get; set; }
    int[] CoveredCellIndices { get; set; }
    bool IsEssential { get; set; }
}