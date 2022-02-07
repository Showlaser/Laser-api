using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Zones
{
    public class ZoneViewmodel
    {
        public Guid Uuid { get; set; }

        /// <summary>
        /// All positions in the zone
        /// </summary>
        public List<ZonesPositionViewmodel> Positions { get; set; }

        /// <summary>
        /// The maximum allowed laser power in a zone in PWM value
        /// </summary>
        public int MaxLaserPowerInZonePwm { get; set; }
    }
}
