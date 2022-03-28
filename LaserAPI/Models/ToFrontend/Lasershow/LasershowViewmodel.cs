using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Lasershow
{
    public class LasershowViewmodel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public List<LasershowAnimationViewmodel> Animations { get; set; }
    }
}
