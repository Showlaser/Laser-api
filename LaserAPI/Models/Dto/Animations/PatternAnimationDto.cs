using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Animations
{
    public class PatternAnimationDto
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public List<PatternAnimationSettingsDto> AnimationSettings { get; set; }
        public int TimeLineId { get; set; }
        public string Name { get; set; }
    }
}
