using System.Collections.Generic;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class RealtimeSchedulingSolution : IRealtimeSchedulingSolution
    {
        public int[] Processes { get; set; } = [];
        public List<string> Steps { get; set; } = new();
    }
}