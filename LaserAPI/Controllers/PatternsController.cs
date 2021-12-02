using LaserAPI.Logic;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.FromFrontend.Patterns;
using LaserAPI.Models.ToFrontend.Pattern;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("pattern")]
    [ApiController]
    public class PatternsController : ControllerBase
    {
        private readonly PatternLogic _patternLogic;

        public PatternsController(PatternLogic patternLogic)
        {
            _patternLogic = patternLogic;
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Pattern pattern)
        {
            try
            {
                PatternDto patternDto = pattern.Adapt<PatternDto>();
                await _patternLogic.Add(patternDto);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult<List<PatternViewmodel>>> All()
        {
            try
            {
                List<PatternDto> patterns = await _patternLogic.All();
                return patterns.Adapt<List<PatternViewmodel>>();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> Update(Pattern pattern)
        {
            try
            {
                PatternDto patternDto = pattern.Adapt<PatternDto>();
                await _patternLogic.Update(patternDto);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ActionResult> Remove(Guid uuid)
        {
            try
            {
                await _patternLogic.Remove(uuid);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}