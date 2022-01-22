using System;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class AnimationPoint
    {
        public Guid Uuid { get; set; }
        public Guid PatternAnimationUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
