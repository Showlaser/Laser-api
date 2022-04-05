using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Zones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class ZoneLogic
    {
        private readonly IZoneDal _zoneDal;

        public ZoneLogic(IZoneDal zoneDal)
        {
            _zoneDal = zoneDal;
        }

        public async Task<List<ZoneDto>> All()
        {
            return await _zoneDal.All();
        }

        public async Task AddOrUpdate(ZoneDto zone)
        {
            if (await _zoneDal.Exists(zone.Uuid))
            {
                await _zoneDal.Update(zone);
                return;
            }

            await _zoneDal.Add(zone);
        }

        public async Task Remove(Guid uuid)
        {
            await _zoneDal.Remove(uuid);
        }

        public static List<ZonesHitData> GetZonesInPathOfPosition(List<ZonesHitData> zones,
            int previousX, int previousY, int newX, int newY)
        {
            int zonesLength = zones.Count;
            List<ZonesHitData> zonesInPath = new();

            for (int i = 0; i < zonesLength; i++)
            {
                ZonesHitData zoneHitData = zones[i];
                ZoneSidesHitHelper zoneSidesHit = GetZoneSidesHitByPath(zoneHitData.Zone, previousX, previousY, newX, newY);

                if (zoneSidesHit.BottomHit || zoneSidesHit.TopHit || zoneSidesHit.LeftHit || zoneSidesHit.RightHit)
                {
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
        /// Checks if the x and y position of the previous point and new point are within a zone
        /// </summary>
        /// <param name="zones">The active zones</param>
        /// <param name="zonesLength">The length of the zones</param>
        /// <param name="x">The new x position</param>
        /// <param name="y">The new y position</param>
        /// <returns>The zone where the previous and new x and y positions are in, null if the points are not within the zone</returns>
        public static ZonesHitData GetZoneWherePathIsInside(List<ZonesHitData> zones, int zonesLength, int x, int y)
        {
            LaserMessage previousMessage = LaserConnectionLogic.PreviousLaserMessage;
            for (int i = 0; i < zonesLength; i++)
            {
                ZonesHitData zoneHitData = zones[i];
                bool positionIsWithinZone = previousMessage.X.IsBetweenOrEqualTo(zoneHitData.ZoneAbsolutePositions.LeftXAxisInZone, zoneHitData.ZoneAbsolutePositions.RightXAxisInZone) &&
                                            previousMessage.Y.IsBetweenOrEqualTo(zoneHitData.ZoneAbsolutePositions.LowestYAxisInZone, zoneHitData.ZoneAbsolutePositions.HighestYAxisInZone) &&
                                            x.IsBetweenOrEqualTo(zoneHitData.ZoneAbsolutePositions.LeftXAxisInZone, zoneHitData.ZoneAbsolutePositions.RightXAxisInZone) &&
                                            y.IsBetweenOrEqualTo(zoneHitData.ZoneAbsolutePositions.LowestYAxisInZone, zoneHitData.ZoneAbsolutePositions.HighestYAxisInZone);

                if (positionIsWithinZone)
                {
                    return zoneHitData;
                }
            }

            return null;
        }

        public static ZoneDto GetZoneWhereMaxLaserPowerPwmIsHighest(List<ZonesHitData> zonesHitDataCollection)
        {
            return zonesHitDataCollection
                .Select(zone => zone.Zone)
                .MaxBy(zone => zone.MaxLaserPowerInZonePwm);
        }
    }
}
