using System;

namespace LaserAPI.Models.ToFrontend.Pattern
{
    public class PointsViewmodel
    {
        public Guid Uuid { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public Guid ConnectedToUuid { get; set; }
    }
}
