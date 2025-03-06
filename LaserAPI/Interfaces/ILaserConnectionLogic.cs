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
        /// Sends a data request to the lasers to send telemetry data back, the result is returned
        /// </summary>
        /// <returns>A list with telemetry data</returns>
        public static Task<List<RegisteredLaserDto>> Status { get; }

        /// <summary>
        /// Removes the specified laser from the database and the connected laser list
        /// </summary>
        /// <param name="registeredLaser">The laser to remove</param>
        public Task Remove(Guid uuid);
    }
}
