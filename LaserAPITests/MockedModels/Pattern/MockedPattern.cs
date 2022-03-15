using LaserAPI.Models.Dto.Patterns;
using System;
using System.Collections.Generic;

namespace LaserAPITests.MockedModels.Pattern
{
    internal class MockedPattern
    {
        internal PatternDto Pattern;
        internal List<PatternDto> PatternList;
        internal PatternDto Empty;
        internal PatternDto ScaleToHigh;
        internal PatternDto ScaleToLow;
        internal PatternDto EmptyPoints;

        public MockedPattern()
        {
            PatternDto? pattern = new PatternDto
            {
                Scale = 1.0,
                Uuid = Guid.Parse("ca1529e8-ac96-4cfb-93fb-1bb075969766"),
                Points = new List<PointDto>
                {
                    new()
                    {
                        Uuid = Guid.Parse("81fb5f2d-5b16-4bfa-a1cf-f595ad1ce20a"),
                        PatternUuid = Guid.Parse("ca1529e8-ac96-4cfb-93fb-1bb075969766"),
                        X = -4000,
                        Y = 4000,
                    },
                    new()
                    {
                        Uuid = Guid.Parse("23b6e906-cf5d-4cdc-8667-1d938f68771b"),
                        PatternUuid = Guid.Parse("ca1529e8-ac96-4cfb-93fb-1bb075969766"),
                        X = -4000,
                        Y = 4000,
                    }
                }
            };

            PatternDto? pattern2 = new PatternDto
            {
                Scale = 0.5,
                Points = new List<PointDto>
                {
                    new()
                    {
                        Uuid = Guid.Parse("1fa48d00-47cb-4987-a9c7-a8c8d1af236d"),
                        X = -4000,
                        Y = 4000,
                    },
                    new()
                    {
                        Uuid = Guid.Parse("9f4c57c9-c818-49e7-aff9-25954da4ab91"),
                        X = -4000,
                        Y = 4000,
                    }
                }
            };

            Pattern = pattern;
            PatternList = new List<PatternDto>
            {
                pattern,
                pattern2
            };

            Empty = new PatternDto
            {
                Scale = double.NaN
            };
            ScaleToHigh = new PatternDto
            {
                Scale = 1.1
            };
            ScaleToLow = new PatternDto
            {
                Scale = 0
            };
            EmptyPoints = new PatternDto
            {
                Scale = 0.5,
                Points = new List<PointDto>()
            };
        }
    }
}
