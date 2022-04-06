using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Zones;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (zone.Positions.Count != 4)
            {
                throw new InvalidDataException();
            }
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

        public static List<ZonesHitData> GetZonesInPathOfPosition(List<ZonesHitData> zones, LaserMessage newMessage)
        {
            int previousX = LaserConnectionLogic.PreviousLaserMessage.X;
            int previousY = LaserConnectionLogic.PreviousLaserMessage.Y;
            int newX = newMessage.X;
            int newY = newMessage.Y;
            
            int zonesLength = zones.Count;
            List<ZonesHitData> zonesInPath = new();

            for (int i = 0; i < zonesLength; i++)
            {
                ZonesHitData zoneHitData = zones[i];
                ZoneSidesHitHelper zoneSidesHit = GetZoneSidesHitByPath(zoneHitData.Zone, newX, newY);

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
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <returns>The zones that are crossed</returns>
        private static ZoneSidesHitHelper GetZoneSidesHitByPath(ZoneDto zone, int newX,
            int newY)
        {
            int previousX = LaserConnectionLogic.PreviousLaserMessage.X;
            int previousY = LaserConnectionLogic.PreviousLaserMessage.Y;

            ZoneAbsolutePositionsHelper absolutePositionsHelper = new(zone);
            return new ZoneSidesHitHelper
            {
                LeftHit = LeftHit(absolutePositionsHelper, newX, newY, previousX, previousY),
                RightHit = RightHit(absolutePositionsHelper, newX, newY, previousX, previousY),
                BottomHit = BottomHit(absolutePositionsHelper, newX, newY, previousX, previousY),
                TopHit = TopHit(absolutePositionsHelper, newX, newY, previousX, previousY)
            };
        }

        private static bool LeftHit(ZoneAbsolutePositionsHelper absolutePositions, int newX, int newY, int previousX, int previousY)
        {
            bool isBetweenLowestAndHighestPosition =
                newY.IsBetweenOrEqualToWithMinMaxCheck(absolutePositions.LowestYAxisInZone,
                    absolutePositions.HighestYAxisInZone) || previousY.IsBetweenOrEqualToWithMinMaxCheck(absolutePositions.LowestYAxisInZone,
                    absolutePositions.HighestYAxisInZone);

            return absolutePositions.LeftXAxisInZone.IsBetweenOrEqualToWithMinMaxCheck(previousX, newX) &&
                   isBetweenLowestAndHighestPosition;

        }

        private static bool RightHit(ZoneAbsolutePositionsHelper absolutePositions, int newX, int newY, int previousX, int previousY)
        {
            bool isBetweenLowestAndHighestPosition =
                newY.IsBetweenOrEqualToWithMinMaxCheck(absolutePositions.LowestYAxisInZone,
                    absolutePositions.HighestYAxisInZone) || previousY.IsBetweenOrEqualToWithMinMaxCheck(absolutePositions.LowestYAxisInZone,
                    absolutePositions.HighestYAxisInZone);

            return absolutePositions.RightXAxisInZone.IsBetweenOrEqualToWithMinMaxCheck(previousX, newX) &&
                   isBetweenLowestAndHighestPosition;
        }

        private static bool TopHit(ZoneAbsolutePositionsHelper absolutePositions, int newX, int newY, int previousX, int previousY)
        {
            bool isBetweenLeftAndRight =
                newX.IsBetweenOrEqualToWithMinMaxCheck(absolutePositions.LeftXAxisInZone,
                    absolutePositions.RightXAxisInZone) || previousX.IsBetweenOrEqualToWithMinMaxCheck(absolutePositions.LeftXAxisInZone,
                    absolutePositions.RightXAxisInZone);

            return absolutePositions.HighestYAxisInZone.IsBetweenOrEqualToWithMinMaxCheck(previousY, newY) &&
                   isBetweenLeftAndRight;
        }

        private static bool BottomHit(ZoneAbsolutePositionsHelper absolutePositions, int newX, int newY, int previousX, int previousY)
        {
            bool isBetweenLeftAndRight =
                newX.IsBetweenOrEqualToWithMinMaxCheck(absolutePositions.LeftXAxisInZone,
                    absolutePositions.RightXAxisInZone) || previousX.IsBetweenOrEqualToWithMinMaxCheck(absolutePositions.LeftXAxisInZone,
                    absolutePositions.RightXAxisInZone);

            return absolutePositions.LowestYAxisInZone.IsBetweenOrEqualToWithMinMaxCheck(previousY, newY) &&
                   isBetweenLeftAndRight;
        }

        public static ZonesHitData GetZoneClosestToMessage(LaserMessage message, List<ZonesHitData> zones, int zonesLength)
        {
            ClosestPosition closestZonePosition = new();
            for (int i = 0; i < zonesLength; i++)
            {
                ZonesHitData zoneHitData = zones[i];
                ZoneDto zone = zoneHitData.Zone;
                int difference = GetXAndYDifferenceBetweenClosestPositionToMessage(zone, message);

                if (difference < closestZonePosition.XAndYPositionCombined)
                {
                    closestZonePosition.XAndYPositionCombined = difference;
                    closestZonePosition.Index = i;
                }
            }

            return zones[closestZonePosition.Index];
        }

        public static LaserMessage PositionMessageIntoZone(LaserMessage message, ZonesHitData zoneHitData)
        {
            ZoneAbsolutePositionsHelper absolutePositions = zoneHitData.ZoneAbsolutePositions;
            if (!message.X.IsBetweenOrEqualTo(absolutePositions.LeftXAxisInZone, absolutePositions.RightXAxisInZone))
            {
                message.X = NumberHelper.Map(message.X, -4000, 4000, zoneHitData.ZoneAbsolutePositions.LeftXAxisInZone,
                    zoneHitData.ZoneAbsolutePositions.RightXAxisInZone);
            }
            if (!message.Y.IsBetweenOrEqualTo(absolutePositions.LowestYAxisInZone, absolutePositions.HighestYAxisInZone))
            {
                message.Y = NumberHelper.Map(message.Y, -4000, 4000, zoneHitData.ZoneAbsolutePositions.LowestYAxisInZone,
                    zoneHitData.ZoneAbsolutePositions.HighestYAxisInZone);
            }

            return message;
        }

        /// <summary>
        /// Find the difference between the message positions and the zone positions. The difference of the closest position to the zone will be returned
        /// </summary>
        /// <param name="zone">The zone to search into</param>
        /// <param name="message">The message send by the user</param>
        /// <returns>The difference between the closest position to the message and the difference between the x and y position added</returns>
        private static int GetXAndYDifferenceBetweenClosestPositionToMessage(ZoneDto zone, LaserMessage message)
        {
            ClosestPosition closestZonePositionToMessage = new() {
                Index = 0,
                XAndYPositionCombined = 8000
            };

            for (int j = 0; j < 4; j++)
            {
                ZonesPositionDto zonePosition = zone.Positions[j];
                int xDifference = NumberHelper.GetDifferenceBetweenTwoNumbers(zonePosition.X, message.X);
                int yDifference = NumberHelper.GetDifferenceBetweenTwoNumbers(zonePosition.Y, message.Y);
                int totalDifference = xDifference + yDifference;

                if (closestZonePositionToMessage.XAndYPositionCombined > totalDifference)
                {
                    closestZonePositionToMessage.XAndYPositionCombined = totalDifference;
                    closestZonePositionToMessage.Index = j;
                }
            }

            return closestZonePositionToMessage.XAndYPositionCombined;
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
