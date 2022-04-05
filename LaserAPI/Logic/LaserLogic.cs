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
    public class LaserLogic
    {
        private readonly List<ZonesHitData> _zones;
        private readonly int _zonesLength;
        private readonly bool _safetyZonesNotActive;
        private readonly ZoneDto _zoneWithHighestLaserPowerPwm;

        public LaserLogic(IZoneDal zoneDal)
        {
            List<ZoneDto> zones = zoneDal
                .All()
                .Result;

            List<ZonesHitData> zonesHitDataCollection = new();
            foreach (ZoneDto zone in zones)
            {
                zonesHitDataCollection.Add(new ZonesHitData
                {
                    Zone = zone,
                    ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(zone),
                });
            }
            _zones = zonesHitDataCollection;
            _safetyZonesNotActive = !zones.Any();
            _zonesLength = _zones.Count;
            _zoneWithHighestLaserPowerPwm = ZoneLogic.GetZoneWhereMaxLaserPowerPwmIsHighest(_zones.ToList());
        }

        public async Task SendData(LaserMessage newMessage)
        {
            int totalLaserPowerPwm = newMessage.RedLaser + newMessage.GreenLaser + newMessage.BlueLaser;
            bool skipSafetyZoneCheck = _safetyZonesNotActive || totalLaserPowerPwm == 0 ||
                                       _zoneWithHighestLaserPowerPwm.MaxLaserPowerInZonePwm >= totalLaserPowerPwm;

            if (skipSafetyZoneCheck)
            {
                await LaserConnectionLogic.SendMessage(newMessage);
                return;
            }

            ZonesHitData zoneHitData = ZoneLogic.GetZoneWherePathIsInside(_zones, _zonesLength, newMessage.X, newMessage.Y);
            bool positionIsInSafetyZone = zoneHitData != null;

            if (positionIsInSafetyZone)
            {
                LaserLogic.LimitTotalLaserPowerIfNecessary(ref newMessage, zoneHitData.Zone.MaxLaserPowerInZonePwm);
                await LaserConnectionLogic.SendMessage(newMessage);
                return;
            }

            IEnumerable<LaserMessage> messagesToSend = GetPathWithSafetyZonePositions(newMessage);
            foreach (LaserMessage message in messagesToSend)
            {
                await LaserConnectionLogic.SendMessage(message);
            }
        }

        /// <summary>
        /// Gets the positions from the edges of the zones that are crossed and the new message position sorted
        /// </summary>
        /// <param name="newMessage">The new incoming message</param>
        /// <returns>A IEnumerable with the new message point and the positions of the zones that are crossed sorted</returns>
        private IEnumerable<LaserMessage> GetPathWithSafetyZonePositions(LaserMessage newMessage)
        {
            List<ZonesHitData> zonesCrossedData =
                ZoneLogic.GetZonesInPathOfPosition(_zones, LaserConnectionLogic.PreviousLaserMessage.X,
                    LaserConnectionLogic.PreviousLaserMessage.Y,
                    newMessage.X, newMessage.Y);

            int zonesCrossedDataCount = zonesCrossedData.Count;
            if (zonesCrossedDataCount != 0)
            {
                List<LaserMessage> messagesToSort = GetMessagesOnZonesEdge(newMessage,
                    zonesCrossedData, zonesCrossedDataCount);

                messagesToSort.Add(newMessage);
                return messagesToSort
                    .OrderByDescending(m => m.Y)
                    .ThenByDescending(m => m.X);
            }

            return new List<LaserMessage> { newMessage };
        }

        /// <summary>
        /// Gets all messages at the edge of the zones that are crossed
        /// </summary>
        /// <param name="newMessage">The new message</param>
        /// <param name="zonesCrossedData">The info of the zones that are crossed</param>
        /// <param name="zonesCrossedCount">The length of the zonesCrossedData list</param>
        /// <returns>A readonly list with the messages that contain the positions of the edges of the zones that are crossed</returns>
        public static List<LaserMessage> GetMessagesOnZonesEdge(LaserMessage newMessage,
            List<ZonesHitData> zonesCrossedData, int zonesCrossedCount)
        {
            int totalLaserPowerPwm = newMessage.RedLaser + newMessage.GreenLaser + newMessage.BlueLaser;
            List<LaserMessage> messagesOnZonesEdge = new();
            for (int i = 0; i < zonesCrossedCount; i++)
            {
                ZonesHitData crossedZoneData = zonesCrossedData[i];
                if (totalLaserPowerPwm <= crossedZoneData.Zone.MaxLaserPowerInZonePwm)
                {
                    continue; // skip check if power is lower or equal to the max in safety zone
                }

                zonesCrossedData[i].ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(crossedZoneData.Zone);
                crossedZoneData.ZoneSidesHit.GetMissingXOrYCoordinateOfZoneCrossing(newMessage, crossedZoneData, LaserConnectionLogic.PreviousLaserMessage.X,
                    LaserConnectionLogic.PreviousLaserMessage.Y, ref messagesOnZonesEdge);
            }

            return messagesOnZonesEdge;
        }

        /// <summary>
        /// Limits the pwm laser power per laser color if the value is greater than the max power per laser color
        /// </summary>
        /// <param name="message">The message to modify</param>
        /// <param name="maxPowerPwmPerLaserColor">The max power allowed in PWM value per laser</param>
        public static void LimitLaserPowerPerLaserIfNecessary(ref LaserMessage message, int maxPowerPwmPerLaserColor)
        {
            if (message.RedLaser > maxPowerPwmPerLaserColor)
            {
                message.RedLaser = NumberHelper.Map(message.RedLaser, 0, 255, 0, maxPowerPwmPerLaserColor);
            }
            if (message.GreenLaser > maxPowerPwmPerLaserColor)
            {
                message.GreenLaser = NumberHelper.Map(message.GreenLaser, 0, 255, 0, maxPowerPwmPerLaserColor);
            }
            if (message.BlueLaser > maxPowerPwmPerLaserColor)
            {
                message.BlueLaser = NumberHelper.Map(message.BlueLaser, 0, 255, 0, maxPowerPwmPerLaserColor);
            }
        }

        /// <summary>
        /// Limits the pwm laser power if the value is greater than the max power
        /// </summary>
        /// <param name="message">The message to modify</param>
        /// <param name="maxPowerPwm">The max power allowed</param>
        public static void LimitTotalLaserPowerIfNecessary(ref LaserMessage message, int maxPowerPwm)
        {
            int combinedPower = message.RedLaser + message.GreenLaser + message.BlueLaser;
            if (combinedPower > maxPowerPwm)
            {
                // ReSharper disable once PossibleLossOfFraction
                double maxPowerPerColor = NumberHelper.Map(combinedPower, 0, 765, 0, maxPowerPwm) / 3;
                message.RedLaser = NumberHelper.Map(message.RedLaser, 0, 255, 0, Convert.ToInt32(maxPowerPerColor));
                message.GreenLaser = NumberHelper.Map(message.GreenLaser, 0, 255, 0, Convert.ToInt32(maxPowerPerColor));
                message.BlueLaser = NumberHelper.Map(message.BlueLaser, 0, 255, 0, Convert.ToInt32(maxPowerPerColor));
            }
        }
    }
}
