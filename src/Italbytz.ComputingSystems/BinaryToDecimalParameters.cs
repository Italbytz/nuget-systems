using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class BinaryToDecimalParameters(byte binary) : IBinaryToDecimalParameters
{
    public byte Binary { get; set; } = binary;

    public BinaryToDecimalParameters()
        : this((byte)new Random().Next(1,256))
    {
    }
}