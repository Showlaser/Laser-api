using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserLogic
    {
        private readonly List<ZonesHitDataHelper> _zones;
        private readonly int _zonesLength;
        private readonly bool _safetyZonesNotActive;
        private readonly ZoneDto _zoneWithHighestLaserPowerPwm;

        public LaserLogic(IZoneDal zoneDal)
        {
            List<ZoneDto> zones = zoneDal
                .All()
                .Result;

            List<ZonesHitDataHelper> zonesHitDataCollection = new();
            foreach (ZoneDto zone in zones)
            {
                zonesHitDataCollection.Add(new ZonesHitDataHelper
                {
                    Zone = zone,
                    ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(zone),
                });
            }
            _zones = zonesHitDataCollection;
            _safetyZonesNotActive = !zones.Any();
            _zonesLength = _zones.Count;
            _zoneWithHighestLaserPowerPwm = ZonesHelper.GetZoneWhereMaxLaserPowerPwmIsHighest(_zones.ToList());
        }

        public async Task SendData(LaserMessage message)
        {
            int totalLaserPowerPwm = message.RedLaser + message.GreenLaser + message.BlueLaser;
            bool skipSafetyZoneCheck = _safetyZonesNotActive || totalLaserPowerPwm == 0 ||
                                       _zoneWithHighestLaserPowerPwm.MaxLaserPowerInZonePwm >= totalLaserPowerPwm;

            if (skipSafetyZoneCheck)
            {
                await LaserConnectionLogic.SendMessage(message);
                return;
            }

            ZonesHitDataHelper zoneHitData = ZonesHelper.GetZoneWherePositionIsIn(_zones, _zonesLength, message.X, message.Y);
            bool positionIsInSafetyZone = zoneHitData != null;

            if (positionIsInSafetyZone)
            {
                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref message, zoneHitData.Zone.MaxLaserPowerInZonePwm);
                await LaserConnectionLogic.SendMessage(message);
                return;
            }

            int zonesCrossedDataLength = 0;
            List<ZonesHitDataHelper> zonesCrossedData =
                    ZonesHelper.GetZonesInPathOfPosition(_zones, LaserConnectionLogic.LastXPosition, LaserConnectionLogic.LastYPosition,
                        message.X, message.Y, ref zonesCrossedDataLength);

            if (zonesCrossedDataLength != 0)
            {
                await HandleZonesBetweenLaserCoordinates(message, zonesCrossedData, totalLaserPowerPwm);
                return;
            }

            await LaserConnectionLogic.SendMessage(message);
        }

        private static async Task HandleZonesBetweenLaserCoordinates(LaserMessage originalMessage,
            List<ZonesHitDataHelper> zonesCrossedData, int totalLaserPowerPwm)
        {
            List<LaserMessage> newLaserMessageCollection = new();
            int zonesCrossedCount = zonesCrossedData.Count;

            for (int i = 0; i < zonesCrossedCount; i++)
            {
                ZonesHitDataHelper crossedZoneData = zonesCrossedData[i];
                double laserPowerLimitFactor = (double)totalLaserPowerPwm / crossedZoneData.Zone.MaxLaserPowerInZonePwm;
                if (laserPowerLimitFactor <= 1)
                {
                    continue; // skip check if power is lower or equal to the max in safety zone
                }

                zonesCrossedData[i].ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(crossedZoneData.Zone);
                crossedZoneData.ZoneSidesHit.GetCoordinateOfZoneCrossing(originalMessage, crossedZoneData, LaserConnectionLogic.LastXPosition,
                    LaserConnectionLogic.LastYPosition, ref newLaserMessageCollection);
            }

            await SortAndSendMessagesByClosedPoint(newLaserMessageCollection, originalMessage, newLaserMessageCollection.Count);
        }

        private static async Task SortAndSendMessagesByClosedPoint(List<LaserMessage> messages, LaserMessage startPoint, int messageCount)
        {
            int differenceInYAxis = NumberHelper.GetHighestNumber(startPoint.Y, LaserConnectionLogic.LastXPosition) -
                                    NumberHelper.GetLowestNumber(startPoint.Y, LaserConnectionLogic.LastXPosition);
            int differenceInXAxis = NumberHelper.GetHighestNumber(startPoint.X, LaserConnectionLogic.LastXPosition) -
                                    NumberHelper.GetLowestNumber(startPoint.X, LaserConnectionLogic.LastXPosition);

            bool checkByXAxis = differenceInXAxis > differenceInYAxis;
            if (messageCount > 1) // performance optimization
            {
                for (int i = 0; i < messageCount; i++)
                {
                    int closestMessageIndex = GetMessageIndexClosestToPoint(messages, messageCount, checkByXAxis);
                    LaserMessage messageToSend = messages[closestMessageIndex];
                    await LaserConnectionLogic.SendMessage(messageToSend);

                    messages.RemoveAt(closestMessageIndex);
                    messageCount--;
                }
            }
            else
            {
                LaserMessage messageToSend = messages[0];
                await LaserConnectionLogic.SendMessage(messageToSend);
            }

            await LaserConnectionLogic.SendMessage(startPoint);
        }

        /// <summary>
        /// Gets the index of the messages that is closest to the to the last point
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="messageLength"></param>
        /// <param name="checkByXAxis"></param>
        /// <returns>The index of the closest messages</returns>
        private static int GetMessageIndexClosestToPoint(IReadOnlyList<LaserMessage> messages, int messageLength, bool checkByXAxis)
        {
            int closestMessageIndex = 0;
            for (int i = 1; i < messageLength; i++) // start at 1 since first element is already used
            {
                LaserMessage message = messages[i];
                LaserMessage closestMessage = messages[closestMessageIndex];

                bool xAxisClosest = checkByXAxis && LaserConnectionLogic.LastXPosition - message.X <
                    LaserConnectionLogic.LastXPosition - closestMessage.X;
                bool yAxisClosest = !checkByXAxis && LaserConnectionLogic.LastYPosition - message.Y <
                    LaserConnectionLogic.LastYPosition - closestMessage.Y;

                if (xAxisClosest || yAxisClosest)
                {
                    closestMessageIndex = i;
                }
            }

            return closestMessageIndex;
        }
    }
}
