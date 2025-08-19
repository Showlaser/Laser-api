using LaserAPI.Enums;
using System;

namespace LaserAPI.Models.FromLaser
{
    public class RegisteredLaser
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public LaserModel ModelType { get; set; }
        public LaserStatus Status { get; set; }
        public string IPAddress { get; set; }
    }
}
