using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Zones;
using NUnit.Framework;

namespace LaserAPITests.Tests.Helper
{
    [TestFixture]
    internal class ZonesAbsolutePathTest
    {
        [Test]
        public void GetAbsoluteValuesTest()
        {
            ZoneDto zone = new()
            {
                Positions = new ZonesPositionDto[]
                {
                    new()
                    {
                        X = -3000,
                        Y = 4000
                    },
                    new()
                    {
                        X = 3000,
                        Y = -4000
                    }
                }
            };

            ZoneAbsolutePositionsHelper absolutePositionsHelper = new(zone);
            Assert.AreEqual(absolutePositionsHelper.LeftXAxisInZone, -3000);
            Assert.AreEqual(absolutePositionsHelper.RightXAxisInZone, 3000);
            Assert.AreEqual(absolutePositionsHelper.HighestYAxisInZone, 4000);
            Assert.AreEqual(absolutePositionsHelper.LowestYAxisInZone, -4000);
        }
    }
}
