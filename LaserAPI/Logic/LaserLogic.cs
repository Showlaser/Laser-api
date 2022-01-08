using LaserAPI.Interfaces.Dal;
using LaserAPI.Interfaces.Helper;
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
        private readonly ILaserConnectionLogic _laserConnectionLogic;
        private readonly List<ZoneDto> _zones;
        private readonly bool _safetyZonesNotActive;
        private int _lastXPosition;
        private int _lastYPosition;
        private readonly ZoneDto _zoneWithHighestLaserPowerPwm;

        public LaserLogic(ILaserConnectionLogic laserConnectionLogic, IZoneDal zoneDal)
        {
            _laserConnectionLogic = laserConnectionLogic;
            _zones = zoneDal.All().Result;
            _safetyZonesNotActive = _zones.Any();
            _zoneWithHighestLaserPowerPwm = ZonesHelper.GetZoneWhereMaxLaserPowerPwmIsHighest(_zones);
        }

        public void SendData(LaserMessage message)
        {
            int totalLaserPowerPwm = message.RedLaser + message.GreenLaser + message.BlueLaser;
            _lastXPosition = _laserConnectionLogic.GetLastXPosition();
            _lastYPosition = _laserConnectionLogic.GetLastYPosition();

            if (_safetyZonesNotActive || totalLaserPowerPwm == 0 || _zoneWithHighestLaserPowerPwm.MaxLaserPowerInZonePwm <= totalLaserPowerPwm)
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
                return;
            }

            List<ZonesHitDataHelper> zonesCrossedData =
                ZonesHelper.GetZonesInPathOfPosition(_zones, _lastXPosition, _lastYPosition, message.X, message.Y);

            if (zonesCrossedData.Any())
            {
                HandleZonesBetweenLaserCoordinates(message, zonesCrossedData, totalLaserPowerPwm, zone);
                return;
            }

            _laserConnectionLogic.SendMessage(message);
        }

        private void HandleZonesBetweenLaserCoordinates(LaserMessage message, List<ZonesHitDataHelper> zonesCrossedData, int totalLaserPowerPwm,
            ZoneDto zone)
        {
            List<LaserMessage> newLaserMessageCollection = new();
            int zonesCrossedCount = zonesCrossedData.Count;

            for (int i = 0; i < zonesCrossedCount; i++)
            {
                ZonesHitDataHelper crossedZoneData = zonesCrossedData[i];
                double laserPowerLimitFactor = (double)totalLaserPowerPwm / zone.MaxLaserPowerInZonePwm;
                if (laserPowerLimitFactor <= 1)
                {
                    continue; // skip check if power is lower or equal to the max in safety zone
                }

                ZoneAbsolutePositionsHelper absolutePositions = new(crossedZoneData.Zone);
                crossedZoneData.ZoneSidesHit.GetCoordinateOfZoneCrossing(message, crossedZoneData, _lastXPosition,
                    _lastYPosition);

                for (int j = 0; j < 2; j++) // checking two times because only two sides of a rectangle can be hit by a line
                {
                    List<LaserMessage> zoneMessage = GenerateMessageOnZoneEdge(zone, crossedZoneData, absolutePositions);
                    newLaserMessageCollection.AddRange(zoneMessage);
                }
            }

            SendNewLaserMessage(newLaserMessageCollection, message, newLaserMessageCollection.Count);
        }

        private static List<LaserMessage> GenerateMessageOnZoneEdge(ZoneDto zone, ZonesHitDataHelper crossedZoneData,
            ZoneAbsolutePositionsHelper absolutePositions)
        {
            List<LaserMessage> messages = new();
            if (crossedZoneData.ZoneSidesHit.TopHit)
            {
                LaserMessage newMessage = new()
                {
                    X = crossedZoneData.ZoneSidesHit.TopXAxis,
                    Y = absolutePositions.HighestYAxisInZone,
                };

                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref newMessage, zone.MaxLaserPowerInZonePwm);
                messages.Add(newMessage);
            }

            if (crossedZoneData.ZoneSidesHit.BottomHit)
            {
                LaserMessage newMessage = new()
                {
                    X = crossedZoneData.ZoneSidesHit.BottomXAxis,
                    Y = absolutePositions.LowestYAxisInZone,
                };

                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref newMessage, zone.MaxLaserPowerInZonePwm);
                messages.Add(newMessage);
            }

            if (crossedZoneData.ZoneSidesHit.LeftHit)
            {
                LaserMessage newMessage = new()
                {
                    X = absolutePositions.LeftXAxisInZone,
                    Y = crossedZoneData.ZoneSidesHit.LeftYAxis
                };

                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref newMessage, zone.MaxLaserPowerInZonePwm);
                messages.Add(newMessage);
            }

            if (crossedZoneData.ZoneSidesHit.RightHit)
            {
                LaserMessage newMessage = new()
                {
                    X = absolutePositions.RightXAxisInZone,
                    Y = crossedZoneData.ZoneSidesHit.RightYAxis
                };

                LaserSafetyHelper.LimitLaserPowerIfNecessary(ref newMessage, zone.MaxLaserPowerInZonePwm);
                messages.Add(newMessage);
            }

            return messages;
        }

        private void SendNewLaserMessage(List<LaserMessage> newMessages, LaserMessage originalMessage, int messageCount)
        {
            List<LaserMessage> sortedList = SortMessagesByClosedPoint(newMessages, originalMessage, messageCount);
            for (int i = 0; i < messageCount; i++)
            {
                LaserMessage message = sortedList[i];
                _laserConnectionLogic.SendMessage(message);
            }
        }

        private List<LaserMessage> SortMessagesByClosedPoint(List<LaserMessage> messages, LaserMessage startPoint, int messageCount)
        {
            int differenceInYAxis = NumberHelper.GetHighestNumber(startPoint.Y, _lastXPosition) -
                                    NumberHelper.GetLowestNumber(startPoint.Y, _lastXPosition);
            int differenceInXAxis = NumberHelper.GetHighestNumber(startPoint.X, _lastXPosition) -
                                    NumberHelper.GetLowestNumber(startPoint.X, _lastXPosition);

            List<LaserMessage> sortedList = new();
            bool checkByXAxis = differenceInXAxis > differenceInYAxis;

            for (int i = 0; i < messageCount; i++)
            {
                LaserMessage closestMessage = GetMessageClosestToPoint(messages, checkByXAxis);
                sortedList.Add(closestMessage);
                messages.Remove(closestMessage);
            }

            return sortedList;
        }

        private LaserMessage GetMessageClosestToPoint(List<LaserMessage> messages, bool checkByXAxis)
        {
            LaserMessage closestMessage = messages[0];
            int messageLength = messages.Count;

            for (int i = 1; i < messageLength; i++) // start at 1 since first element is already used
            {
                LaserMessage message = messages[i];
                if (checkByXAxis && _lastXPosition - message.X < _lastXPosition - closestMessage.X)
                {
                    closestMessage = message;
                }
                if (!checkByXAxis && _lastYPosition - message.Y < _lastYPosition - closestMessage.Y)
                {
                    closestMessage = message;
                }
            }

            return closestMessage;
        }
    }
}
