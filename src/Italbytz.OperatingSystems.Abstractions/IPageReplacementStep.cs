using System;
namespace Italbytz.OperatingSystems.Abstractions
{
    public interface IPageReplacementStep
    {
        int[] Frames { get; set; }
        int[] FrameInformation { get; set; }
        int Count { get; set; }
        string AdditionalInfo { get; set; }
        int Element { get; set; }
    }
}
