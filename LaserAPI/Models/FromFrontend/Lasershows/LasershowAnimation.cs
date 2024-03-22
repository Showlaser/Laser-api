using LaserAPI.Models.FromFrontend.Animations;
using System;

namespace LaserAPI.Models.FromFrontend.Lasershows
{
    public class LasershowAnimation
    {
        public Guid Uuid { get; set; }
        public Guid LasershowUuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public string Name { get; set; }
        public Animation Animation { get; set; }
        public int StartTimeMs { get; set; }
        public int TimeLineId { get; set; }
    }
}
