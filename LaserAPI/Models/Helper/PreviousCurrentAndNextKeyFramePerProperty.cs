using LaserAPI.Models.Dto.Animations;
using System.Collections.Generic;

namespace LaserAPI.Models.Helper
{
    public class PreviousCurrentAndNextKeyFramePerProperty
    {
        public List<AnimationPatternKeyFrameDto> Previous { get; set; }
        public List<AnimationPatternKeyFrameDto> Current { get; set; }
        public List<AnimationPatternKeyFrameDto> Next { get; set; }
    }
}
