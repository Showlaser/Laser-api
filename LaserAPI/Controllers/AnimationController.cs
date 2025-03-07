﻿using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.FromFrontend.Animations;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend.Animations;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("animation")]
    [ApiController]
    public class AnimationController(AnimationLogic _animationLogic, ControllerResultHandler _controllerResultHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] Animation animation)
        {
            async Task Action()
            {
                AnimationDto animationDto = animation.Adapt<AnimationDto>();
                await _animationLogic.AddOrUpdate(animationDto);
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<AnimationViewModel>>> All()
        {
            async Task<List<AnimationViewModel>> Action()
            {
                List<AnimationDto> animations = await _animationLogic.All();
                return animations.Adapt<List<AnimationViewModel>>();
            }

            return await _controllerResultHandler.Execute(Action());
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            async Task Action()
            {
                await _animationLogic.Remove(uuid);
            }

            return await _controllerResultHandler.Execute(Action());
        }
    }
}
