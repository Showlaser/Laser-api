namespace LaserAPI.Models.Helper
{
    public class LaserMessage
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

        }
    }
}