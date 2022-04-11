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

            ControllerResultHandler controllerResultHandler = new(); // todo dp injection
            return controllerResultHandler.Execute(Action);
        }

        [HttpPost("stop")]
        public ActionResult Stop()
        {
            ControllerResultHandler controllerResultHandler = new();
            return controllerResultHandler.Execute(() => _gameLogic.Stop());
        }

        [HttpPost("move")]
        public ActionResult Move([FromQuery] GameMovement movement)
        {
            ControllerResultHandler controllerResultHandler = new();
            return controllerResultHandler.Execute(() => _gameLogic.Move(movement));
        }

        [HttpGet]
        public ActionResult<List<string>> GetNames()
        {
            ControllerResultHandler controllerResultHandler = new();
            return controllerResultHandler.Execute(() => _gameLogic.GetGameNames());
        }
    }
}
