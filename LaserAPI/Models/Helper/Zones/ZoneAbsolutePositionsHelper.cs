using LaserAPI.Models.Dto.Zones;

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
            for (int i = 0; i < 4; i++)
            {
                ZonesPositionDto zonePosition = zone.Positions[i];
                switch (zonePosition.X)
                {
                    case < 0:
                        LeftXAxisInZone = zonePosition.X;
                        break;
                    case > 0:
                        RightXAxisInZone = zonePosition.X;
                        break;
                }

                switch (zonePosition.Y)
                {
                    case > 0:
                        HighestYAxisInZone = zonePosition.Y;
                        break;
                    case < 0:
                        LowestYAxisInZone = zonePosition.Y;
                        break;
                }
            }
        }
    }
}
