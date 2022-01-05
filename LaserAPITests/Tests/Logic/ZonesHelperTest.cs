using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper.Laser;
using LaserAPI.Models.Helper.Zones;
using LaserAPITests.MockedModels.Zones;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace LaserAPITests.Tests.Logic
{
    [TestFixture]
    public class ZonesHelperTest
    {
        private readonly List<ZoneDto> _zones;

        public ZonesHelperTest()
        {
            MockedZones mockedZones = new MockedZones();
            var mockedZoneDal = new Mock<IZoneDal>();
            mockedZoneDal.Setup(d => d.All()).ReturnsAsync(mockedZones.Zones);

            _zones = mockedZoneDal.Object
                .All()
                .Result;

            //TODO move this to different class
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

            ZoneDto zone = ZonesHelper.GetZoneWherePositionIsIn(message.X, message.Y, _zones);
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

            ZoneDto zone = ZonesHelper.GetZoneWherePositionIsIn(message.X, message.Y, _zones);
            Assert.Null(zone);
        }
    }
}
