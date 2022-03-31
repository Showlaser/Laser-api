using LaserAPI.Models.Dto.Zones;
using System;
using System.Collections.Generic;

namespace LaserAPITests.MockedModels.Zones
{
    internal class MockedZones
    {
        internal List<ZoneDto> Zones = new();

        public MockedZones()
        {
            ZoneDto zone1 = new()
            {
                MaxLaserPowerInZonePwm = 50,
                Uuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                Positions = new List<ZonesPositionDto>
                {
                    new()
                    {
                        ZoneUuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                        Uuid = Guid.Parse("9df6bd3a-db73-475f-beaa-09f3c5f1ee9f"),
                        X = -4000,
                        Y = -4000
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                        Uuid = Guid.Parse("18fcaa8c-7a3c-4505-961a-d9aa403eb225"),
                        X = -4000,
                        Y = 0
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                        Uuid = Guid.Parse("5e87ee08-5212-43a4-aa30-1231330669f2"),
                        X = 4000,
                        Y = 0
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                        Uuid = Guid.Parse("432acb83-4638-493f-b469-f42dae78eb9b"),
                        X = 4000,
                        Y = -4000
                    },
                }
            };

            Zones.Add(zone1);
        }
    }
}
