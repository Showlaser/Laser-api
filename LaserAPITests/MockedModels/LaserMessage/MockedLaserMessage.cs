namespace LaserAPITests.MockedModels.LaserMessage
{
    public class MockedLaserMessage
    {
        public readonly LaserAPI.Models.Helper.LaserMessage LaserMessage = new()
        {
            RedLaser = 255,
            GreenLaser = 255,
            BlueLaser = 255,
            X = 0,
            Y = 0
        };
    }
}
