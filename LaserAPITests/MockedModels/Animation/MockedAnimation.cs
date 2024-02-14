using LaserAPI.Models.Dto.Animations;
using LaserAPITests.MockedModels.AnimationPatterns;
using System;
using System.Collections.Generic;

namespace LaserAPITests.MockedModels.Animation
{
    internal class MockedAnimation
    {
        internal AnimationDto Animation;
        internal List<AnimationDto> AnimationList;
        internal AnimationDto Empty;

        public MockedAnimation()
        {
            Animation = new AnimationDto
            {
                Uuid = Guid.Parse("824e91dd-f0ac-46a9-bd51-4723f45af2b0"),
                Name = "Animation",
                Image = "123",
                AnimationPatterns = new MockedAnimationPattern().AnimationPattern,
            };

            AnimationList = [
                Animation
            ];

            Empty = new();
        }
    }
}
