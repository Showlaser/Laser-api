using LaserAPI.Models.FromFrontend.Animations;
using System;

namespace LaserAPI.Models.FromFrontend.Lasershow
{
    public class LasershowAnimation
    {
        public Guid Uuid { get; set; }
        public Guid LasershowUuid { get; set; }
        public string Name { get; set; }
        public int StartTimeOffset { get; set; }
        public int TimeLineId { get; set; }
        public Animation Animation { get; set; }
    }
}
