using System;

namespace LaserAPI.Models.FromFrontend.Zones
{
    public class SafetyZonePoint
    {
        public Guid Uuid { get; set; }
        public Guid SafetyZoneUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int OrderNr { get; set; }
    }
}
