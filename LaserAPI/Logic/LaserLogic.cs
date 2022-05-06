using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using System;
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

        public async Task SendData(IReadOnlyList<LaserMessage> messages, long duration)
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

                LaserMessage[] zoneCrossingPoints = _zoneLogic.GetPointsOfZoneLinesHitByPath(message, previousMessage);
                if (zoneCrossingPoints.Length > 0)
                {
                    messagesToSend.AddRange(zoneCrossingPoints);
                }
            }

            await LaserConnectionLogic.SendMessages(new LaserCommando(duration, messagesToSend.ToArray()));
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
                double redLaserPowerPercentage = message.RedLaser.ToDouble() / combinedPower.ToDouble();
                double greenLaserPowerPercentage = message.GreenLaser.ToDouble() / combinedPower.ToDouble();
                double blueLaserPowerPercentage = message.BlueLaser.ToDouble() / combinedPower.ToDouble();

                message.RedLaser = Math.Floor(maxPowerPwm * redLaserPowerPercentage).ToInt();
                message.GreenLaser = Math.Floor(maxPowerPwm * greenLaserPowerPercentage).ToInt();
                message.BlueLaser = Math.Floor(maxPowerPwm * blueLaserPowerPercentage).ToInt();
            }
        }
    }
}
