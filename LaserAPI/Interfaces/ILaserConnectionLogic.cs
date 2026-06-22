using LaserAPI.Models.Dto.RegisteredLaser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaserAPI.Interfaces
{
    public interface ILaserConnectionLogic
    {
        /// <summary>
        /// Connect to the laser with the specified ip and uses the password for authentication
        /// </summary>
        /// <param name="laserUuid">The Uuid of the laser to connect to</param>
        public Task Connect(RegisteredLaserDto registeredLaser);

        /// <summary>
        /// Adopts the laser and sends a request connection to the laser
        /// </summary>
        /// <param name="registeredLaser"></param>
        /// <returns></returns>
        public Task<bool> Adopt(RegisteredLaserDto registeredLaser);

        /// <summary>
        /// Updates the data of the showlaser
        /// </summary>
        public Task Update(RegisteredLaserDto registeredLaser);

        /// <summary>
        /// Removes the specified laser from the database and the connected laser list
        /// </summary>
        /// <param name="registeredLaser">The laser to remove</param>
        public Task Remove(Guid uuid);
    }
}
