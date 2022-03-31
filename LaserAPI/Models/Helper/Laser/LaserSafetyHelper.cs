using System;

namespace LaserAPI.Models.Helper.Laser
{
    public static class LaserSafetyHelper
    {
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
        public static void LimitTotalLaserPowerNecessary(ref LaserMessage message, int maxPowerPwm)
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
