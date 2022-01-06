using LaserAPI.Models.Dto.Zones;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper.Zones
{
    public static class ZonesHelper
    {
        //TODO add method to check if new position crosses a zone

        /// <summary>
        /// Checks if the positions are within a zone
        /// </summary>
        /// <param name="zones"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ZoneDto GetZoneWherePositionIsIn(IReadOnlyList<ZoneDto> zones, short x, short y)
        {
            int zonesLength = zones.Count;
            for (int i = 0; i < zonesLength; i++)
            {
                ZoneDto zone = zones[i];

                int highestPositionInZone = 0;
                int lowestPositionInZone = 0;
                int mostLeftPositionInZone = 0;
                int mostRightPositionInZone = 0;

                GetAbsolutePositionsFromZone(zone,
                    ref mostLeftPositionInZone,
                    ref mostRightPositionInZone,
                    ref highestPositionInZone,
                    ref lowestPositionInZone);

                bool positionIsWithinZone = x.IsBetweenOrEqualTo(mostLeftPositionInZone, mostRightPositionInZone) &&
                                            y.IsBetweenOrEqualTo(lowestPositionInZone, highestPositionInZone);

                if (positionIsWithinZone)
                {
                    return zone;
                }
            }

            return null;
        }

        private static void GetAbsolutePositionsFromZone(ZoneDto zone, ref int mostLeftPositionInZone, ref int mostRightPositionInZone,
            ref int highestPositionInZone, ref int lowestPositionInZone)
        {
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
        }
    }
}
