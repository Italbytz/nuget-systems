using System;
namespace Italbytz.OperatingSystems.Resources.PageReplacement
{
    public class Fifo : BackwardDistanceStrategy
    {
        public Fifo(int[] requests, int memorySize) : base(requests, memorySize)
        {
        }
    }
}
