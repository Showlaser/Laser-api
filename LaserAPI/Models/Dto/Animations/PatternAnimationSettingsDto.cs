using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Animations
{
    public class PatternAnimationSettingsDto
    {
        public Guid Uuid { get; set; }
        public Guid PatternAnimationUuid { get; set; }
        public int StartTimeMs { get; set; }
        public int DurationTimeMs { get; set; }
        public int TimeLineId { get; set; }
        public double Scale { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public List<AnimationPointDto> Points { get; set; }
    }
}
