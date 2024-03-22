using LaserAPI.Models.Dto.Animations;
using System;

namespace LaserAPI.Models.Dto.Lasershows
{
    public class LasershowAnimationDto
    {
        public Guid Uuid { get; set; }
        public Guid LasershowUuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public string Name { get; set; }
        public AnimationDto Animation { get; set; }
        public int StartTimeMs { get; set; }
        public int TimeLineId { get; set; }
    }
}
