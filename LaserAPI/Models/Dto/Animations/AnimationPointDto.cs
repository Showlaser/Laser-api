using System;

namespace LaserAPI.Models.Dto.Animations
{
    public class AnimationPointDto
    {
        public Guid Uuid { get; set; }
        public Guid TimelineSettingsUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int RedLaserPowerPwm { get; set; }
        public int GreenLaserPowerPwm { get; set; }
        public int BlueLaserPowerPwm { get; set; }
    }
}