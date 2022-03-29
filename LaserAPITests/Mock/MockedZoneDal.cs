using LaserAPI.Interfaces.Dal;
using LaserAPITests.MockedModels.Zones;
using Moq;

namespace LaserAPITests.Mock
{
    public class MockedZoneDal
    {
        public readonly IZoneDal ZoneDal;

        public MockedZoneDal()
        {
            MockedZones mockedZones = new();
            Mock<IZoneDal> mockedZoneDal = new();
            mockedZoneDal.Setup(d => d.All()).ReturnsAsync(mockedZones.Zones);
            ZoneDal = mockedZoneDal.Object;
        }
    }
}
