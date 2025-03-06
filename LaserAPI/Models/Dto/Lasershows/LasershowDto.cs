using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Lasershows
{
    public class LasershowDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<LasershowAnimationDto> LasershowAnimations { get; set; }
    }
}
