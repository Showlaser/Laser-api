using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Zones
{
    public class Zone
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// All positions in the zone
        /// </summary>
        public List<ZonesPosition> Points { get; set; }

        /// <summary>
        /// The maximum allowed laser power in a zone in PWM value
        /// </summary>
        public int MaxLaserPowerInZonePwm { get; set; }
    }
}
