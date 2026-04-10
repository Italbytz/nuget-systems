using System;
using Italbytz.Common.Random;
using Italbytz.Networking.Abstractions;

namespace Italbytz.Networking
{
    public class NetmaskParameters : INetmaskParameters
    {
        public int PrefixLength { get; set; }
        public string Address { get; set; } = string.Empty;

        public NetmaskParameters() : this(
            string.Join(".", new Random().NextIntArray(4, 1, 255)),
            new Random().Next(21, 30))
        {
        }

        public NetmaskParameters(string address, int prefixLength)
        {
            PrefixLength = prefixLength;
            Address = address;
        }
    }
}
