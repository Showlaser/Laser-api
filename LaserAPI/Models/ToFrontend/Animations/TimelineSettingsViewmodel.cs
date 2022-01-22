using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Animations
{
    public class TimelineSettingsViewmodel
    {
        public Guid Uuid { get; set; }
        public Guid AnimationTimelineUuid { get; set; }
        public double Scale { get; set; }
        public int CenterX { get; set; }
        public int CenterY { get; set; }
        public List<AnimationPointViewmodel> Points { get; set; }
        public int StartTime { get; set; }
    }
}
