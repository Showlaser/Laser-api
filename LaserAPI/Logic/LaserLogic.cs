using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Logic
{
    public class LaserLogic
    {
        private readonly LaserConnectionLogic _laserConnectionLogic;
        private readonly ZonesHitDataHelper[] _zones;
        private readonly int _zonesLength;
        private readonly bool _safetyZonesNotActive;
        private readonly ZoneDto _zoneWithHighestLaserPowerPwm;

        public LaserLogic(LaserConnectionLogic laserConnectionLogic, IZoneDal zoneDal)
        {
            _laserConnectionLogic = laserConnectionLogic;
            ZoneDto[] zones = zoneDal
                .All()
                .Result
                .ToArray();

            List<ZonesHitDataHelper> zonesHitDataCollection = new();
            foreach (var zone in zones)
            {
                zonesHitDataCollection.Add(new ZonesHitDataHelper
                {
                    Zone = zone,
                    ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(zone),
                });
            }
            _zones = zonesHitDataCollection.ToArray();
            _safetyZonesNotActive = !zones.Any();
            _zonesLength = _zones.Length;
            _zoneWithHighestLaserPowerPwm = ZonesHelper.GetZoneWhereMaxLaserPowerPwmIsHighest(_zones.ToList());
        }

        public void SendData(LaserMessage message)
        {
            int totalLaserPowerPwm = message.RedLaser + message.GreenLaser + message.BlueLaser;
            bool skipSafetyZoneCheck = _safetyZonesNotActive || totalLaserPowerPwm == 0 ||
                                       _zoneWithHighestLaserPowerPwm.MaxLaserPowerInZonePwm >= totalLaserPowerPwm;

            if (skipSafetyZoneCheck)
            {
                _laserConnectionLogic.SendMessage(message);
                return;
            }

            ZonesHitDataHelper zoneHitData = ZonesHelper.GetZoneWherePositionIsIn(_zones, _zonesLength, message.X, message.Y);
            bool positionIsInSafetyZone = zoneHitData != null;

            if (positionIsInSafetyZone)
            {
                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref message, zoneHitData.Zone.MaxLaserPowerInZonePwm);
                _laserConnectionLogic.SendMessage(message);
                return;
            }

            int zonesCrossedDataLength = 0;
            ZonesHitDataHelper[] zonesCrossedData =
                    ZonesHelper.GetZonesInPathOfPosition(_zones, _laserConnectionLogic.LastXPosition, _laserConnectionLogic.LastYPosition,
                        message.X, message.Y, ref zonesCrossedDataLength);

            if (zonesCrossedDataLength != 0)
            {
                HandleZonesBetweenLaserCoordinates(message, zonesCrossedData, totalLaserPowerPwm);
                return;
            }

            _laserConnectionLogic.SendMessage(message);
        }

        private void HandleZonesBetweenLaserCoordinates(LaserMessage originalMessage,
            ZonesHitDataHelper[] zonesCrossedData, int totalLaserPowerPwm)
        {
            List<LaserMessage> newLaserMessageCollection = new();
            int zonesCrossedCount = zonesCrossedData.Length;

            for (int i = 0; i < zonesCrossedCount; i++)
            {
                ZonesHitDataHelper crossedZoneData = zonesCrossedData[i];
                double laserPowerLimitFactor = (double)totalLaserPowerPwm / crossedZoneData.Zone.MaxLaserPowerInZonePwm;
                if (laserPowerLimitFactor <= 1)
                {
                    continue; // skip check if power is lower or equal to the max in safety zone
                }

                zonesCrossedData[i].ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(crossedZoneData.Zone);
                crossedZoneData.ZoneSidesHit.GetCoordinateOfZoneCrossing(originalMessage, crossedZoneData, _laserConnectionLogic.LastXPosition,
                    _laserConnectionLogic.LastYPosition, ref newLaserMessageCollection);
            }

            SortAndSendMessagesByClosedPoint(newLaserMessageCollection, originalMessage, newLaserMessageCollection.Count);
        }

        private void SortAndSendMessagesByClosedPoint(List<LaserMessage> messages, LaserMessage startPoint, int messageCount)
        {
            int differenceInYAxis = NumberHelper.GetHighestNumber(startPoint.Y, _laserConnectionLogic.LastXPosition) -
                                    NumberHelper.GetLowestNumber(startPoint.Y, _laserConnectionLogic.LastXPosition);
            int differenceInXAxis = NumberHelper.GetHighestNumber(startPoint.X, _laserConnectionLogic.LastXPosition) -
                                    NumberHelper.GetLowestNumber(startPoint.X, _laserConnectionLogic.LastXPosition);

            bool checkByXAxis = differenceInXAxis > differenceInYAxis;
            if (messageCount > 1) // performance optimization
            {
                for (int i = 0; i < messageCount; i++)
                {
                    int closestMessageIndex = GetMessageIndexClosestToPoint(messages, messageCount, checkByXAxis);
                    LaserMessage messageToSend = messages[closestMessageIndex];
                    _laserConnectionLogic.SendMessage(messageToSend);

                    messages.RemoveAt(closestMessageIndex);
                    messageCount--;
                }
            }
            else
            {
                LaserMessage messageToSend = messages[0];
                _laserConnectionLogic.SendMessage(messageToSend);
            }

            _laserConnectionLogic.SendMessage(startPoint);
        }

        private int GetMessageIndexClosestToPoint(IReadOnlyList<LaserMessage> messages, int messageLength, bool checkByXAxis)
        {
            int closestMessageIndex = 0;
            for (int i = 1; i < messageLength; i++) // start at 1 since first element is already used
            {
                LaserMessage message = messages[i];
                LaserMessage closestMessage = messages[closestMessageIndex];

                switch (checkByXAxis)
                {
                    case true when _laserConnectionLogic.LastXPosition - message.X < _laserConnectionLogic.LastXPosition - closestMessage.X:
                    case false when _laserConnectionLogic.LastYPosition - message.Y < _laserConnectionLogic.LastYPosition - closestMessage.Y:
                        closestMessageIndex = i;
                        break;
                }
            }

            return closestMessageIndex;
        }
    }
}
