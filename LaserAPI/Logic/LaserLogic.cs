using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using System.Collections.Generic;
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

        public async Task SendData(LaserMessage newMessage)
        {
            ZoneDto zoneWherePathIsInside = _zoneLogic.GetZoneWherePathIsInside(newMessage);
            bool positionIsInProjectionZone = zoneWherePathIsInside != null;

            if (positionIsInProjectionZone)
            {
                LimitTotalLaserPowerIfNecessary(ref newMessage, zoneWherePathIsInside.MaxLaserPowerInZonePwm);
                await LaserConnectionLogic.SendMessage(newMessage);
                return;
            }

            List<LaserMessage> messagesToSend = _zoneLogic.GetPointsOfZoneLinesHitByPath(newMessage);
            int messagesToSendLength = messagesToSend.Count;
            for (int i = 0; i < messagesToSendLength; i++)
            {
                LaserMessage message = messagesToSend[i];
                await LaserConnectionLogic.SendMessage(message);
            }
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
                double divideFactor = combinedPower.ToDouble() / maxPowerPwm.ToDouble();
                message.RedLaser = (message.RedLaser / divideFactor).ToInt();
                message.GreenLaser = (message.GreenLaser / divideFactor).ToInt();
                message.BlueLaser = (message.BlueLaser / divideFactor).ToInt();
            }
        }
    }
}
