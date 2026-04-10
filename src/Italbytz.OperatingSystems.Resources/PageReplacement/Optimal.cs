using System;
namespace Italbytz.OperatingSystems.Resources.PageReplacement
{
    public class Optimal : ForwardDistanceStrategy
    {
        public Optimal(int[] requests, int memorySize) : base(requests,
            memorySize)
        {
        }

        protected override void UpdateAfterHit(int index, int element)
        {
            UpdateAfterReplacement(index, element);
        }

        protected override void UpdateAfterReplacement(int index, int element)
        {
            FrameInformation[index] = GetInitialFrameInformationValue();
            for (var i = Index + 1; i < Requests.Length; i++)
                if (Requests[i] == element)
                {
                    FrameInformation[index] = i - Index;
                    break;
                }
        }
    }
}
