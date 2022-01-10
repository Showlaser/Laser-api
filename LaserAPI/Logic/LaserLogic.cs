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
        private readonly ZoneDto[] _zones;
        private readonly bool _safetyZonesNotActive;
        private int _lastXPosition;
        private int _lastYPosition;
        private readonly ZoneDto _zoneWithHighestLaserPowerPwm;

        public LaserLogic(LaserConnectionLogic laserConnectionLogic, IZoneDal zoneDal)
        {
            _laserConnectionLogic = laserConnectionLogic;
            _zones = zoneDal
                .All()
                .Result
                .ToArray();

            _safetyZonesNotActive = !_zones.Any();
            _zoneWithHighestLaserPowerPwm = ZonesHelper.GetZoneWhereMaxLaserPowerPwmIsHighest(_zones.ToList());
        }

        private void SetLastPositions(LaserMessage message)
        {
            _lastXPosition = message.X;
            _lastYPosition = message.Y;
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

            ZoneDto zone = ZonesHelper.GetZoneWherePositionIsIn(_zones, message.X, message.Y);
            bool positionIsInSafetyZone = zone != null;

            if (positionIsInSafetyZone)
            {
                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref message, zone.MaxLaserPowerInZonePwm);
                _laserConnectionLogic.SendMessage(message);
                SetLastPositions(message);
                return;
            }

            int zonesCrossedDataLength = 0;
            ZonesHitDataHelper[] zonesCrossedData =
                    ZonesHelper.GetZonesInPathOfPosition(_zones, _lastXPosition, _lastYPosition,
                        message.X, message.Y, ref zonesCrossedDataLength);

            if (zonesCrossedDataLength != 0)
            {
                HandleZonesBetweenLaserCoordinates(message, zonesCrossedData, totalLaserPowerPwm);

                return;
            }

            _laserConnectionLogic.SendMessage(message);
            SetLastPositions(message);
        }

        private void HandleZonesBetweenLaserCoordinates(LaserMessage originalMessage, ZonesHitDataHelper[] zonesCrossedData, int totalLaserPowerPwm)
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
                crossedZoneData.ZoneSidesHit.GetCoordinateOfZoneCrossing(originalMessage, crossedZoneData, _lastXPosition,
                    _lastYPosition, ref newLaserMessageCollection);
            }

            SendNewLaserMessage(newLaserMessageCollection, originalMessage, newLaserMessageCollection.Count);
        }

        private void SendNewLaserMessage(List<LaserMessage> newMessages, LaserMessage originalMessage, int messageCount)
        {
            IReadOnlyList<LaserMessage> sortedList = SortMessagesByClosedPoint(newMessages, originalMessage, messageCount);
            for (int i = 0; i < messageCount; i++)
            {
                LaserMessage message = sortedList[i];
                _laserConnectionLogic.SendMessage(message);
            }

            SetLastPositions(sortedList[messageCount - 1]);
        }

        private IReadOnlyList<LaserMessage> SortMessagesByClosedPoint(List<LaserMessage> messages, LaserMessage startPoint, int messageCount)
        {
            int differenceInYAxis = NumberHelper.GetHighestNumber(startPoint.Y, _lastXPosition) -
                                    NumberHelper.GetLowestNumber(startPoint.Y, _lastXPosition);
            int differenceInXAxis = NumberHelper.GetHighestNumber(startPoint.X, _lastXPosition) -
                                    NumberHelper.GetLowestNumber(startPoint.X, _lastXPosition);

            bool checkByXAxis = differenceInXAxis > differenceInYAxis;
            List<LaserMessage> sortedMessages = new();

            if (messageCount > 1) // performance optimization
            {
                for (int i = 0; i < messageCount; i++)
                {
                    int closestMessageIndex = GetMessageIndexClosestToPoint(messages, messageCount, checkByXAxis);
                    sortedMessages.Add(messages[closestMessageIndex]);

                    messages.RemoveAt(closestMessageIndex);
                    messageCount--;
                }
            }
            else
            {
                sortedMessages.Add(messages[0]);
            }

            sortedMessages.Add(startPoint);
            return sortedMessages;
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
                    case true when _lastXPosition - message.X < _lastXPosition - closestMessage.X:
                    case false when _lastYPosition - message.Y < _lastYPosition - closestMessage.Y:
                        closestMessageIndex = i;
                        break;
                }
            }

            return closestMessageIndex;
        }
    }
}
