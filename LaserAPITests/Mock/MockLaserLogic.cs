using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPITests.MockedModels.Zones;
using Moq;

namespace LaserAPITests.Mock
{
    internal class MockLaserLogic
    {
        internal LaserLogic LaserLogic;

        public MockLaserLogic()
        {
            MockedZones mockedZones = new();

            Mock<IZoneDal>? mockedZoneDal = new Mock<IZoneDal>();
            mockedZoneDal.Setup(d => d.All()).ReturnsAsync(mockedZones.Zones);
            LaserLogic? mockedLaserLogic = new LaserLogic(mockedZoneDal.Object);
            LaserConnectionLogic.RanByUnitTest = true;

            LaserLogic = mockedLaserLogic;
        }
    }
}
