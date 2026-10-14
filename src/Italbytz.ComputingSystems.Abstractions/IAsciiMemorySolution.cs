using System.Collections.Generic;
using Italbytz.Systems.Abstractions;

namespace Italbytz.ComputingSystems.Abstractions;

public class AsciiMemoryByte
{
    public int Address { get; set; }
    public byte Value { get; set; }
    public char Character { get; set; }
    public string Hex { get; set; } = string.Empty;
    public int ByteOffsetInWord { get; set; }
    public int RegisterBytePosition { get; set; }
}

public class AsciiMemoryWord
{
    public int Address { get; set; }
    public string HexWord { get; set; } = string.Empty;
    public AsciiMemoryByte[] Bytes { get; set; } = new AsciiMemoryByte[4];
}

public interface IAsciiMemorySolution : ITracedSolution
{
    string Word { get; set; }
    bool LittleEndian { get; set; }
    List<string> MemoryLines { get; set; }
    List<AsciiMemoryWord> Words { get; set; }
}
