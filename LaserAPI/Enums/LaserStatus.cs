namespace LaserAPI.Enums
{
    public enum LaserStatus
    {
        Emitting = 0,
        Standby = 1,
        EmergencyButtonPressed = 2,
        PendingConnection = 3,
        ConnectionLost = 4,
        Defect = 5,
        NotConfigured = 6
    }

    public enum LaserModel
    {
        Version5
    }
}
