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
            if (message.RedLaser > maxPowerPwm)
            {
                message.RedLaser = maxPowerPwm;
            }

            if (message.GreenLaser > maxPowerPwm)
            {
                message.GreenLaser = maxPowerPwm;
            }

            if (message.BlueLaser > maxPowerPwm)
            {
                message.BlueLaser = maxPowerPwm;
            }
        }
    }
}
