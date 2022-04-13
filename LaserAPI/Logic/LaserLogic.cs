using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserLogic
    {
        private readonly ZoneLogic _zoneLogic;

        public LaserLogic(ZoneLogic zoneLogic)
        {
            _zoneLogic = zoneLogic;
        }

        public async Task SendData(List<LaserMessage> messages)
        {
            List<LaserMessage> messagesToSend = new();

            int messagesLength = messages.Count;
            for (int i = 0; i < messagesLength; i++)
            {
                LaserMessage message = messages[i];
                LaserMessage previousMessage = i == 0 ? message : messages[i - 1];

                ZoneDto zoneWherePathIsInside = _zoneLogic.GetZoneWherePathIsInside(message, previousMessage);
                bool positionIsInProjectionZone = zoneWherePathIsInside != null;

                if (positionIsInProjectionZone)
                {
                    LimitTotalLaserPowerIfNecessary(ref message, zoneWherePathIsInside.MaxLaserPowerInZonePwm);
                    messagesToSend.Add(message);
                }

                List<LaserMessage> zoneCrossingPoints = _zoneLogic.GetPointsOfZoneLinesHitByPath(message, previousMessage);
                if (zoneCrossingPoints.Any())
                {
                    messagesToSend.AddRange(zoneCrossingPoints);
                }
            }

            await LaserConnectionLogic.SendMessages(messagesToSend);
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
                message.RedLaser = message.RedLaser.Map(0, 255, 0, maxPowerPwmPerLaserColor);
            }
            if (message.GreenLaser > maxPowerPwmPerLaserColor)
            {
                message.GreenLaser = message.GreenLaser.Map(0, 255, 0, maxPowerPwmPerLaserColor);
            }
            if (message.BlueLaser > maxPowerPwmPerLaserColor)
            {
                message.BlueLaser = message.BlueLaser.Map(0, 255, 0, maxPowerPwmPerLaserColor);
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
                int maxPowerPerColor = maxPowerPwm / 3;
                message.RedLaser = message.RedLaser.Map(0, 255, 0, maxPowerPerColor);
                message.GreenLaser = message.GreenLaser.Map(0, 255, 0, maxPowerPerColor);
                message.BlueLaser = message.BlueLaser.Map(0, 255, 0, maxPowerPerColor); ;
            }
        }
    }
}
