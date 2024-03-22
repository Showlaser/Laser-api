using System;

namespace LaserAPI.Models.ToFrontend.Animations
{
    public class AnimationPatternKeyFrameViewModel
    {
        public Guid Uuid { get; set; }
        public Guid AnimationPatternUuid { get; set; }
        public int TimeMs { get; set; }
        public string PropertyEdited { get; set; }
        public double PropertyValue { get; set; }
    }
}
