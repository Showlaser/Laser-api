using System;

namespace LaserAPI.Models.Dto.Zones
{
    public class ZonesPositionDto
    {
        public Guid Uuid { get; set; }
        public Guid ZoneUuid { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
    }
}
