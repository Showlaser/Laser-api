using System.IO;
using LaserAPI.Logic;
using LaserAPI.Models.Dto;
using LaserAPI.Models.FromFrontend;
using LaserAPI.Models.Helper;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace LaserAPI.Controllers
{
    [Route("settings")]
    [ApiController]
    public class LocalSettingsController : ControllerBase
    {
        private readonly ControllerResultHandler _controllerResultHandler;

        public LocalSettingsController(ControllerResultHandler controllerResultHandler)
        {
            _controllerResultHandler = controllerResultHandler;
        }

        [HttpPost]
        public ActionResult SetSettings([FromBody] LocalSettings localSettings)
        {
            void Action()
            {
                LocalSettingsDto localSettingsDto = localSettings.Adapt<LocalSettingsDto>();

            }

            return _controllerResultHandler.Execute(Action);
        }

        [HttpPost("serial")]
        public ActionResult SendSerialMessageToLaser([FromQuery] string comport, [FromQuery] string ip)
        {
            void Action()
            {
                LaserConnectionLogic.ConnectSerial(comport);
                LaserConnectionLogic.SetLaserSettingsBySerial(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    ip
                }));
            }

            return _controllerResultHandler.Execute(Action);
        }

        [HttpGet("serial")]
        public ActionResult<string[]> GetComDevices()
        {
            static string[] Action()
            {
                return LaserConnectionLogic.GetAvailableComDevices();
            }

            return _controllerResultHandler.Execute(Action);
        }

        [HttpPost("connection-method")]
        public void SetConnectionMethod([FromQuery] string connectionMethod, [FromQuery] string comPort)
        {
            void Action()
            {
                bool connectionMethodIsValid = connectionMethod == "Network" || connectionMethod == "Usb";
                bool comPortIsValid = !string.IsNullOrEmpty(comPort) && comPort.Contains("COM");
                if (!connectionMethodIsValid || !comPortIsValid)
                {
                    throw new InvalidDataException();
                }

                if (connectionMethod == "Usb")
                {
                    LaserConnectionLogic.ComPort = comPort;
                }

                LaserConnectionLogic.ConnectionMethod = connectionMethod;
            }

            _controllerResultHandler.Execute(Action);
        }

        [HttpGet("current-com-device")]
        public ActionResult<string> GetCurrentComDevice()
        {
            return LaserConnectionLogic.ComPort;
        }
    }
}
