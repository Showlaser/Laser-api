using LaserAPI.Models.Dto.Zones;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Models.Helper.Zones
{
    public static class ZonesHelper
    {
        public static IReadOnlyList<ZonesHitDataHelper> GetZonesInPathOfPosition(ZoneDto[] zones,
            int previousX, int previousY, int newX, int newY, ref int zonesCrossedDataLength)
        {
            int zonesLength = zones.Length;
            List<ZonesHitDataHelper> zonesInPath = new();

            for (int i = 0; i < zonesLength; i++)
            {
                ZoneDto zone = zones[i];
                ZoneSidesHitHelper zoneSidesHit = GetZoneSidesHitByPath(zone, previousX, previousY, newX, newY);

                if (zoneSidesHit.BottomHit || zoneSidesHit.TopHit || zoneSidesHit.LeftHit || zoneSidesHit.RightHit)
                {
                    zonesCrossedDataLength++;
                    zonesInPath.Add(new ZonesHitDataHelper
                    {
                        Zone = zone,
                        ZoneSidesHit = zoneSidesHit
                    });
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
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ZoneDto GetZoneWherePositionIsIn(IReadOnlyList<ZoneDto> zones, int x, int y)
        {
            int zonesLength = zones.Count;
            for (int i = 0; i < zonesLength; i++)
            {
                ZoneDto zone = zones[i];
                ZoneAbsolutePositionsHelper absolutePositionsHelper = new(zone);

                bool positionIsWithinZone = x.IsBetweenOrEqualTo(absolutePositionsHelper.LeftXAxisInZone, absolutePositionsHelper.RightXAxisInZone) &&
                                            y.IsBetweenOrEqualTo(absolutePositionsHelper.LowestYAxisInZone, absolutePositionsHelper.HighestYAxisInZone);

                if (positionIsWithinZone)
                {
                    return zone;
                }
            }

            return null;
        }

        public static ZoneDto GetZoneWhereMaxLaserPowerPwmIsHighest(List<ZoneDto> zones)
        {
            return zones.MaxBy(zone => zone.MaxLaserPowerInZonePwm);
        }
    }
}
