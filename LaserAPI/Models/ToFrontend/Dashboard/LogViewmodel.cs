using LaserAPI.Enums;

namespace LaserAPI.Models.ToFrontend.Dashboard
{
    public class LogViewmodel
    {
        public string Message { get; set; }
        public LogType LogType { get; set; }

        public LogViewmodel(string message, LogType logType)
        {
            Message = message;
            LogType = logType;
        }
    }
}
