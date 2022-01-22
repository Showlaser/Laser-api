using System;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class AnimationTimeline
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public TimelineSettings Settings { get; set; }
        public int TimeLineId { get; set; }
    }
}
