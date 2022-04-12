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
        private readonly ControllerResultHandler _controllerResultHandler;

        public GameController(GameLogic gameLogic, ControllerResultHandler controllerResultHandler)
        {
            _gameLogic = gameLogic;
            _controllerResultHandler = controllerResultHandler;
        }

        [HttpPost("start")]
        public ActionResult Start([FromQuery] string gameName)
        {
            void Action()
            {
                _gameLogic.Start(gameName);
            }

            return _controllerResultHandler.Execute(Action);
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
            return _controllerResultHandler.Execute(() => _gameLogic.Move(movement));
        }

        [HttpGet]
        public ActionResult<List<string>> GetNames()
        {
            return _controllerResultHandler.Execute(() => _gameLogic.GetGameNames());
        }
    }
}
