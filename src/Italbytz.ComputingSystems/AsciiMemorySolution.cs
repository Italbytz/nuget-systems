using System.Collections.Generic;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class AsciiMemorySolution : IAsciiMemorySolution
{
    public string Word { get; set; } = string.Empty;
    public bool LittleEndian { get; set; }
    public List<string> MemoryLines { get; set; } = new();
    public List<AsciiMemoryWord> Words { get; set; } = new();
    public List<string> Steps { get; set; } = new();
}
