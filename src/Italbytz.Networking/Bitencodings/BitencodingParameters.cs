using System;
using Italbytz.Common.Random;
using Italbytz.Networking.Abstractions;

namespace Italbytz.Networking
{
    public class BitencodingParameters : IBitencodingParameters
    {
        public int[] Bits { get; set; } = Array.Empty<int>();

        public BitencodingParameters() : this(new Random().NextIntArray(10, 0, 1))
        {
        }

        public BitencodingParameters(int[] bits)
        {
            Bits = bits;
        }
    }
}
