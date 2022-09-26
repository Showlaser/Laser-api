using LaserAPI.Models.Dto.Patterns;
using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Animations
{
    public class AnimationDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public PointDto Point { get; set; }
        public List<PatternAnimationDto> PatternAnimations { get; set; }
    }
}
