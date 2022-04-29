namespace LaserAPI.Models.Helper
{
    public struct DistanceSorter
    {
        public LaserMessage Message { get; set; }
        public double Distance { get; set; }
        public int TotalLaserPower { get; set; }

        public DistanceSorter(LaserMessage message, double distance, int totalLaserPower)
        {
            Message = message;
            Distance = distance;
            TotalLaserPower = totalLaserPower;
        }
    }
}
