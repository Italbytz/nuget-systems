using System;
namespace Italbytz.OperatingSystems.Resources.Buddy
{
    public class Process
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Size { get; set; }
        public string OpType { get; set; } = string.Empty;
    }
}
