using LaserAPI.Interfaces;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.RegisteredLaser;
using LaserAPI.Models.FromFrontend.Laser;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.FromLaser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public static class LaserConnectionLogicState
    {
        public static List<RegisteredLaserDto> ConnectedLasers { get; set; } = [];
        public static List<UDPBroadcast> AdoptionPending { get; set; } = [];
    }

    public class LaserConnectionLogic(IRegisteredLaserDal _registeredLaserDal) : ILaserConnectionLogic
    {
        public async Task<bool> Adopt(RegisteredLaserDto registeredLaser)
        {
            try
            {
                using HttpClient httpClient = new();
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(registeredLaser);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync($"http://{registeredLaser.IPAddress}/adopt", content);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

       public async Task Connect(RegisteredLaserDto registeredLaser)
        {
           /* RegisteredLaserDto registerLaserFromDb = await _registeredLaserDal.Find(registeredLaser.LaserId);
            int connectedLaserFromListIndex = LaserConnectionLogicState.ConnectedLasers.FindIndex(cl => cl.IPAddress == registerLaserFromDb.IPAddress);
            if (connectedLaserFromListIndex != -1)
            {
                LaserConnectionLogicState.ConnectedLasers[connectedLaserFromListIndex].Status = registeredLaser.Status;
            }

            RegisteredLaserDto connectedLaser = new()
            {
                Uuid = Guid.NewGuid(),
                LaserId = registeredLaser.LaserId,
                Name = registeredLaser.Name,
                ModelType = registeredLaser.ModelType,
                Status = registeredLaser.Status,
                IPAddress = registeredLaser.IPAddress,
            };

            LaserConnectionLogicState.ConnectedLasers.Add(connectedLaser);
            await _registeredLaserDal.Add(registeredLaser);*/
        }

        /// <summary>
        /// Gets the status from all lasers in the database that are online
        /// This function is called every 30 seconds in Startup.cs to keep the list updated
        /// </summary>
        /// <returns>The results of all the lasers in the connected lasers list</returns>
        public static async Task<List<RegisteredLaserDto>> GetStatus()
        {
            using HttpClient httpClient = new();

            int length = LaserConnectionLogicState.ConnectedLasers.Count;
            for (int i = 0; i < length; i++)
            {
                RegisteredLaserDto connectedLaser = LaserConnectionLogicState.ConnectedLasers[i];
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"{connectedLaser.IPAddress}/telemetry");
                    LaserTelemetry laserTelemetry = await response.Content.ReadFromJsonAsync<LaserTelemetry>();
                }
                catch (HttpRequestException)
                {
                }
            }

            httpClient.Dispose();
            return LaserConnectionLogicState.ConnectedLasers;
        }

        public static List<UDPBroadcast> GetPendingAdoptions()
        {
            return LaserConnectionLogicState.AdoptionPending;
        }

        /// <summary>
        /// Sends data to the specified laser
        /// </summary>
        /// <param name="wrappedPoints">A model with points and info about the laser to send the data to</param>
        public static async Task SendData(List<PointWrapper> wrappedPoints)
        {
            using HttpClient httpClient = new();
            List<Task> tasks = [];

            foreach (RegisteredLaserDto connectedLaser in LaserConnectionLogicState.ConnectedLasers.FindAll(cl =>
                cl.Status is (LaserStatus)Enums.LaserStatus.Emitting or
                (LaserStatus)Enums.LaserStatus.Standby))
            {
                string url = $"{connectedLaser.IPAddress}/send";
                List<PointWrapper> wrappedPointToPlayOnLaser = wrappedPoints.FindAll(wp => wp.LaserToProjectOnUuid == connectedLaser.Uuid);

                tasks.Add(httpClient.PostAsJsonAsync(url, wrappedPointToPlayOnLaser));
            }

            await Task.WhenAll(tasks);
            httpClient.Dispose();
        }

        /// <summary>
        /// Removes the specified laser from the database and the connected laser list
        /// </summary>
        /// <param name="registeredLaser">The laser to remove</param>
        public async Task Remove(Guid uuid)
        {
            await _registeredLaserDal.Remove(uuid);
            LaserConnectionLogicState.ConnectedLasers.RemoveAll(cl => cl.Uuid == uuid);
        }
    }
}
