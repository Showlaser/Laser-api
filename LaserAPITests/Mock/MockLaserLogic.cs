using LaserAPI.Interfaces.Dal;
using LaserAPI.Interfaces.Helper;
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
            MockedZones mockedZones = new MockedZones();

            var mockedLaserConnectionLogic = new Mock<ILaserConnectionLogic>();
            var mockedZoneDal = new Mock<IZoneDal>();
            mockedZoneDal.Setup(d => d.All()).ReturnsAsync(mockedZones.Zones);
            var mockedLaserLogic = new LaserLogic(mockedLaserConnectionLogic.Object, mockedZoneDal.Object);

            LaserLogic = mockedLaserLogic;
        }
    }
}
