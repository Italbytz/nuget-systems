using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public interface IKarnaughMapSolution : ITracedSolution
{
    int RowCount { get; set; }
    int ColumnCount { get; set; }
    IKarnaughMapCell[] Cells { get; set; }
    IKarnaughMapGroup[] Groups { get; set; }
}