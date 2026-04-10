using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class TwosComplementParameters(byte positiveBinary)
    : ITwosComplementParameters
{
    public byte PositiveBinary { get; set; } = positiveBinary;
    public TwosComplementParameters()
        : this((byte)new Random().Next(1,127))
    {
    }
}