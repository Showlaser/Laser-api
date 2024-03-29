﻿using System;

namespace LaserAPI.Models.Helper
{
    public class AlgorithmSettings
    {
        public Range FrequencyRange { get; set; } = new(2, 5);

        public double Threshold { get; set; } = 0.02;
    }
}
