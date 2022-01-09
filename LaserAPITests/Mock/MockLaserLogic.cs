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

            var mockedLaserConnectionLogic = new LaserConnectionLogic(true);
            var mockedZoneDal = new Mock<IZoneDal>();
            mockedZoneDal.Setup(d => d.All()).ReturnsAsync(mockedZones.Zones);
            var mockedLaserLogic = new LaserLogic(mockedLaserConnectionLogic, mockedZoneDal.Object);

            LaserLogic = mockedLaserLogic;
        }
    }
}
