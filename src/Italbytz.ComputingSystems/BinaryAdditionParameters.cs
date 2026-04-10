using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class BinaryAdditionParameters(byte summand1, byte summand2)
    : IBinaryAdditionParameters
{
    public byte Summand1 { get; set; } = summand1;
    public byte Summand2 { get; set; } = summand2;
    
    public BinaryAdditionParameters()
        : this((byte)new Random().Next(1,256),(byte)new Random().Next(1,256))
    {
    }
}