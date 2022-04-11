namespace LaserAPI.Models.Helper
{
    public class DistanceSorterHelper
    {
        public LaserMessage Message { get; set; }
        public double Distance { get; set; }
        public int TotalLaserPower { get; set; }

        public DistanceSorterHelper(LaserMessage message, double distance, int totalLaserPower)
        {
            Message = message;
            Distance = distance;
            TotalLaserPower = totalLaserPower;
        }
    }
}
