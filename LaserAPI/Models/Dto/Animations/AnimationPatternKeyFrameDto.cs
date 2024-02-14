using System;

namespace LaserAPI.Models.Dto.Animations
{
    public enum PropertyEdited
    {
        Scale,
        XOffset,
        YOffset,
        Rotation
    }

    public class AnimationPatternKeyFrameDto
    {
        public Guid Uuid { get; set; }
        public Guid AnimationPatternUuid { get; set; }
        public int TimeMs { get; set; }
        public PropertyEdited PropertyEdited { get; set; }
        public int PropertyValue { get; set; }
    }
}
