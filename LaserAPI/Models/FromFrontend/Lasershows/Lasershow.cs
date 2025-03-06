using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Lasershows
{
    public class Lasershow
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<LasershowAnimation> LasershowAnimations { get; set; }
    }
}
