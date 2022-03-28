using LaserAPI.Models.FromFrontend.Animations;
using System;

namespace LaserAPI.Models.FromFrontend.Lasershow
{
    public class LasershowAnimation
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public int StartTime { get; set; }
        public int TimelineId { get; set; }
        public Animation Animation { get; set; }
    }
}
