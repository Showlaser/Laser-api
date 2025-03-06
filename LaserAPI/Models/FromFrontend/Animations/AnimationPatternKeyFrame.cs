using System;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class AnimationPatternKeyFrame
    {
        public Guid Uuid { get; set; }
        public Guid AnimationPatternUuid { get; set; }
        public int TimeMs { get; set; }
        public string PropertyEdited { get; set; }
        public double PropertyValue { get; set; }
    }
}
