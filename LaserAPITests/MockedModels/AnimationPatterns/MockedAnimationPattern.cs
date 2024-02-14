using LaserAPI.Models.Dto.Animations;
using LaserAPITests.MockedModels.AnimationPatternKeyFrames;
using LaserAPITests.MockedModels.Pattern;
using System;
using System.Collections.Generic;

namespace LaserAPITests.MockedModels.AnimationPatterns
{
    internal class MockedAnimationPattern
    {
        internal List<AnimationPatternDto> AnimationPattern;

        public MockedAnimationPattern()
        {
            AnimationPattern = [new()
            {
                Uuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                AnimationUuid = Guid.Parse("824e91dd-f0ac-46a9-bd51-4723f45af2b0"),
                Name = "Pattern",
                Pattern = new MockedPattern().Pattern,
                AnimationPatternKeyFrames = new MockedAnimationPatternKeyFrames().AnimationPatternKeyFrames,
            }];
        }
    }
}
