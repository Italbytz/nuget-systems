using System;
using System.Net;

namespace Italbytz.Networking.Abstractions
{
    public interface INetmaskSolution
    {
        IPAddress NetworkAddress { get; set; }
        IPAddress HostAddress { get; set; }
    }
}
