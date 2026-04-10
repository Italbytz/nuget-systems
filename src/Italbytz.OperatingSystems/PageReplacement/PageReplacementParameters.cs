using System;
using Italbytz.Common.Random;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class PageReplacementParameters : IPageReplacementParameters
    {
        public int[] ReferenceRequests { get; set; }
        public int MemorySize { get; set; }

        public PageReplacementParameters() : this(new Random().NextIntArray(13, 1, 6))
        {

        }

        public PageReplacementParameters(int[] referenceRequests)
        {
            ReferenceRequests = referenceRequests;
        }

    }
}
