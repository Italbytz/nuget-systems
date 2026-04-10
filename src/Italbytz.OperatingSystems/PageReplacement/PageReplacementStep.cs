using System;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems.PageReplacement
{
    public class PageReplacementStep : IPageReplacementStep
    {
        public int[] Frames { get; set; } = Array.Empty<int>();
        public int[] FrameInformation { get; set; } = Array.Empty<int>();
        public int Count { get; set; }
        public string AdditionalInfo { get; set; } = string.Empty;
        public int Element { get; set; }
    }
}
