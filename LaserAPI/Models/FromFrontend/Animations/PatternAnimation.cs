using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class PatternAnimation
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public List<PatternsAnimationSettings> Settings { get; set; }
        public int StartTimeOffset { get; set; }
        public int TimeLineId { get; set; }
        public string Name { get; set; }
    }
}
