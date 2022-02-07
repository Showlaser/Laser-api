using LaserAPI.Models.Dto.Zones;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Models.Helper.Zones
{
    public static class ZonesHelper
    {
        public static List<ZonesHitDataHelper> GetZonesInPathOfPosition(List<ZonesHitDataHelper> zones,
            int previousX, int previousY, int newX, int newY, ref int zonesCrossedDataLength)
        {
            int zonesLength = zones.Count;
            List<ZonesHitDataHelper> zonesInPath = new();

            for (int i = 0; i < zonesLength; i++)
            {
                ZonesHitDataHelper zoneHitData = zones[i];
                ZoneSidesHitHelper zoneSidesHit = GetZoneSidesHitByPath(zoneHitData.Zone, previousX, previousY, newX, newY);

                if (zoneSidesHit.BottomHit || zoneSidesHit.TopHit || zoneSidesHit.LeftHit || zoneSidesHit.RightHit)
                {
                    zonesCrossedDataLength++;
                    zoneHitData.ZoneSidesHit = zoneSidesHit;
                    zonesInPath.Add(zoneHitData);
                }
            }

            return zonesInPath;
        }

        /// <summary>
        /// Checks if the path of the previous position and the new position crosses a zone
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="previousX"></param>
        /// <param name="previousY"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <returns>The zones that are crossed</returns>
        private static ZoneSidesHitHelper GetZoneSidesHitByPath(ZoneDto zone, int previousX, int previousY, int newX,
            int newY)
        {
            ZoneAbsolutePositionsHelper absolutePositionsHelper = new(zone);
            return new ZoneSidesHitHelper
            {
                LeftHit = absolutePositionsHelper.LeftXAxisInZone.IsBetweenOrEqualTo(
                    NumberHelper.GetLowestNumber(previousX, newX),
                    NumberHelper.GetHighestNumber(previousX, newX)),
                RightHit = absolutePositionsHelper.RightXAxisInZone.IsBetweenOrEqualTo(
                    NumberHelper.GetLowestNumber(previousX, newX),
                    NumberHelper.GetHighestNumber(previousX, newX)),
                BottomHit = absolutePositionsHelper.LowestYAxisInZone.IsBetweenOrEqualTo(
                    NumberHelper.GetLowestNumber(previousY, newY),
                    NumberHelper.GetHighestNumber(previousY, newY)),
                TopHit = absolutePositionsHelper.HighestYAxisInZone.IsBetweenOrEqualTo(
                    NumberHelper.GetLowestNumber(previousY, newY),
                    NumberHelper.GetHighestNumber(previousY, newY))
            };
        }

        /// <summary>
        /// Checks if the positions are within a zone
        /// </summary>
        /// <param name="zones"></param>
        /// <param name="zonesLength"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ZonesHitDataHelper GetZoneWherePositionIsIn(List<ZonesHitDataHelper> zones, int zonesLength, int x, int y)
        {
            for (int i = 0; i < zonesLength; i++)
            {
                ZonesHitDataHelper zoneHitData = zones[i];
                bool positionIsWithinZone = x.IsBetweenOrEqualTo(zoneHitData.ZoneAbsolutePositions.LeftXAxisInZone, zoneHitData.ZoneAbsolutePositions.RightXAxisInZone) &&
                                            y.IsBetweenOrEqualTo(zoneHitData.ZoneAbsolutePositions.LowestYAxisInZone, zoneHitData.ZoneAbsolutePositions.HighestYAxisInZone);

                if (positionIsWithinZone)
                {
                    return zoneHitData;
                }
            }

            return null;
        }

        public static ZoneDto GetZoneWhereMaxLaserPowerPwmIsHighest(List<ZonesHitDataHelper> zonesHitDataCollection)
        {
            return zonesHitDataCollection
                .Select(zone => zone.Zone)
                .MaxBy(zone => zone.MaxLaserPowerInZonePwm);
        }
    }
}
