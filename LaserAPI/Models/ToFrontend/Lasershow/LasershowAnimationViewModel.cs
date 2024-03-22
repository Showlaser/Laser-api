using LaserAPI.Models.ToFrontend.Animations;
using System;

namespace LaserAPI.Models.ToFrontend.Lasershow
{
    public class LasershowAnimationViewModel
    {
        public Guid Uuid { get; set; }
        public Guid LasershowUuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public string Name { get; set; }
        public AnimationViewModel Animation { get; set; }
        public int StartTimeMs { get; set; }
        public int TimeLineId { get; set; }
    }
}
