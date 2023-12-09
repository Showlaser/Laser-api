using LaserAPI.Models.Dto.Patterns;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Animations
{
    public class AnimationPatternDto
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public string Name { get; set; }
        public PatternDto Pattern { get; set; }
        public List<AnimationPatternKeyFrameDto> AnimationPatternKeyFrames { get; set; }
        public int StartTimeMs { get; set; }
        public int TimeLineId { get; set; }
    }
}
