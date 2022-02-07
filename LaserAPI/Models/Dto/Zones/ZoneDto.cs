using System;

namespace LaserAPI.Models.Dto.Zones
{
    public class ZoneDto
    {
        public Guid Uuid { get; set; }

        /// <summary>
        /// All positions in the zone
        /// </summary>
        public ZonesPositionDto[] Positions { get; set; }

        /// <summary>
        /// The maximum allowed laser power in a zone in PWM value
        /// </summary>
        public int MaxLaserPowerInZonePwm { get; set; }
    }
}
