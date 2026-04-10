using System;
namespace Italbytz.OperatingSystems.Abstractions
{
    public interface IRealtimeSchedulingParameters
    {
        (int, int)[] Requests { get; set; }
    }
}
