using System;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Zones;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaserAPI.Models.FromFrontend.Zones;

namespace LaserAPI.Logic
{
    public class LaserLogic
    {
        private static List<ZoneDto> _zones;
        private static int _zonesLength;
        private static bool _projectionZonesNotActive;
        private static ZoneDto _zoneWithHighestLaserPowerPwm;

        public LaserLogic(IZoneDal zoneDal)
        {
            _zones = zoneDal
                .All()
                .Result;

            _projectionZonesNotActive = !_zones.Any();
            _zonesLength = _zones.Count;
            _zoneWithHighestLaserPowerPwm = ZoneLogic.GetZoneWhereMaxLaserPowerPwmIsHighest(_zones.ToList());
        }

        public async Task SendData(LaserMessage newMessage)
        {
            int totalLaserPowerPwm = newMessage.RedLaser + newMessage.GreenLaser + newMessage.BlueLaser;
            bool skipProjectionZoneCheck = _projectionZonesNotActive || totalLaserPowerPwm == 0;

            if (skipProjectionZoneCheck)
            {
                await LaserConnectionLogic.SendMessage(newMessage);
                return;
            }

            /*ZonesHitData zoneHitData = ZoneLogic.GetZoneWherePathIsInside(_zones, _zonesLength, newMessage.X, newMessage.Y);
            bool positionIsInProjectionZone = zoneHitData != null;

            if (positionIsInProjectionZone)
            {
                LimitTotalLaserPowerIfNecessary(ref newMessage, zoneHitData.Zone.MaxLaserPowerInZonePwm);
                await LaserConnectionLogic.SendMessage(newMessage);
                return;
            }
            //Todo sort list from low to high

            List<LaserMessage> messagesToSend = GetMessagesToSend(newMessage);
            int messagesToSendLength = messagesToSend.Count;
            for (int i = 0; i < messagesToSendLength; i++)
            {
                LaserMessage message = messagesToSend[i];
                await LaserConnectionLogic.SendMessage(message);
            }            */

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
