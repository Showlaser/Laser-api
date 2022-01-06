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
            int highestPositionInZone = 0;
            int lowestPositionInZone = 0;
            int mostLeftPositionInZone = 0;
            int mostRightPositionInZone = 0;

            int positionsLength = zone.Positions.Count;
            for (int i = 0; i < positionsLength; i++)
            {
                ZonesPositionDto zonePosition = zone.Positions[i];
                if (zonePosition.X < mostLeftPositionInZone)
                {
                    mostLeftPositionInZone = zonePosition.X;
                }

                if (zonePosition.X > mostRightPositionInZone)
                {
                    mostRightPositionInZone = zonePosition.X;
                }

                if (zonePosition.Y > highestPositionInZone)
                {
                    highestPositionInZone = zonePosition.Y;
                }

                if (zonePosition.Y < lowestPositionInZone)
                {
                    lowestPositionInZone = zonePosition.Y;
                }
            }

            HighestYAxisInZone = highestPositionInZone;
            LowestYAxisInZone = lowestPositionInZone;
            LeftXAxisInZone = mostLeftPositionInZone;
            RightXAxisInZone = mostRightPositionInZone;
        }
    }
}
