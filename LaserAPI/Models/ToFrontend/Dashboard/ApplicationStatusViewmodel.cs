namespace LaserAPI.Models.ToFrontend.Dashboard
{
    public class ApplicationStatusViewmodel
    {
        public bool? LaserConnected { get; set; }
        public string ComputerIpAddress { get; set; }

        public ApplicationStatusViewmodel(bool? laserConnected, string computerIpAddress)
        {
            LaserConnected = laserConnected ?? false;
            ComputerIpAddress = computerIpAddress;
        }
    }
}
