using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;

namespace LaserAPITests.MockedModels.AnimationPatternKeyFrames
{
    internal class MockedAnimationPatternKeyFrames
    {
        public List<AnimationPatternKeyFrameDto> AnimationPatternKeyFrames;

        public MockedAnimationPatternKeyFrames()
        {
            AnimationPatternKeyFrames =
            [
                new()
                {
                    Uuid = Guid.Parse("bfc2275e-5a53-4c22-8c2f-91b0f68e2d55"),
                    AnimationPatternUuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                    TimeMs = 0,
                    PropertyEdited = PropertyEdited.Scale,
                    PropertyValue = 0
                },
                new()
                {
                    Uuid = Guid.Parse("bc6735ed-6650-47ef-9ef6-3edfc7d8b8f0"),
                    AnimationPatternUuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                    TimeMs = 0,
                    PropertyEdited = PropertyEdited.XOffset,
                    PropertyValue = 0
                },
                new()
                {
                    Uuid = Guid.Parse("2991ad82-3806-43f1-82bd-2a6ca23466d4"),
                    AnimationPatternUuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                    TimeMs = 0,
                    PropertyEdited = PropertyEdited.YOffset,
                    PropertyValue = 0
                },
                new()
                {
                    Uuid = Guid.Parse("70e96d41-26a3-4a1f-9449-f208bc0ff712"),
                    AnimationPatternUuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                    TimeMs = 0,
                    PropertyEdited = PropertyEdited.Rotation,
                    PropertyValue = 0
                },
                // Starting new timems
                new()
                {
                    Uuid = Guid.Parse("f3563d6c-6f13-4281-adc0-4a563794fc56"),
                    AnimationPatternUuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                    TimeMs = 50,
                    PropertyEdited = PropertyEdited.Scale,
                    PropertyValue = 0
                },
                new()
                {
                    Uuid = Guid.Parse("1f4cfbff-2255-4746-bf17-9783528dd807"),
                    AnimationPatternUuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                    TimeMs = 50,
                    PropertyEdited = PropertyEdited.XOffset,
                    PropertyValue = 0
                },
                new()
                {
                    Uuid = Guid.Parse("6054dff0-8b3b-45f2-a818-cbee267b988f"),
                    AnimationPatternUuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                    TimeMs = 50,
                    PropertyEdited = PropertyEdited.YOffset,
                    PropertyValue = 0
                },
                new()
                {
                    Uuid = Guid.Parse("9cd32d90-409a-4542-aebb-294477da62b9"),
                    AnimationPatternUuid = Guid.Parse("a9de1d0b-a623-433e-8a17-57555d3c5e4e"),
                    TimeMs = 50,
                    PropertyEdited = PropertyEdited.Rotation,
                    PropertyValue = 0
                }
            ];
        }
    }
}
