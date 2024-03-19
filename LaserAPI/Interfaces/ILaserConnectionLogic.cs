using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.Helper;
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
        public Task Connect(string ipAddress, string laserId, string password);

        /// <summary>
        /// Sends data to the lasers to project
        /// </summary>
        /// <param name="wrappedPoints">The points to project</param>
        public Task SendData(List<PointWrapper> wrappedPoints);

        /// <summary>
        /// Disconnect from the laser
        /// </summary>
        /// <param name="laserUuid">The UUID of the laser to disconnect from</param>
        public void Disconnect(string laserId);

        /// <summary>
        /// Sends a data request to the lasers to send telemetry data back, the result is returned
        /// </summary>
        /// <returns>A list with telemetry data</returns>
        public Task<List<ConnectedLaser>> GetStatus();
    }
}
