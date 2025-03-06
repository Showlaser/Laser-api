using LaserAPI.Models.Dto.Animations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface IAnimationDal
    {
        public Task Add(AnimationDto animation);
        public Task<List<AnimationDto>> All();
        public Task<bool> Exists(Guid uuid);
        public Task Update(AnimationDto animation);
        public Task Remove(Guid uuid);
    }
}