using System;
namespace Italbytz.OperatingSystems.Abstractions
{
    public interface IPageReplacementParameters
    {
        int[] ReferenceRequests { get; set; }
        int MemorySize { get; set; }
    }
}
