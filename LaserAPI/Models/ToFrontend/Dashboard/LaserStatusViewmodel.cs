namespace LaserAPI.Models.ToFrontend.Dashboard
{
    public class LaserStatusViewmodel
    {
        public bool Connected { get; set; }

        public LaserStatusViewmodel(bool connected)
        {
            Connected = connected;
        }
    }
}
