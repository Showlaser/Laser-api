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
            ZoneDto zone2 = new()
            {
                MaxLaserPowerInZonePwm = 40,
                Uuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                Positions = new List<ZonesPositionDto>
                {
                    new()
                    {
                        ZoneUuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                        Uuid = Guid.Parse("b75f2e15-b005-4626-b71d-fc353959cd7f"),
                        X = -400,
                        Y = 100
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                        Uuid = Guid.Parse("1829d10c-7c1d-42d7-ace2-7f78cdfd2448"),
                        X = -400,
                        Y = 100
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                        Uuid = Guid.Parse("16fb7dfb-7a12-4d7e-a5ca-938389c99b08"),
                        X = 400,
                        Y = 4000
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                        Uuid = Guid.Parse("2cb0839b-a7eb-4a4c-8237-cda10660bce8"),
                        X = 400,
                        Y = 4000
                    },
                }
            };

            Zones.Add(zone1);
            Zones.Add(zone2);
        }
    }
}
