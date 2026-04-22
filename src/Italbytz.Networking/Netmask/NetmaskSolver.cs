using System;
using System.Collections.Generic;
using System.Net;
using Italbytz.Networking.Abstractions;
using Italbytz.Networking.Resources;

namespace Italbytz.Networking
{
    public class NetmaskSolver : INetmaskSolver
    {
        public NetmaskSolver()
        {
        }

        public INetmaskSolution Solve(INetmaskParameters parameters)
        {
            var IPAddr = IPAddress.Parse(parameters.Address);
            var SubMask = SubnetMask.CreateByNetBitLength(parameters.PrefixLength);
            var networkAddress = IPAddr.GetNetworkAddress(SubMask);
            var hostAddress = IPAddr.GetHostAddress(SubMask);
            var steps = new List<string>
            {
                $"Input address: {IPAddr}.",
                $"Prefix length: /{parameters.PrefixLength}.",
                $"Subnet mask: {SubMask}.",
                $"Network address = IP AND mask = {networkAddress}.",
                $"Host address = IP AND NOT(mask) = {hostAddress}."
            };
            var solution = new NetmaskSolution
            {
                NetworkAddress = networkAddress,
                HostAddress = hostAddress,
                Steps = steps
            };
            return solution;
        }
    }
}
