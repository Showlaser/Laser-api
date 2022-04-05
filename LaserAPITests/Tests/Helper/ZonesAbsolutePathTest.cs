using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Zones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LaserAPITests.Tests.Helper
{
    [TestClass]
    public class ZonesAbsolutePathTest
    {
        [TestMethod]
        public void GetAbsoluteValuesTest()
        {
            ZoneDto zone = new()
            {
                Positions = new List<ZonesPositionDto>
                {
                    new()
                    {
                        X = -3000,
                        Y = -4000
                    },
                    new()
                    {
                        X = -3000,
                        Y = 0
                    },
                    new()
                    {
                        X = 3000,
                        Y = 0
                    },
                    new()
                    {
                        X = -3000,
                        Y = -4000
                    }
                }
            };

            ZoneAbsolutePositionsHelper absolutePositionsHelper = new(zone);
            Assert.AreEqual(absolutePositionsHelper.LeftXAxisInZone, -3000);
            Assert.AreEqual(absolutePositionsHelper.RightXAxisInZone, 3000);
            Assert.AreEqual(absolutePositionsHelper.HighestYAxisInZone, 0);
            Assert.AreEqual(absolutePositionsHelper.LowestYAxisInZone, -4000);
        }
    }
}
