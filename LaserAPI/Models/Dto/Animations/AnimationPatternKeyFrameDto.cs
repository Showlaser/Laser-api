using LaserAPI.Models.Helper;
using System;

namespace LaserAPI.Models.Dto.Animations
{
    public class AnimationPatternKeyFrameDto
    {
        public Guid Uuid { get; set; }
        public Guid AnimationPatternUuid { get; set; }
        public int TimeMs { get; set; }
        public PropertyEdited PropertyEdited { get; set; }
        public double PropertyValue { get; set; }
    }
}
