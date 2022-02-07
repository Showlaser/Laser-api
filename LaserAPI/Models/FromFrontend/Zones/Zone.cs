using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Zones
{
    public class Zone
    {
        public Guid Uuid { get; set; }

        /// <summary>
        /// All positions in the zone
        /// </summary>
        public List<ZonesPosition> Positions { get; set; }

        /// <summary>
        /// The maximum allowed laser power in a zone in PWM value
        /// </summary>
        public int MaxLaserPowerInZonePwm { get; set; }
    }
}
