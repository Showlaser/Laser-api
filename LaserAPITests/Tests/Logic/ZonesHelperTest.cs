using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.MockedModels.Zones;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LaserAPITests.Tests.Logic
{
    [TestFixture]
    public class ZonesHelperTest
    {
        private readonly List<ZoneDto> _zones;

        public ZonesHelperTest()
        {
            MockedZones mockedZones = new();
            var mockedZoneDal = new Mock<IZoneDal>();
            mockedZoneDal.Setup(d => d.All()).ReturnsAsync(mockedZones.Zones);

            _zones = mockedZoneDal.Object
                .All()
                .Result;

            //TODO move this to different class
        }

        [Test]
        public void GetZoneWherePositionIsInPerformanceTest()
        {
            var message = new LaserMessage
            {
                RedLaser = 0,
                GreenLaser = 255,
                BlueLaser = 255,
                X = 2000,
                Y = -2000
            };

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 2; i++)
            {
                ZonesHelper.GetZoneWherePositionIsIn(_zones, message.X, message.Y);
            }

            stopwatch.Stop();
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, 1);
        }

        [Test]
        public void GetZoneWherePositionIsInTest()
        {
            var message = new LaserMessage
            {
                RedLaser = 0,
                GreenLaser = 255,
                BlueLaser = 255,
                X = 2000,
                Y = -2000
            };

            ZoneDto zone = ZonesHelper.GetZoneWherePositionIsIn(_zones, message.X, message.Y);
            Assert.NotNull(zone);
        }

        [Test]
        public void GetZoneWherePositionIsNotInTest()
        {
            var message = new LaserMessage
            {
                RedLaser = 0,
                GreenLaser = 255,
                BlueLaser = 255,
                X = 2000,
                Y = 4000
            };

            ZoneDto zone = ZonesHelper.GetZoneWherePositionIsIn(_zones, message.X, message.Y);
            Assert.Null(zone);
        }

        [Test]
        public void GetZonesThatCrossesPathPerformanceTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 2; i++)
            {
                ZonesHelper.GetZonesInPathOfPosition(_zones, 0, 4000, 0, -4000);
            }

            stopwatch.Stop();
            Assert.LessOrEqual(stopwatch.ElapsedMilliseconds, 1);
        }

        [Test]
        public void GetZonesThatCrossesPathTest()
        {
            List<ZoneDto> zones = ZonesHelper.GetZonesInPathOfPosition(_zones, 0, 4000, 0, -4000)
                .Select(z => z.Zone)
                .ToList();

            List<ZoneDto> zones2 = ZonesHelper.GetZonesInPathOfPosition(_zones, -4000, 0, 4000, 0)
                .Select(z => z.Zone)
                .ToList();

            Assert.IsTrue(zones.Any() && zones2.Any());
        }
    }
}
