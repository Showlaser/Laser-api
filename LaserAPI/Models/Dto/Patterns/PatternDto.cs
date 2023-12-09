using System;
using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Patterns
{
    public class PatternDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<PointDto> Points { get; set; } = new();
        public double Scale { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int Rotation { get; set; }
    }
}
