using LaserAPI.Models.Dto.Zones;

namespace LaserAPI.Models.Helper.Zones
{
    public class ZonesHitDataHelper
    {
        public ZoneDto Zone { get; set; }
        public ZoneSidesHitHelper ZoneSidesHit { get; set; }
        public ZoneAbsolutePositionsHelper ZoneAbsolutePositions { get; set; }
    }
}
