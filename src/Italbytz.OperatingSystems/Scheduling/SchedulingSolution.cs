using System.Collections.Generic;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class SchedulingSolution : ISchedulingSolution
    {
        public double Time { get; set; }
        public List<string> Steps { get; set; } = new();
    }
}