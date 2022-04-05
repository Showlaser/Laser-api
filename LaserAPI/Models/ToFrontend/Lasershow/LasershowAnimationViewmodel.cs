using LaserAPI.Models.ToFrontend.Animations;
using System;

namespace LaserAPI.Models.ToFrontend.Lasershow
{
    public class LasershowAnimationViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid LasershowUuid { get; set; }
        public string Name { get; set; }
        public int StartTimeOffset { get; set; }
        public int TimeLineId { get; set; }
        public AnimationViewModel Animation { get; set; }
    }
}
