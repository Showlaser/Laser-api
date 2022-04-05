using LaserAPI.Models.Dto.Animations;
using System;

namespace LaserAPI.Models.Dto.Lasershow
{
    public class LasershowAnimationDto
    {
        public Guid Uuid { get; set; }
        public Guid LasershowUuid { get; set; }
        public string Name { get; set; }
        public int StartTimeOffset { get; set; }
        public int TimeLineId { get; set; }
        public AnimationDto Animation { get; set; }
    }
}
