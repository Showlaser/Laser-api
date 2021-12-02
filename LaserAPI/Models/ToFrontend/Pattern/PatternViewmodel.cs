using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Pattern
{
    public class PatternViewmodel
    {
        public double Scale { get; set; }
        public List<PointsViewmodel> Points { get; set; }
    }
}
