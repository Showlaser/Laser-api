using System.Drawing;

namespace LaserAPI.Models.Helper
{
    public class ZoneLine
    {
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public Point CrossedPoint { get; set; }
        public int Index { get; set; }
    }
}
