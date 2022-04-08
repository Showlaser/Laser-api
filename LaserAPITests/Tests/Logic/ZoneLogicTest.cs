using System;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class ZoneLogicTest
    {
        private readonly List<ZonesHitData> _zones;

        public ZoneLogicTest()
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
                ZoneLogic.GetZoneWherePathIsInside(_zones, _zones.Count, message.X, message.Y);
            }

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 1);
        }

        [TestMethod]
        public void GetZoneWherePathIsInsideTest()
        {
            LaserMessage message = new(0, 255, 255, 4000, -4000);
            ZonesHitData zone = ZoneLogic.GetZoneWherePathIsInside(_zones, _zones.Count, message.X, message.Y);
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

            ZonesHitData zone = ZoneLogic.GetZoneWherePathIsInside(_zones, _zones.Count, message.X, message.Y);
            Assert.IsNull(zone);
        }

        [TestMethod]
        public void GetZonesThatCrossesPathPerformanceTest()
        {
            LaserMessage message = new(0, 0, 4000, 0, -4000);
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 2; i++)
            {
                ZoneLogic.GetZonesInPathOfPosition(_zones, message);
            }

            stopwatch.Stop();
            Assert.IsTrue(stopwatch.ElapsedMilliseconds <= 1);
        }

        [TestMethod]
        public void GetZonesThatCrossesPathTest()
        {
            LaserMessage message = new(0, 0, 4000, 0, -4000);
            LaserMessage message2 = new(0, 0, 12, 4000, -4000);
            List<ZoneDto> zones = ZoneLogic.GetZonesInPathOfPosition(_zones, message)
                .Select(z => z.Zone)
                .ToList();

            List<ZoneDto> zones2 = ZoneLogic.GetZonesInPathOfPosition(_zones, message2)
                .Select(z => z.Zone)
                .ToList();

            Assert.IsTrue(zones.Any() && zones2.Any());
        }

        [TestMethod]
        public void GetZoneClosestToMessageTest()
        {
            LaserMessage message = new(255, 25, 255, -4000, 0);
            ZonesHitData zone = ZoneLogic.GetZoneClosestToMessage(message, _zones, _zones.Count);
            Assert.IsNotNull(zone);
        }

        [TestMethod]
        public void PositionMessageInZoneTest()
        {
            LaserMessage message = new(255, 25, 255, -4000, -100);
            ZonesHitData zone = _zones[1];
            LaserMessage newMessage = ZoneLogic.PositionMessageIntoZone(message, zone);
            Assert.IsTrue(newMessage.X == -400 && newMessage.Y == 100);
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesTest()
        {
            LaserConnectionLogic.PreviousLaserMessage.X = -4000;
            LaserConnectionLogic.PreviousLaserMessage.Y = 4000;
            Point crossingPoint = ZoneLogic.CalculateCrossingPointOfTwoLines(new LaserMessage(0, 0, 0, 4000, 0),
                new Point(-1000, 3000), new Point(-1000, 0), _zones[0]);

            Assert.IsTrue(crossingPoint.X == -1000 && crossingPoint.Y == 2500);
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesTest2()
        {
            LaserConnectionLogic.PreviousLaserMessage.X = -2000;
            LaserConnectionLogic.PreviousLaserMessage.Y = 3000;
            Point crossingPoint = ZoneLogic.CalculateCrossingPointOfTwoLines(new LaserMessage(0, 0, 0, 4000, 0),
                new Point(-1000, 3000), new Point(-1000, 0), _zones[0]);

            Assert.IsTrue(crossingPoint.X == -1000 && crossingPoint.Y == 2500);
        }
    }
}
