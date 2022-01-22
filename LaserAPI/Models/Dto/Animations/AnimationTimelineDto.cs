using System;

namespace LaserAPI.Models.Dto.Animations
{
    public class AnimationTimelineDto
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public TimelineSettingsDto Settings { get; set; }
        public int TimeLineId { get; set; }
    }
}
