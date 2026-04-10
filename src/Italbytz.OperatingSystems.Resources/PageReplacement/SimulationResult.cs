using System;
namespace Italbytz.OperatingSystems.Resources.PageReplacement
{
    public class SimulationResult
    {
        public int[] Frames { get; set; }
        public int[] FrameInformation { get; set; }
        public int Count { get; set; }
        public string AdditionalInfo { get; set; }
        public int Element { get; set; }

        public override string ToString() =>
            $"Requesting {Element}:\n{string.Join(",", Frames)}\n{string.Join(",", FrameInformation)}\n{Count}{AdditionalInfo}";
    }
}
