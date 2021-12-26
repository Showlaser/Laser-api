using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Patterns
{
    public class PatternDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public double Scale { get; set; } = 1;
        public List<PointDto> Points { get; set; } = new();
    }
}
