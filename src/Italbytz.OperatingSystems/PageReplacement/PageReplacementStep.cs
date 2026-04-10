using System;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems.PageReplacement
{
    public class PageReplacementStep : IPageReplacementStep
    {
        public PageReplacementStep()
        {
        }

        public int[] Frames { get; set; }
        public int[] FrameInformation { get; set; }
        public int Count { get; set; }
        public string AdditionalInfo { get; set; }
        public int Element { get; set; }
    }
}
