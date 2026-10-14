using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class AsciiMemoryParameters : IAsciiMemoryParameters
{
    public AsciiMemoryParameters(string word, bool littleEndian = true)
    {
        Word = word;
        LittleEndian = littleEndian;
    }

    public string Word { get; set; }
    public bool LittleEndian { get; set; }
}
