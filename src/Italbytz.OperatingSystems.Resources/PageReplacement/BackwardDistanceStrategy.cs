using System;
namespace Italbytz.OperatingSystems.Resources.PageReplacement
{
    public abstract class BackwardDistanceStrategy : DistanceStrategy
    {
        protected BackwardDistanceStrategy(int[] requests, int memorySize) :
            base(requests, memorySize)
        {
        }

        protected override void UpdateForNewRequest()
        {
            for (var i = 0; i < FrameInformation.Length; i++)
                if (FrameInformation[i] != GetInitialFrameInformationValue())
                    FrameInformation[i]++;
        }

        protected override void UpdateAfterReplacement(int index, int element)
        {
            FrameInformation[index] = 0;
        }
    }
}
