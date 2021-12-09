using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Pattern
{
    public class PatternViewmodel
    {
        public Guid Uuid { get; set; }
        public double Scale { get; set; }
        public List<PointsViewmodel> Points { get; set; }
    }
}
