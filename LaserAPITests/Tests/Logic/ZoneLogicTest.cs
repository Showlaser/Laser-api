using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;
using LaserAPITests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class ZoneLogicTest
    {
        private readonly List<SafetyZoneDto> _zones;
        private readonly ZoneLogic _zoneLogic;

        public ZoneLogicTest()
        {
            IZoneDal zoneDal = new MockedZoneDal().ZoneDal;
            _zones = zoneDal
                .All()
                .Result;
            _zoneLogic = new MockedZoneLogic().ZoneLogic;
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesTest()
        {
            LaserMessage previousMessage = new(0, 0, 0, -4000, 4000);
            Point crossingPoint = ProjectionZonesLogic.CalculateCrossingPointOfZoneLineAndLaserPath(new LaserMessage(0, 0, 0, 4000, 0),
                previousMessage, new Point(-1000, 3000), new Point(-1000, 0));

            Assert.IsTrue(crossingPoint.X == -1000 && crossingPoint.Y == 2500);
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesTest3()
        {
            LaserMessage previousMessage = new(0, 0, 0, -4000, 4000);
            Point crossingPoint = ProjectionZonesLogic.CalculateCrossingPointOfZoneLineAndLaserPath(new LaserMessage(0, 0, 0, 4000, -4000),
                previousMessage, new Point(-4000, -4000), new Point(4000, 4000));

            Assert.IsTrue(crossingPoint.X == 0 && crossingPoint.Y == 0);
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesTest4()
        {
            LaserMessage previousMessage = new(0, 0, 0, -4000, 4000);
            Point crossingPoint = ProjectionZonesLogic.CalculateCrossingPointOfZoneLineAndLaserPath(new LaserMessage(0, 0, 0, 4000, 0),
                previousMessage, new Point(-4000, 0), new Point(4000, 4000));

            Assert.IsTrue(crossingPoint.X == 0 && crossingPoint.Y == 2000);
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesLineDoesNotCrossTest()
        {
            LaserMessage previousMessage = new(0, 0, 0, -4000, 4000);
            Point crossingPoint = ProjectionZonesLogic.CalculateCrossingPointOfZoneLineAndLaserPath(new LaserMessage(0, 0, 0, -4000, 0),
                previousMessage, new Point(4000, 0), new Point(4000, 4000));

            Assert.IsTrue(crossingPoint.X == -4001 && crossingPoint.Y == -4001);
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesLineDoesNotCrossTest2()
        {
            LaserMessage previousMessage = new(0, 0, 0, -4000, 4000);
            Point crossingPoint = ProjectionZonesLogic.CalculateCrossingPointOfZoneLineAndLaserPath(new LaserMessage(0, 0, 0, 4000, 4000),
                previousMessage, new Point(-4000, 0), new Point(4000, 0));

            Assert.IsTrue(crossingPoint.X == -4001 && crossingPoint.Y == -4001);
        }

        [TestMethod]
        public void GetLineHitByPathTest()
        {
            LaserMessage previousMessage = new(0, 0, 0, -4000, 4000);
            ZoneLine[] zoneLinesHit = ProjectionZonesLogic.GetZoneLineHitByPath(_zones[0], new LaserMessage(0, 0, 0, 4000, 0), previousMessage);

            Point firstIntersectPosition = zoneLinesHit[0].CrossedPoint;
            Point secondIntersectPosition = zoneLinesHit[1].CrossedPoint;
            Assert.IsTrue(zoneLinesHit.Length == 2 &&
                          firstIntersectPosition.X == 1000 && firstIntersectPosition.Y == 1500 &&
                          secondIntersectPosition.X == -1000 && secondIntersectPosition.Y == 2500);
        }

        [TestMethod]
        public void GetLineHitByPathParallelogramZoneTest()
        {
            LaserMessage previousMessage = new(0, 0, 0, -3000, -1000);
            ZoneLine[] zoneLinesHit = ProjectionZonesLogic.GetZoneLineHitByPath(_zones[2], new LaserMessage(0, 0, 0, 3000, -1000), previousMessage);

            Point firstIntersectPosition = zoneLinesHit[0].CrossedPoint;
            Point secondIntersectPosition = zoneLinesHit[1].CrossedPoint;
            Assert.IsTrue(zoneLinesHit.Length == 2 &&
                          firstIntersectPosition.X == 2250 && firstIntersectPosition.Y == -1000 &&
                          secondIntersectPosition.X == -2250 && secondIntersectPosition.Y == -1000);
        }

        [TestMethod]
        public void GetLineHitByPath6PointZoneTest()
        {
            LaserMessage previousMessage = new(0, 0, 0, -3000, -1000);
            ZoneLine[] zoneLinesHit = ProjectionZonesLogic.GetZoneLineHitByPath(_zones[3], new LaserMessage(0, 0, 0, 3000, -1000), previousMessage);

            Assert.IsTrue(zoneLinesHit.Length == 3);
        }

        [TestMethod]
        public void IsInsidePolygonTest()
        {
            Point[] polygon = {
                new(-3500, 0),
                new(3500, 0),
                new(4000, -4000),
                new (-4000, -4000)
            };

            bool positionIsInsidePolygon = ProjectionZonesLogic.IsInsidePolygon(polygon, new Point(0, 0));
            Assert.IsTrue(positionIsInsidePolygon);
        }

        [TestMethod]
        public void IsNotInsidePolygonTest()
        {
            Point[] polygon = {
                new(-3500, 0),
                new(3500, 0),
                new(4000, -4000),
                new (-4000, -4000)
            };

            bool positionIsInsidePolygon = ProjectionZonesLogic.IsInsidePolygon(polygon, new Point(0, 4000));
            Assert.IsFalse(positionIsInsidePolygon);
        }

        [TestMethod]
        public void IsInsidePolygon6PointsTest()
        {
            Point[] polygon = {
                new(-3500, 0),
                new(0, 1000),
                new(3500, 0),
                new(4000, -4000),
                new(0, -3500),
                new (-4000, -4000)
            };

            bool positionIsInsidePolygon = ProjectionZonesLogic.IsInsidePolygon(polygon, new Point(0, -1000));
            Assert.IsTrue(positionIsInsidePolygon);
        }

        [TestMethod]
        public void GetZoneWherePathIsInsideTest()
        {
            LaserMessage previousMessage = new(0, 0, 0, 500, 0);
            SafetyZoneDto zone = ProjectionZonesLogic.GetZoneWherePathIsInside(new LaserMessage(0, 0, 0, 500, 0), previousMessage);
            Assert.IsTrue(zone.Uuid == Guid.Parse("fc220bc5-68ff-45d8-8e51-d884687e324b"));
        }
    }
}
