using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Animations
{
    public class AnimationDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<AnimationPatternDto> AnimationPatterns { get; set; }
    }
}
