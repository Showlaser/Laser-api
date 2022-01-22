using System;

namespace LaserAPI.Models.ToFrontend.Animations
{
    public class AnimationPointViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid PatternAnimationUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}