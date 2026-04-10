using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class DecimalToBinaryParameters(byte _decimal)
    : IDecimalToBinaryParameters
{
    public byte Decimal { get; set; } = _decimal;

    public DecimalToBinaryParameters()
        : this((byte)new Random().Next(1,256))
    {
    }
}