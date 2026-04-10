using System;
namespace Italbytz.OperatingSystems.Resources.Buddy
{
    public class Node
    {
        public int ProcessId { get; set; }
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsAssigned { get; set; }

    }
}
