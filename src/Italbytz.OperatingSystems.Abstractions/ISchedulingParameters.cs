using System;
namespace Italbytz.OperatingSystems.Abstractions
{
    public interface ISchedulingParameters
    {
        int[] Values { get; set; }
        string[] Priorities { get; set; }
    }
}
