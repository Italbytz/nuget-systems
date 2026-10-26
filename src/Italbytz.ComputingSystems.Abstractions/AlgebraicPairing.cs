namespace Italbytz.ComputingSystems.Abstractions;

public class AlgebraicPairing
{
    public int MintermA { get; set; }
    public int MintermB { get; set; }
    public string TermA { get; set; } = string.Empty;
    public string TermB { get; set; } = string.Empty;
    public string DifferingVariable { get; set; } = string.Empty;
    public string CommonFactor { get; set; } = string.Empty;
    public string CombinedTerm { get; set; } = string.Empty;
}
