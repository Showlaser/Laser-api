namespace LaserAPI.Models.Helper.Laser
{
    public static class LaserSafetyHelper
    {
        /// <summary>
        /// Limits the laser power if the value is greater than the max power in the zone
        /// </summary>
        /// <param name="message"></param>
        /// <param name="maxPowerPwm"></param>
        public static void LimitLaserPowerIfNecessary(ref LaserMessage message, short maxPowerPwm)
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
