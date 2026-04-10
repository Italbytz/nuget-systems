using System;
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
            var solution = new NetmaskSolution
            {
                NetworkAddress = IPAddr.GetNetworkAddress(SubMask),
                HostAddress = IPAddr.GetHostAddress(SubMask)
            };
            return solution;
        }
    }
}
