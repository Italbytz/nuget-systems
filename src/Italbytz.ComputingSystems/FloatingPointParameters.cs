using System;
using Italbytz.ComputingSystems.Abstractions;

namespace Italbytz.ComputingSystems;

public class FloatingPointParameters : IFloatingPointParameters
{
    private static readonly float[] DefaultPresets =
    [
        -0.375f,
        18.0f,
        -12.5f,
        6.25f,
        0.75f,
        -42.5f,
        0.1875f,
        -1.5f,
        2.625f,
        -8.25f
    ];

    public uint BitPattern { get; set; }

    public FloatingPointParameters(uint bitPattern)
    {
        BitPattern = bitPattern;
    }

    public FloatingPointParameters(float value)
    {
        BitPattern = BitConverter.SingleToUInt32Bits(value);
    }

    public FloatingPointParameters()
        : this(DefaultPresets[new Random().Next(DefaultPresets.Length)])
    {
    }
}
