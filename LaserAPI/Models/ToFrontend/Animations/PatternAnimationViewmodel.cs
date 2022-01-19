using System;

namespace LaserAPI.Models.ToFrontend.Animations
{
    public class PatternAnimationViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public PatternAnimationSettingsViewmodel Settings { get; set; }
    }
}
