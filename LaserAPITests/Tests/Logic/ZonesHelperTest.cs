using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.MockedModels.Zones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class ZonesHelperTest
    {
        private readonly List<ZonesHitDataHelper> _zones;

        public ZonesHelperTest()
        {
            MockedZones mockedZones = new();
            Mock<IZoneDal> mockedZoneDal = new();
            mockedZoneDal.Setup(d => d.All()).ReturnsAsync(mockedZones.Zones);
            List<ZoneDto> zones = mockedZoneDal.Object
                .All()
                .Result;

            List<ZonesHitDataHelper> zoneHitDataCollection = zones
                .Select(zone =>
                    new ZonesHitDataHelper
                    {
                        Zone = zone,
                        ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(zone)
                    }).ToList();

            _zones = zoneHitDataCollection;

            //TODO move this to different class
        }

        [TestMethod]
        public void GetZoneWherePositionIsInPerformanceTest()
        {
            LaserMessage message = new()
            {
                RedLaser = 0,
                GreenLaser = 255,
                BlueLaser = 255,
                X = 4000,
                Y = -4000
            };

            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 2; i++)
            {
                ZonesHelper.GetZoneWherePositionIsIn(_zones, _zones.Count, message.X, message.Y);
            }

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 1);
        }

        [TestMethod]
        public void GetZoneWherePositionIsInTest()
        {
            LaserMessage message = new(0, 255, 255, 4000, -4000);

            ZonesHitDataHelper zone = ZonesHelper.GetZoneWherePositionIsIn(_zones, _zones.Count, message.X, message.Y);
            Assert.IsNotNull(zone);
        }

        [TestMethod]
        public void GetZoneWherePositionIsNotInTest()
        {
            LaserMessage message = new()
            {
                RedLaser = 0,
                GreenLaser = 255,
                BlueLaser = 255,
                X = 4000,
                Y = 4000
            };

            ZonesHitDataHelper zone = ZonesHelper.GetZoneWherePositionIsIn(_zones, _zones.Count, message.X, message.Y);
            Assert.IsNotNull(zone);
        }

        [TestMethod]
        public void GetZonesThatCrossesPathPerformanceTest()
        {
            int length = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 2; i++)
            {
                ZonesHelper.GetZonesInPathOfPosition(_zones, 0, 4000, 0, -4000, ref length);
            }

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 1);
        }

        [TestMethod]
        public void GetZonesThatCrossesPathTest()
        {
            int length = 0;

            List<ZoneDto> zones = ZonesHelper.GetZonesInPathOfPosition(_zones, 0, 4000, 0, -4000, ref length)
                .Select(z => z.Zone)
                .ToList();

            List<ZoneDto> zones2 = ZonesHelper.GetZonesInPathOfPosition(_zones, -4000, 0, 4000, 0, ref length)
                .Select(z => z.Zone)
                .ToList();

            Assert.IsTrue(zones.Any() && zones2.Any());
        }
    }
}
