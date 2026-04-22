using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class KarnaughMapSolution : IKarnaughMapSolution
{
    public int RowCount { get; set; }

    public int ColumnCount { get; set; }

    public IKarnaughMapCell[] Cells { get; set; } = [];

    public IKarnaughMapGroup[] Groups { get; set; } = [];

    public List<string> Steps { get; set; } = new();
}