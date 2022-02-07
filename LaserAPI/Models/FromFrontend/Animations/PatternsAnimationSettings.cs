using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class PatternsAnimationSettings
    {
        public Guid Uuid { get; set; }
        public Guid PatternAnimationUuid { get; set; }
        public double Scale { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public List<AnimationPoint> Points { get; set; }
        public int StartTime { get; set; }
    }
}
