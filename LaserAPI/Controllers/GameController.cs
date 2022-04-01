using LaserAPI.Enums;
using LaserAPI.Logic.Game;
using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LaserAPI.Controllers
{
    [Route("game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameLogic _gameLogic;

        public GameController(GameLogic gameLogic)
        {
            _gameLogic = gameLogic;
        }

        [HttpPost("start")]
        public ActionResult Start([FromQuery] string gameName)
        {
            void Action()
            {
                _gameLogic.Start(gameName);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return controllerErrorHandler.Execute(Action);
        }

        [HttpPost("stop")]
        public ActionResult Stop()
        {
            ControllerErrorHandler controllerErrorHandler = new();
            return controllerErrorHandler.Execute(() => _gameLogic.Stop());
        }

        [HttpPost("move")]
        public ActionResult Move([FromQuery] GameMovement movement)
        {
            ControllerErrorHandler controllerErrorHandler = new();
            return controllerErrorHandler.Execute(() => _gameLogic.Move(movement));
        }

        [HttpGet]
        public ActionResult<List<string>> GetNames()
        {
            ControllerErrorHandler controllerErrorHandler = new();
            return controllerErrorHandler.Execute(() => _gameLogic.GetGameNames());
        }
    }
}
