using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Patterns
{
    public class Pattern
    {
        public Guid Uuid { get; set; }
        public double Scale { get; set; }
        public List<Point> Points { get; set; }
    }
}
