using System;
using Italbytz.OperatingSystems.Resources.PageReplacement;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems.PageReplacement
{
    public static class Extensions
    {
        public static IPageReplacementStep ToStep(this SimulationResult sim)
        {
            return new PageReplacementStep()
            {
                AdditionalInfo = sim.AdditionalInfo,
                Count = sim.Count,
                Element = sim.Element,
                FrameInformation = sim.FrameInformation,
                Frames = sim.Frames
            };
    }
}
}
