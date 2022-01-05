using LaserAPI.Models.Dto.Zones;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Models.Helper.Zones
{
    public static class ZonesHelper
    {
        private static short GetHighestPositionInZone(IEnumerable<short> zonePositions)
        {
            return zonePositions.Max();
        }

        private static short GetLowestPositionInZone(IEnumerable<short> zonePositions)
        {
            return zonePositions.Min();
        }

        public static ZoneDto GetZoneWherePositionIsIn(short x, short y, List<ZoneDto> zones)
        {
            return zones.Find(zone =>
            {
                IEnumerable<short> yCollection = zone.Positions.Select(pos => pos.Y);
                IEnumerable<short> xCollection = zone.Positions.Select(pos => pos.X);

                var yPositions = yCollection as short[] ?? yCollection.ToArray();
                short highestPosition = GetHighestPositionInZone(yPositions);
                short lowestPosition = GetLowestPositionInZone(yPositions);

                var xPositions = xCollection as short[] ?? xCollection.ToArray();
                short mostLeftPosition = GetLowestPositionInZone(xPositions);
                short mostRightPosition = GetHighestPositionInZone(xPositions);

                return y <= highestPosition && y >= lowestPosition &&
                    x >= mostLeftPosition && x <= mostRightPosition;
            });
        }
    }
}
