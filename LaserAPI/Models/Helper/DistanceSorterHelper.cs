namespace LaserAPI.Models.Helper
{
    public class DistanceSorterHelper
    {
        public LaserMessage Message { get; set; }
        public double Distance { get; set; }

        public DistanceSorterHelper(LaserMessage message, double distance)
        {
            Message = message;
            Distance = distance;
        }
    }
}
