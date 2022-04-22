namespace LaserAPI.Models.Helper
{
    public struct LaserMessage
    {
        public int RedLaser { get; set; }
        public int GreenLaser { get; set; }
        public int BlueLaser { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public LaserMessage(int redLaser, int greenLaser, int blueLaser, int x, int y)
        {
            RedLaser = redLaser;
            GreenLaser = greenLaser;
            BlueLaser = blueLaser;
            X = x;
            Y = y;
        }

        public LaserMessage()
        {
            RedLaser = 0;
            GreenLaser = 0;
            BlueLaser = 0;
            X = 0;
            Y = 0;
        }
    }
}