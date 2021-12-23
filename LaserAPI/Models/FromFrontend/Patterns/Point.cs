using System;

namespace LaserAPI.Models.FromFrontend.Patterns
{
    public class Point
    {
        public Guid Uuid { get; set; }
        public Guid PatternUuid { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public Guid ConnectedToUuid { get; set; }
    }
}
