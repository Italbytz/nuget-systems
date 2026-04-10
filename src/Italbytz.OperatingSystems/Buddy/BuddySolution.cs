using System;
using System.Collections.Generic;
using Italbytz.OperatingSystems.Abstractions;

namespace Italbytz.OperatingSystems
{
    public class BuddySolution : IBuddySolution
    {
        public List<int[]> History { get; set; } = new();
    }
}
