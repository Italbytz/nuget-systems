using System;
namespace Italbytz.Networking.Abstractions
{
    public interface INetmaskParameters
    {
        int PrefixLength { get; set; }
        string Address { get; set; }
    }
}
