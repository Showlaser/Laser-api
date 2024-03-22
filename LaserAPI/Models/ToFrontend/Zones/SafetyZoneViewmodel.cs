using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Zones
{
    public class SafetyZoneViewmodel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid AppliedOnShowLaserUuid { get; set; }

        /// <summary>
        /// All positions in the zone
        /// </summary>
        public List<SafetyZonePointViewmodel> Points { get; set; }

        /// <summary>
        /// The maximum allowed laser power in a zone in PWM value
        /// </summary>
        public int MaxLaserPowerInZonePercentage { get; set; }
    }
}
