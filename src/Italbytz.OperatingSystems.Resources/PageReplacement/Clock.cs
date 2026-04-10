using System;
namespace Italbytz.OperatingSystems.Resources.PageReplacement
{
    public class Clock : PageReplacementStrategy
    {
        private int _pointer;

        public Clock(int[] requests, int memorySize) : base(requests,
            memorySize)
        {
        }

        protected override int IndexToReplace(int element)
        {
            while (FrameInformation[_pointer] == 1)
            {
                FrameInformation[_pointer] = 0;
                _pointer = (_pointer + 1) % FrameInformation.Length;
            }

            return _pointer;
        }

        protected override void UpdateAfterReplacement(int index, int element)
        {
            _pointer = (_pointer + 1) % FrameInformation.Length;
        }

        protected override void UpdateAfterHit(int index, int element)
        {
            FrameInformation[index] = 1;
        }

        protected override int GetInitialFrameInformationValue() => 0;

        public override string ToString() =>
            base.ToString() + $"\nPointer: {_pointer}";

        protected override string GetAdditionalSimulationInfo() =>
            (_pointer + 1).ToString();
    }
}
