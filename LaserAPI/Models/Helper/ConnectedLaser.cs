using LaserAPI.Enums;
using System;

namespace LaserAPI.Models.Helper
{
    public class ConnectedLaser
    {
        public Guid Uuid { get; set; }
        public string LaserConnectionId { get; set; }
        public string Name { get; set; }
        public string ModelType { get; set; }
        public LaserStatus Status { get; set; }
        public bool Online { get; set; }
        public string IPAddress { get; set; }
        public string Password { get; set; }
    }
}
