using System;
using System.Collections.Generic;

namespace Italbytz.OperatingSystems.Resources.PageReplacement
{
    public abstract class PageReplacementStrategy
    {
        protected PageReplacementStrategy(int[] requests, int memorySize)
        {
            Requests = requests;
            Frames = new int[memorySize];
            FrameInformation = new int[memorySize];
            for (var i = 0; i < Frames.Length; i++)
            {
                Frames[i] = int.MaxValue;
                // ReSharper disable once VirtualMemberCallInConstructor
                FrameInformation[i] = GetInitialFrameInformationValue();
            }
        }

        protected int[] Requests { get; }
        protected int Index { get; private set; }
        private int Count { get; set; }

        private int[] Frames { get; }
        protected int[] FrameInformation { get; }

        private void Request(int element)
        {
            // Init
            UpdateForNewRequest();

            // Check if element is in frame
            var hitIndex = Array.IndexOf(Frames, element);
            if (hitIndex >= 0)
            {
                UpdateAfterHit(hitIndex, element);
                return;
            }

            // Element is not in frame
            Count++;
            var replaceIndex = IndexToReplace(element);
            Frames[replaceIndex] = element;
            UpdateAfterReplacement(replaceIndex, element);
        }

        protected virtual void UpdateForNewRequest()
        {
        }

        protected abstract int IndexToReplace(int element);

        protected virtual void UpdateAfterHit(int index, int element)
        {
        }

        protected abstract void UpdateAfterReplacement(int index, int element);

        public List<SimulationResult> Simulate()
        {
            var result = new List<SimulationResult>
            {
                new SimulationResult
                {
                    Frames = (int[]) Frames.Clone(),
                    FrameInformation = (int[]) FrameInformation.Clone(),
                    Count = 0,
                    Element = int.MinValue,
                    AdditionalInfo = GetAdditionalSimulationInfo()
                }
            };

            for (Index = 0; Index < Requests.Length; Index++)
            {
                Request(Requests[Index]);
                result.Add(new SimulationResult
                {
                    Frames = (int[])Frames.Clone(),
                    FrameInformation = (int[])FrameInformation.Clone(),
                    Count = Count,
                    Element = Requests[Index],
                    AdditionalInfo = GetAdditionalSimulationInfo()
                });
            }

            return result;
        }

        protected virtual string GetAdditionalSimulationInfo() => string.Empty;

        protected virtual int GetInitialFrameInformationValue() => int.MaxValue;

        public override string ToString() =>
            $"{string.Join(",", Frames)}\n{string.Join(",", FrameInformation)}\n{Count}";
    }
}
