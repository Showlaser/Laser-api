using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Lasershow
{
    public class LasershowDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public List<LasershowAnimationDto> Animations { get; set; }
    }
}
