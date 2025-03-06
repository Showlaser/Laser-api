using LaserAPI.Models.Dto.Lasershows;
using LaserAPITests.MockedModels.Animation;
using System;
using System.Collections.Generic;

namespace LaserAPITests.MockedModels.Lasershow
{
    internal class MockedLasershowAnimation
    {
        internal LasershowAnimationDto LasershowAnimation;
        internal List<LasershowAnimationDto> LasershowAnimationList;
        internal LasershowAnimationDto Empty;

        public MockedLasershowAnimation()
        {
            MockedAnimation mockedAnimation = new();

            LasershowAnimation = new LasershowAnimationDto
            {
                Uuid = Guid.Parse("10865c77-f495-41bc-a244-8dcf8bd4fef7"),
                LasershowUuid = Guid.Parse("cb10219c-2a18-430d-9fa5-abaff77e0430"),
                Name = "LasershowAnimation",
                Animation = mockedAnimation.Animation,
                StartTimeMs = 0,
                TimelineId = 0,
            };

            LasershowAnimationDto lasershowAnimation2 = new()
            {
                Uuid = Guid.Parse("fa6952a5-754f-41f6-bf63-4eb2b4f5dc4c"),
                LasershowUuid = Guid.Parse("cb10219c-2a18-430d-9fa5-abaff77e0430"),
                Name = "LasershowAnimation2",
                Animation = mockedAnimation.Animation,
                StartTimeMs = 100,
                TimelineId = 0,
            };

            LasershowAnimationList = [
                LasershowAnimation,
                lasershowAnimation2
            ];

            Empty = new();
        }
    }
}
