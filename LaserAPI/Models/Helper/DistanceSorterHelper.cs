using System.Drawing;

namespace LaserAPI.Models.Helper
{
    public class DistanceSorterHelper
    {
        public Point Point { get; set; }
        public double Distance { get; set; }

        public DistanceSorterHelper(Point point, double distance)
        {
            Point = point;
            Distance = distance;
        }
    }
}
