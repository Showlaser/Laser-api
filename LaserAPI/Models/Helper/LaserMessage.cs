using System.Runtime.Serialization;

namespace LaserAPI.Models.Helper
{
    public struct LaserMessage
    {
        [DataMember(Name = "r")]
        public int RedLaser { get; set; }

        [DataMember(Name = "g")]
        public int GreenLaser { get; set; }

        [DataMember(Name = "b")]
        public int BlueLaser { get; set; }

        [DataMember(Name = "x")]
        public int X { get; set; }

        [DataMember(Name = "y")]
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