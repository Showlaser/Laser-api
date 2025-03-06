using LaserAPI.Models.FromFrontend.Patterns;
using System;

namespace LaserAPI.Models.FromFrontend.Points
{
    public class PointWrapper
    {
        public int TimeMs { get; set; }
        public Guid LaserToProjectOnUuid { get; set; }
        public PatternPoint PatternPoint { get; set; }
    }
}
