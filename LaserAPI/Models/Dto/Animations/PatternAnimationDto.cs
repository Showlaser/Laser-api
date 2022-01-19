﻿using System;

namespace LaserAPI.Models.Dto.Animations
{
    public class PatternAnimationDto
    {
        public Guid Uuid { get; set; }
        public Guid AnimationUuid { get; set; }
        public PatternAnimationSettingsDto Settings { get; set; }
    }
}
