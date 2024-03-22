using System;

namespace LaserAPI.Models.ToFrontend.Zones
{
    public class SafetyZonePointViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid SafetyZoneUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int OrderNr { get; set; }
    }
}
