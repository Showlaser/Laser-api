using LaserAPI.Enums;

namespace LaserAPI.Models.FromLaser
{
    public class RegisteredLaserViewModel
    {
        public string LaserId { get; set; }
        public string Name { get; set; }
        public string ModelType { get; set; }
        public LaserStatus Status { get; set; }
        public string IPAddress { get; set; }
    }
}
