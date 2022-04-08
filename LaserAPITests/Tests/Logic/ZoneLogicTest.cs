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
        private readonly List<ZoneDto> _zones;

        public ZoneLogicTest()
        {
            IZoneDal zoneDal = new MockedZoneDal().ZoneDal;
            _zones = zoneDal
                .All()
                .Result;
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesTest()
        {
            LaserConnectionLogic.PreviousLaserMessage.X = -4000;
            LaserConnectionLogic.PreviousLaserMessage.Y = 4000;
            Point crossingPoint = ZoneLogic.CalculateCrossingPointOfTwoLines(new LaserMessage(0, 0, 0, 4000, 0),
                new Point(-1000, 3000), new Point(-1000, 0));

            Assert.IsTrue(crossingPoint.X == -1000 && crossingPoint.Y == 2500);
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesTest2()
        {
            LaserConnectionLogic.PreviousLaserMessage.X = -2000;
            LaserConnectionLogic.PreviousLaserMessage.Y = 3000;
            Point crossingPoint = ZoneLogic.CalculateCrossingPointOfTwoLines(new LaserMessage(0, 0, 0, 4000, 0),
                new Point(-1000, 3000), new Point(-1000, 0));

            Assert.IsTrue(crossingPoint.X == -1000 && crossingPoint.Y == 2500);
        }

        [TestMethod]
        public void CalculateCrossingPointBetweenTwoLinesTest3()
        {
            LaserConnectionLogic.PreviousLaserMessage.X = -4000;
            LaserConnectionLogic.PreviousLaserMessage.Y = 4000;
            Point crossingPoint = ZoneLogic.CalculateCrossingPointOfTwoLines(new LaserMessage(0, 0, 0, 4000, -4000),
                new Point(-4000, -4000), new Point(4000, 4000));

            Assert.IsTrue(crossingPoint.X == 0 && crossingPoint.Y == 0);
        }
    }
}
