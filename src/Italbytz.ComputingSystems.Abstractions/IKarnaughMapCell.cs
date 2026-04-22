namespace Italbytz.ComputingSystems.Abstractions;

public interface IKarnaughMapCell
{
    int RowIndex { get; set; }
    int ColumnIndex { get; set; }
    int MintermIndex { get; set; }
    int Value { get; set; }
    string RowCode { get; set; }
    string ColumnCode { get; set; }
}