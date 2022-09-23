using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Patterns
{
    public class Pattern
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public double Scale { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int Rotation { get; set; }
        public List<PatternPoint> Points { get; set; }
    }
}
