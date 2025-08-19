using LaserAPI.Enums;
using System;

namespace LaserAPI.Models.Dto.RegisteredLaser
{
    public enum LaserStatus
    {
        Emitting,
        Standby,
        PoweredOff,
        EmergencyButtonPressed,
        PendingConnection,
        ConnectionLost,
    }

    public enum LaserModel
    {
        Version5
    }

    public class RegisteredLaserDto
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public LaserModel ModelType { get; set; }
        public LaserStatus Status { get; set; }
        public string IPAddress { get; set; }
    }
}
