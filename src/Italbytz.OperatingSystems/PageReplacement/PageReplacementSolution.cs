using System;
using System.Collections.Generic;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems.PageReplacement
{
    public class PageReplacementSolution : IPageReplacementSolution
    {
        public List<IPageReplacementStep> Steps { get; set; } = new();
    }
}
