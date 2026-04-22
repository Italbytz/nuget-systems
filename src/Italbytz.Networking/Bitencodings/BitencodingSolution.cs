using System;
using System.Collections.Generic;
using Italbytz.Networking.Abstractions;

namespace Italbytz.Networking
{
    public class BitencodingSolution : IBitencodingSolution
    {
        public string[] NRZ { get; set; } = Array.Empty<string>();
        public string[] NRZI { get; set; } = Array.Empty<string>();
        public string[] MLT3 { get; set; } = Array.Empty<string>();
        public List<string> Steps { get; set; } = new();
    }
}
