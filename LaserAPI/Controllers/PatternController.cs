
using LaserAPI.Logic;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.FromFrontend.Patterns;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend.Pattern;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("pattern")]
    [ApiController]
    public class PatternController : ControllerBase
    {
        private readonly PatternLogic _patternLogic;
        private readonly ControllerResultHandler _controllerResultHandler;

        public PatternController(PatternLogic patternLogic, ControllerResultHandler controllerResultHandler)
        {
            _patternLogic = patternLogic;
            _controllerResultHandler = controllerResultHandler;
        }

        [HttpPost("play")]
        public ActionResult PlayPattern([FromBody] Pattern pattern)
        {
            void Action()
            {
                PatternDto patternDto = pattern.Adapt<PatternDto>();
                _patternLogic.PlayPattern(patternDto);
            }

            return _controllerResultHandler.Execute(Action);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] Pattern pattern)
        {
            async Task Action()
            {
                PatternDto patternDto = pattern.Adapt<PatternDto>();
                await _patternLogic.AddOrUpdate(patternDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<PatternViewmodel>>> All()
        {
            async Task<List<PatternViewmodel>> Action()
            {
                List<PatternDto> patterns = await _patternLogic.All();
                return patterns.Adapt<List<PatternViewmodel>>();
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            async Task Action()
            {
                await _patternLogic.Remove(uuid);
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}