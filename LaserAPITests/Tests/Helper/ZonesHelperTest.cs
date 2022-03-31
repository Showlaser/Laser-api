using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class ZonesHelperTest
    {
        private readonly List<ZonesHitData> _zones;

        public ZonesHelperTest()
        {
            IZoneDal zoneDal = new MockedZoneDal().ZoneDal;
            List<ZoneDto> zones = zoneDal
                .All()
                .Result;

            List<ZonesHitData> zoneHitDataCollection = zones
                .Select(zone =>
                    new ZonesHitData
                    {
                        Zone = zone,
                        ZoneAbsolutePositions = new ZoneAbsolutePositionsHelper(zone)
                    }).ToList();

            _zones = zoneHitDataCollection;
        }

        [TestMethod]
        public void GetZoneWherePathIsInsidePerformanceTest()
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
                ZonesHelper.GetZoneWherePathIsInside(_zones, _zones.Count, message.X, message.Y);
            }

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 1);
        }

        [TestMethod]
        public void GetZoneWherePathIsInsideTest()
        {
            LaserMessage message = new(0, 255, 255, 4000, -4000);
            ZonesHitData zone = ZonesHelper.GetZoneWherePathIsInside(_zones, _zones.Count, message.X, message.Y);
            Assert.IsNotNull(zone);
        }

        [TestMethod]
        public void GetZoneWherePathIsInsideNullTest()
        {
            LaserMessage message = new()
            {
                RedLaser = 0,
                GreenLaser = 255,
                BlueLaser = 255,
                X = 4000,
                Y = 4000
            };

            ZonesHitData zone = ZonesHelper.GetZoneWherePathIsInside(_zones, _zones.Count, message.X, message.Y);
            Assert.IsNull(zone);
        }

        [TestMethod]
        public void GetZonesThatCrossesPathPerformanceTest()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 2; i++)
            {
                ZonesHelper.GetZonesInPathOfPosition(_zones, 0, 4000, 0, -4000);
            }

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 1);
        }

        [TestMethod]
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
