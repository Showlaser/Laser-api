namespace LaserAPI.Models.Helper.Laser
{
    public static class LaserSafetyHelper
    {
        /// <summary>
        /// Limits the laser power if the value is greater than the max power in the zone
        /// </summary>
        /// <param name="message">The message to modify</param>
        /// <param name="maxPowerPwm">The max power allowed in PWM value</param>
        public static void LimitLaserPowerIfNecessary(ref LaserMessage message, int maxPowerPwm)
        {
            int totalLaserPowerPwm = message.RedLaser + message.GreenLaser + message.BlueLaser;
            int maxPowerPerLaserColorPwm = NumberHelper.Map(totalLaserPowerPwm, 0, 511, 0, totalLaserPowerPwm / 3);
            if (totalLaserPowerPwm <= maxPowerPwm)
            {
                return;
            }

            if (message.RedLaser > maxPowerPerLaserColorPwm)
            {
                message.RedLaser = NumberHelper.Map(message.RedLaser, 0, 511, 0, maxPowerPerLaserColorPwm);
            }

            if (message.GreenLaser > maxPowerPerLaserColorPwm)
            {
                message.GreenLaser = NumberHelper.Map(message.GreenLaser, 0, 511, 0, maxPowerPerLaserColorPwm);
            }

            if (message.BlueLaser > maxPowerPerLaserColorPwm)
            {
                message.BlueLaser = NumberHelper.Map(message.BlueLaser, 0, 511, 0, maxPowerPerLaserColorPwm);
            }
        }
    }
}
