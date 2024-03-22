using LaserAPI.Models.ToFrontend.Pattern;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Animations
{
    public class AnimationPatternViewModel
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public Guid PatternUuid { get; set; }
        public PatternViewmodel Pattern { get; set; }
        public string Name { get; set; }
        public List<AnimationPatternKeyFrameViewModel> AnimationPatternKeyFrames { get; set; }
        public int StartTimeMs { get; set; }
        public int TimeLineId { get; set; }
    }
}
