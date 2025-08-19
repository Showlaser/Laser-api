namespace LaserAPI.Enums
{
    public enum LaserModel
    {
        Version5
    }

    public enum LaserStatus
    {
        Emitting,
        Standby,
        PoweredOff,
        EmergencyButtonPressed,
        PendingConnection,
        ConnectionLost
    }
}
