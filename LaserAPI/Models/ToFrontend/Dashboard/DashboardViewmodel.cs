using System.Collections.Generic;

namespace LaserAPI.Models.ToFrontend.Dashboard
{
    public class DashboardViewmodel
    {
        public LaserStatusViewmodel LaserStatus { get; set; }
        public LaserSettingsViewmodel LaserSettings { get; set; }
        public List<LogViewmodel> Logs { get; set; }
        public List<ShowViewmodel> Shows { get; set; }
    }
}
