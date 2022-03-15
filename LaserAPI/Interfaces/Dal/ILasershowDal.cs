using LaserAPI.Models.Dto.Lasershow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces.Dal
{
    public interface ILasershowDal
    {
        /// <summary>
        /// Adds the lasershow to the database
        /// </summary>
        /// <param name="lasershow">The lasershow to add</param>
        public Task Add(LasershowDto lasershow);

        /// <returns>All the shows in the database</returns>
        public Task<List<LasershowDto>> All();

        /// <summary>
        /// Updates the lasershow in the database
        /// </summary>
        /// <param name="lasershow">The updated lasershow</param>
        public Task Update(LasershowDto lasershow);

        /// <summary>
        /// Removes the lasershow which matches the uuid
        /// </summary>
        /// <param name="uuid">The uuid of the lasershow to remove</param>
        public Task Remove(Guid uuid);
    }
}
