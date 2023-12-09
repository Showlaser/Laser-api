using System;

namespace LaserAPI.Models.Dto.Animations
{
    public class AnimationPatternKeyFrameDto
    {
        public Guid Uuid { get; set; }
        public Guid AnimationPatternUuid { get; set; }
        public int TimeMs { get; set; }
        public string PropertyEdited { get; set; }
        public int PropertyValue { get; set; }
    }
}
