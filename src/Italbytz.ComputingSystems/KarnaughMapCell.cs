using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class KarnaughMapCell : IKarnaughMapCell
{
    public int RowIndex { get; set; }

    public int ColumnIndex { get; set; }

    public int MintermIndex { get; set; }

    public int Value { get; set; }

    public string RowCode { get; set; } = string.Empty;

    public string ColumnCode { get; set; } = string.Empty;
}