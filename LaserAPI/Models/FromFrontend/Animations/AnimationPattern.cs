using LaserAPI.Models.FromFrontend.Patterns;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class AnimationPattern
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public Guid PatternUuid { get; set; }
        public Pattern Pattern { get; set; }
        public string Name { get; set; }
        public List<AnimationPatternKeyFrame> AnimationPatternKeyFrames { get; set; }
        public int StartTimeMs { get; set; }
        public int TimeLineId { get; set; }
    }
}
