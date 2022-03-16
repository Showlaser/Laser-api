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
            controllerErrorHandler.Execute(Action);
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [HttpPost("stop")]
        public ActionResult Stop()
        {
            void Action()
            {
                _gameLogic.Stop();
            }

            ControllerErrorHandler controllerErrorHandler = new();
            controllerErrorHandler.Execute(Action);
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [HttpPost("move")]
        public ActionResult Move([FromQuery] GameMovement movement)
        {
            void Action()
            {
                _gameLogic.Move(movement);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            controllerErrorHandler.Execute(Action);
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [HttpGet]
        public ActionResult<List<string>> GetNames()
        {
            ControllerErrorHandler controllerErrorHandler = new();
            return controllerErrorHandler.Execute(_gameLogic.GetGameNames);
        }
    }
}
