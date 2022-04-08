using LaserAPI.Models.Dto.Zones;
using System.Linq;

namespace LaserAPI.Models.Helper.Zones
{
    public class ZoneAbsolutePositionsHelper
    {
        public int LeftXAxisInZone { get; private set; }
        public int RightXAxisInZone { get; private set; }
        public int HighestYAxisInZone { get; private set; }
        public int LowestYAxisInZone { get; private set; }

        /// <summary>
        /// This class gets the most absolute values from the positions of a zone
        /// </summary>
        /// <param name="zone"></param>
        public ZoneAbsolutePositionsHelper(ZoneDto zone)
        {
            GetAbsolutePositionsFromZone(zone);
        }

        private void GetAbsolutePositionsFromZone(ZoneDto zone)
        {
            LeftXAxisInZone = zone.Positions.GroupBy(z => z.X).First().Key;
            RightXAxisInZone = zone.Positions.GroupBy(z => z.X).Last().Key;
            LowestYAxisInZone = zone.Positions.GroupBy(z => z.Y).Last().Key;
            HighestYAxisInZone = zone.Positions.GroupBy(z => z.Y).First().Key;
        }
    }
}
