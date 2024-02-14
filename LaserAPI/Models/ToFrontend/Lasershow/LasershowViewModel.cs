using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Lasershow
{
    public class LasershowViewModel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<LasershowAnimationViewModel> LasershowAnimations { get; set; }
    }
}
