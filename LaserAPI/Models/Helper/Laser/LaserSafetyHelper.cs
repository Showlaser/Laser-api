namespace LaserAPI.Models.Helper.Laser
{
    public static class LaserSafetyHelper
    {
        /// <summary>
        /// Limits the laser power if the value is greater than the max power in the zone
        /// </summary>
        /// <param name="message">The message to modify</param>
        /// <param name="maxPowerPwmPerLaser">The max power allowed in PWM value per laser</param>
        public static void LimitLaserPowerIfNecessary(ref LaserMessage message, int maxPowerPwmPerLaser)
        {
            /*message.RedLaser = NumberHelper.Map(message.RedLaser, 0, 255, 0, maxPowerPwmPerLaser);
            message.GreenLaser = NumberHelper.Map(message.GreenLaser, 0, 255, 0, maxPowerPwmPerLaser);
            message.BlueLaser = NumberHelper.Map(message.BlueLaser, 0, 255, 0, maxPowerPwmPerLaser);*/
            message.RedLaser = 7;//new Random().Next(0, 7);
            message.GreenLaser = 0;// new Random().Next(0, 7);
            message.BlueLaser = 0;// new Random().Next(0, 7);
        }
    }
}
