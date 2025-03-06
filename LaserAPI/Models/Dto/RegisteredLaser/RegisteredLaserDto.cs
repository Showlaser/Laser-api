using LaserAPI.Enums;
using System;

namespace LaserAPI.Models.Dto.RegisteredLaser
{
    public class RegisteredLaserDto
    {
        public Guid Uuid { get; set; }
        public string LaserId { get; set; }
        public string Name { get; set; }
        public string ModelType { get; set; }
        public LaserStatus Status { get; set; }
        public string IPAddress { get; set; }
    }
}
