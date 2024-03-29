﻿using LaserAPI.Models.Dto.Zones;
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
                Points = new List<ZonesPositionDto>
                {
                    new()
                    {
                        ZoneUuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                        Uuid = Guid.Parse("9df6bd3a-db73-475f-beaa-09f3c5f1ee9f"),
                        X = -1000,
                        Y = 4000,
                        Order = 0
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                        Uuid = Guid.Parse("18fcaa8c-7a3c-4505-961a-d9aa403eb225"),
                        X = 1000,
                        Y = 4000,
                        Order = 1
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                        Uuid = Guid.Parse("5e87ee08-5212-43a4-aa30-1231330669f2"),
                        X = 1000,
                        Y = 0,
                        Order = 2
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"),
                        Uuid = Guid.Parse("432acb83-4638-493f-b469-f42dae78eb9b"),
                        X = -1000,
                        Y = 0,
                        Order = 3
                    },
                }
            };
            ZoneDto zone2 = new()
            {
                MaxLaserPowerInZonePwm = 40,
                Uuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                Points = new List<ZonesPositionDto>
                {
                    new()
                    {
                        ZoneUuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                        Uuid = Guid.Parse("b75f2e15-b005-4626-b71d-fc353959cd7f"),
                        X = -4000,
                        Y = -2000,
                        Order = 0
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                        Uuid = Guid.Parse("1829d10c-7c1d-42d7-ace2-7f78cdfd2448"),
                        X = 4000,
                        Y = -2000,
                        Order = 1
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                        Uuid = Guid.Parse("16fb7dfb-7a12-4d7e-a5ca-938389c99b08"),
                        X = 4000,
                        Y = -4000,
                        Order = 2
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("c9ba9637-96c6-455d-be72-849ce86438a8"),
                        Uuid = Guid.Parse("2cb0839b-a7eb-4a4c-8237-cda10660bce8"),
                        X = -4000,
                        Y = -4000,
                        Order = 3
                    },
                },
            };
            ZoneDto zone3 = new()
            {
                MaxLaserPowerInZonePwm = 40,
                Uuid = Guid.Parse("37a8eb34-7b11-42f9-ba85-cdc778d48a3c"),
                Points = new List<ZonesPositionDto>
                {
                    new()
                    {
                        ZoneUuid = Guid.Parse("37a8eb34-7b11-42f9-ba85-cdc778d48a3c"),
                        Uuid = Guid.Parse("e658779d-d90e-4968-8e96-1bd3eea93fc1"),
                        X = -2000,
                        Y = 0,
                        Order = 0
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("37a8eb34-7b11-42f9-ba85-cdc778d48a3c"),
                        Uuid = Guid.Parse("c6ebe4be-1c24-40d4-bd6c-964367b88c27"),
                        X = 2500,
                        Y = 0,
                        Order = 1
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("37a8eb34-7b11-42f9-ba85-cdc778d48a3c"),
                        Uuid = Guid.Parse("7e4095a8-0f2b-4e6b-a35d-b598eeec881c"),
                        X = 2000,
                        Y = -2000,
                        Order = 2
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("37a8eb34-7b11-42f9-ba85-cdc778d48a3c"),
                        Uuid = Guid.Parse("c7e18686-2b12-4c66-953e-861e98616b40"),
                        X = -2500,
                        Y = -2000,
                        Order = 3
                    },
                }
            };
            ZoneDto zone4 = new()
            {
                MaxLaserPowerInZonePwm = 40,
                Uuid = Guid.Parse("e4c8cea7-9722-4f1f-bbd1-9c64f7da7e2e"),
                Points = new List<ZonesPositionDto>
                {
                    new()
                    {
                        ZoneUuid = Guid.Parse("e4c8cea7-9722-4f1f-bbd1-9c64f7da7e2e"),
                        Uuid = Guid.Parse("5c7adca6-cff7-41b8-aabb-8d7c97a86818"),
                        X = -2000,
                        Y = 1000,
                        Order = 0
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("e4c8cea7-9722-4f1f-bbd1-9c64f7da7e2e"),
                        Uuid = Guid.Parse("f5966b78-cba2-417b-b622-ad843ddffe2d"),
                        X = 0,
                        Y = 0,
                        Order = 1
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("e4c8cea7-9722-4f1f-bbd1-9c64f7da7e2e"),
                        Uuid = Guid.Parse("9084bf4f-9b32-47dd-bc68-33ac644de270"),
                        X = 2000,
                        Y = 1000,
                        Order = 2
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("e4c8cea7-9722-4f1f-bbd1-9c64f7da7e2e"),
                        Uuid = Guid.Parse("ee695f9e-418f-44e5-a0a6-735060953ca7"),
                        X = 2000,
                        Y = -2000,
                        Order = 3
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("e4c8cea7-9722-4f1f-bbd1-9c64f7da7e2e"),
                        Uuid = Guid.Parse("5fbf7c5c-2cf8-4500-8c53-541c8a67e776"),
                        X = 0,
                        Y = -1000,
                        Order = 4
                    },
                    new()
                    {
                        ZoneUuid = Guid.Parse("e4c8cea7-9722-4f1f-bbd1-9c64f7da7e2e"),
                        Uuid = Guid.Parse("91043eda-ecee-42c7-af32-260222d2cdde"),
                        X = -2000,
                        Y = -1000,
                        Order = 5
                    },
                }
            };

            Zones.Add(zone1);
            Zones.Add(zone2);
            Zones.Add(zone3);
            Zones.Add(zone4);
        }
    }
}
