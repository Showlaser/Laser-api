using System;
using System.Collections.Generic;

namespace LaserAPI.Models.FromFrontend.Lasershow
{
    public class Lasershow
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public List<LasershowAnimation> Animations { get; set; }
    }
}
