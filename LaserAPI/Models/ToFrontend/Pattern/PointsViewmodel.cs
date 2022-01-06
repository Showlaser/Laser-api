using System;

namespace LaserAPI.Models.ToFrontend.Pattern
{
    public class PointsViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid PatternUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Guid ConnectedToUuid { get; set; }
    }
}
