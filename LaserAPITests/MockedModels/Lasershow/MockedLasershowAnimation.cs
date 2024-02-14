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
                TimeLineId = 0,
            };

            LasershowAnimationList = [
                LasershowAnimation
            ];

            Empty = new();
        }
    }
}
