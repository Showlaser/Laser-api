using System;

namespace LaserAPI.Models.ToFrontend.Animations
{
    public class PatternAnimation
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public TimelineSettingsViewmodel Settings { get; set; }
        public int TimeLineId { get; set; }
    }
}
