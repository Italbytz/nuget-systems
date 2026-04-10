using System;
using System.Net;
using Italbytz.Networking.Abstractions;

namespace Italbytz.Networking
{
    public class NetmaskSolution : INetmaskSolution
    {
        public NetmaskSolution()
        {
        }

        public IPAddress NetworkAddress { get; set; } = IPAddress.None;
        public IPAddress HostAddress { get; set; } = IPAddress.None;
    }
}
