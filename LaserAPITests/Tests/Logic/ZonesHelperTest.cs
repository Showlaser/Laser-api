using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            IZoneDal zoneDal = new MockedZoneDal().ZoneDal;
            List<ZoneDto> zones = zoneDal
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
        public void GetZoneWherePositionIsInNullTest()
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
            Assert.IsNull(zone);
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
