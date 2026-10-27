namespace Italbytz.Systems.Blazor.Components.ComputingSystems;

public class KvDiagramCellContext
{
    public int RowIndex { get; set; }
    public int ColumnIndex { get; set; }
    public string RowCode { get; set; } = string.Empty;
    public string ColumnCode { get; set; } = string.Empty;
    public int MintermIndex { get; set; }
    public int Value { get; set; }
}
