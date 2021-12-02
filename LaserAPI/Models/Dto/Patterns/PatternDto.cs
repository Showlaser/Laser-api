using System.Collections.Generic;

namespace LaserAPI.Models.Dto.Patterns
{
    public class PatternDto
    {
        public double Scale { get; set; } = 1;
        public List<PointDto> Points { get; set; } = new();
    }
}
