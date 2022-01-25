using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Animations
{
    public class Animation
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public List<PatternAnimation> PatternAnimations { get; set; }
    }
}
