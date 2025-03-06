using LaserAPI.Enums;

namespace LaserAPI.Models.FromFrontend.Laser
{
    public class LaserTelemetry
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string ModelType { get; set; }
        public LaserStatus Status { get; set; }
    }
}
