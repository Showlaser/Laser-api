using System;

namespace LaserAPI.Models.FromFrontend.Zones
{
    public class Zone
    {
        public Guid Uuid { get; set; }

        /// <summary>
        /// All positions in the zone
        /// </summary>
        public ZonesPosition[] Positions { get; set; }

        /// <summary>
        /// The maximum allowed laser power in a zone in PWM value
        /// </summary>
        public int MaxLaserPowerInZonePwm { get; set; }
    }
}
