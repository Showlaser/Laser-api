﻿using System;

namespace LaserAPI.Models.FromFrontend.Patterns
{
    public class PatternPoint
    {
        public Guid Uuid { get; set; }
        public Guid PatternUuid { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}