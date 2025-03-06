using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Pattern
{
    public class PatternViewmodel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Scale { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int Rotation { get; set; }
        public List<PointsViewmodel> Points { get; set; }
    }
}
