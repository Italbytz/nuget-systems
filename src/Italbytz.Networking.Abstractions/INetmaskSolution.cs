using System;
using System.Net;
using Italbytz.Systems.Abstractions;

namespace Italbytz.Networking.Abstractions
{
    public interface INetmaskSolution : ITracedSolution
    {
        IPAddress NetworkAddress { get; set; }
        IPAddress HostAddress { get; set; }
    }
}
