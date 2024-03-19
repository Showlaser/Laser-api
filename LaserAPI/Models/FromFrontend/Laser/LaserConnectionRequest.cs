using LaserAPI.Enums;

namespace LaserAPI.Models.FromFrontend.Laser
{
    public class LaserConnectionRequest
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string ModelType { get; set; }
        public readonly LaserStatus Status = LaserStatus.PendingConnection;
        public string Password { get; set; }
    }
}