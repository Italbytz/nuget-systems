using System;
namespace Italbytz.OperatingSystems.Resources.PageReplacement
{
    public class SimulationResult
    {
        public int[] Frames { get; set; } = Array.Empty<int>();
        public int[] FrameInformation { get; set; } = Array.Empty<int>();
        public int Count { get; set; }
        public string AdditionalInfo { get; set; } = string.Empty;
        public int Element { get; set; }

        public override string ToString() =>
            $"Requesting {Element}:\n{string.Join(",", Frames)}\n{string.Join(",", FrameInformation)}\n{Count}{AdditionalInfo}";
    }
}
