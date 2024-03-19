using LaserAPI.Logic;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.FromFrontend.Animations;
using LaserAPI.Models.Helper;
using LaserAPI.Models.ToFrontend.Animations;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LaserAPI.Controllers
{
    [Route("animation")]
    [ApiController]
    public class AnimationController(AnimationLogic animationLogic, ControllerResultHandler controllerResultHandler) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate([FromBody] Animation animation)
        {
            async Task Action()
            {
                AnimationDto animationDto = animation.Adapt<AnimationDto>();
                await animationLogic.AddOrUpdate(animationDto);
            }

            return await controllerResultHandler.Execute(Action());
        }

        [HttpPost("play")]
        public async Task<ActionResult> PlayAnimation([FromBody] Animation animation)
        {
            async Task Action()
            {
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                AnimationDto animationDto = animation.Adapt<AnimationDto>();
                await AnimationLogic.PlayAnimation(animationDto);
            }

            return await controllerResultHandler.Execute(Action());
        }

        [HttpGet]
        public async Task<ActionResult<List<AnimationViewModel>>> All()
        {
            async Task<List<AnimationViewModel>> Action()
            {
                List<AnimationDto> animations = await animationLogic.All();
                return animations.Adapt<List<AnimationViewModel>>();
            }

            return await controllerResultHandler.Execute(Action());
        }

        [HttpDelete("{uuid}")]
        public async Task<ActionResult> Remove(Guid uuid)
        {
            async Task Action()
            {
                await animationLogic.Remove(uuid);
            }

            return await controllerResultHandler.Execute(Action());
        }
    }
}
