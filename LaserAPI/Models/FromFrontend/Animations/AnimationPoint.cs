using System;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class AnimationPoint
    {
        public Guid Uuid { get; set; }
        public Guid PatternAnimationSettingsUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int RedLaserPowerPwm { get; set; }
        public int GreenLaserPowerPwm { get; set; }
        public int BlueLaserPowerPwm { get; set; }
        public int Order { get; set; }
    }
}
