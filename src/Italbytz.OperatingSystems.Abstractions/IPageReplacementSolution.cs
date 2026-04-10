using System;
using System.Collections.Generic;

namespace Italbytz.OperatingSystems.Abstractions
{
    public interface IPageReplacementSolution
    {
        List<IPageReplacementStep> Steps { get; set; }
    }
}
