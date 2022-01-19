using System;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class PatternAnimation
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public PatternAnimationSettings Settings { get; set; }
    }
}
