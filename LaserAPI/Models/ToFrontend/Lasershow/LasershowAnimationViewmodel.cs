using LaserAPI.Models.ToFrontend.Animations;
using System;

namespace LaserAPI.Models.ToFrontend.Lasershow
{
    public class LasershowAnimationViewmodel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public int StartTime { get; set; }
        public int TimelineId { get; set; }
        public AnimationViewModel Animation { get; set; }
    }
}
