using System;

namespace LaserAPI.Models.ToFrontend.Zones
{
    public class ZonesPositionViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid ZoneUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
