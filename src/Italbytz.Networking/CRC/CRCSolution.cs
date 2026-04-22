using System;
using System.Collections.Generic;
using Italbytz.Networking.Abstractions;

namespace Italbytz.Networking
{
    public class CRCSolution : ICRCSolution
    {
        public CRCSolution()
        {
        }

        public string Calculation { get; set; } = string.Empty;
        public string Check { get; set; } = string.Empty;
        public List<string> Steps { get; set; } = new();
    }
}
