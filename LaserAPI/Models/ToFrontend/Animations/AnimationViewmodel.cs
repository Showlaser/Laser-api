﻿using System;
using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Animations
{
    public class AnimationViewModel
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<AnimationPatternViewModel> AnimationPatterns { get; set; }
    }
}
