using LaserAPI.Models.Dto.Lasershows;
using System;
using System.Collections.Generic;

namespace LaserAPITests.MockedModels.Lasershow
{
    internal class MockedLasershow
    {
        internal LasershowDto Lasershow;
        internal List<LasershowDto> LasershowList;
        internal LasershowDto Empty;

        public MockedLasershow()
        {
            Lasershow = new LasershowDto
            {
                Uuid = Guid.Parse("cb10219c-2a18-430d-9fa5-abaff77e0430"),
                Name = "Lasershow",
                Image = "123",
                LasershowAnimations = new MockedLasershowAnimation().LasershowAnimationList
            };

            LasershowList = [
                Lasershow
            ];

            Empty = new();
        }
    }
}
