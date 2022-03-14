using LaserAPI.Models.Dto.Animations;
using System;

namespace LaserAPI.Models.Dto.Lasershow
{
    public class LasershowAnimationDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public int StartTime { get; set; }
        public int TimelineId { get; set; }
        public AnimationDto Animation { get; set; }
    }
}
