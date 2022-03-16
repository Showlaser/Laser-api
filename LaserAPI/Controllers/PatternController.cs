﻿using LaserAPI.Logic;
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

        public PatternController(PatternLogic patternLogic)
        {
            _patternLogic = patternLogic;
        }

        [HttpPost("play")]
        public async Task<ActionResult> PlayPattern([FromBody] Pattern pattern)
        {
            async Task Action()
            {
                PatternDto patternDto = pattern.Adapt<PatternDto>();
                await _patternLogic.PlayPattern(patternDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] Pattern pattern)
        {
            async Task Action()
            {
                PatternDto patternDto = pattern.Adapt<PatternDto>();
                await _patternLogic.AddOrUpdate(patternDto);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }

        [HttpGet]
        public async Task<ActionResult<List<PatternViewmodel>>> All()
        {
            async Task<List<PatternViewmodel>> Action()
            {
                List<PatternDto> patterns = await _patternLogic.All();
                return patterns.Adapt<List<PatternViewmodel>>();
            }

            ControllerErrorHandler controllerErrorHandler = new();
            return await controllerErrorHandler.Execute(Action());
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            async Task Action()
            {
                await _patternLogic.Remove(uuid);
            }

            ControllerErrorHandler controllerErrorHandler = new();
            await controllerErrorHandler.Execute(Action());
            return StatusCode(controllerErrorHandler.StatusCode);
        }
    }
}