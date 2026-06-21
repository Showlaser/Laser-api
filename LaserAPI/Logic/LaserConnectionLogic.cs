using LaserAPI.Interfaces;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.RegisteredLaser;
using LaserAPI.Models.FromFrontend.Laser;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.FromLaser;
using LaserAPI.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static List<RegisteredLaserDto> RegisteredLasers { get; set; } = [];
        public static List<UDPBroadcast> AdoptionPending { get; set; } = [];
    }

    public class LaserConnectionLogic(IRegisteredLaserDal _registeredLaserDal) : ILaserConnectionLogic
    {
        public async Task Init()
        {
            LaserConnectionLogicState.RegisteredLasers = await _registeredLaserDal.All();
        }

        public async Task<bool> Adopt(RegisteredLaserDto registeredLaser)
        {
            try
            {
                Console.WriteLine($"Adoption requested for {registeredLaser.Name}");
                using HttpClient httpClient = new();
                string json = JsonConvert.SerializeObject(registeredLaser);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync($"http://{registeredLaser.IPAddress}/adopt", content);
                response.EnsureSuccessStatusCode();

                Console.WriteLine("Adoption sended successfull");

                LaserConnectionLogicState.AdoptionPending.RemoveAll(laser => laser.Uuid == registeredLaser.Uuid);
                RegisteredLaserDto registeredLaserDto = await _registeredLaserDal.Find(registeredLaser.Uuid);
                if (registeredLaserDto == null)
                {
                    await _registeredLaserDal.Add(registeredLaser);
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

       public async Task Connect(RegisteredLaserDto registeredLaser)
        {
            RegisteredLaserDto registerLaserFromDb = (await _registeredLaserDal.Find(registeredLaser.Uuid)) ?? throw new KeyNotFoundException();
            int connectedLaserFromListIndex = LaserConnectionLogicState.RegisteredLasers.FindIndex(cl => cl.Uuid == registerLaserFromDb?.Uuid);
            if (connectedLaserFromListIndex != -1)
            {
                LaserConnectionLogicState.RegisteredLasers[connectedLaserFromListIndex].Status = registeredLaser.Status;
                LaserConnectionLogicState.RegisteredLasers[connectedLaserFromListIndex].IPAddress = registeredLaser.IPAddress;
            }
            else
            {
                LaserConnectionLogicState.RegisteredLasers.Add(registerLaserFromDb);
            }

            await _registeredLaserDal.Update(registeredLaser);
            LaserConnectionLogicState.RegisteredLasers.RemoveAll(rl => rl.Uuid == registeredLaser.Uuid);
            LaserConnectionLogicState.RegisteredLasers.Add(registeredLaser);
        }

        private static bool ShowlaserValid(RegisteredLaserDto registeredLaserDto)
        {
            return registeredLaserDto.Uuid != Guid.Empty && 
                registeredLaserDto.Name.Length > 0 && 
                registeredLaserDto.IPAddress.Length > 0 &&
                registeredLaserDto.MaxPowerPerlaserInPercentage.IsBetweenOrEqualTo(0, 100) && 
                registeredLaserDto.ProjectionTopInPercentage.IsBetweenOrEqualTo(0, 100) && 
                registeredLaserDto.ProjectionBottomInPercentage.IsBetweenOrEqualTo(0, 100) && 
                registeredLaserDto.ProjectionLeftInPercentage.IsBetweenOrEqualTo(0, 100) && 
                registeredLaserDto.ProjectionRightInPercentage.IsBetweenOrEqualTo(0, 100);
        }

        public async Task UpdateShowlaser(RegisteredLaserDto registeredLaser)
        {
            if (!ShowlaserValid(registeredLaser))
            {
                throw new InvalidDataException();
            }

            RegisteredLaserDto registerLaserFromDb = (await _registeredLaserDal.Find(registeredLaser.Uuid)) ?? throw new KeyNotFoundException();

            try
            {
                Console.WriteLine($"Changing settings for {registerLaserFromDb.Name}");
                registerLaserFromDb.Name = registeredLaser.Name;

                using HttpClient httpClient = new();
                string json = JsonConvert.SerializeObject(registeredLaser);
                StringContent content = new(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync($"http://{registeredLaser.IPAddress}/settings", content);
                response.EnsureSuccessStatusCode();

                Console.WriteLine("Settings Update send successfull");
                await _registeredLaserDal.Update(registerLaserFromDb);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the status from all lasers in the database that are online
        /// This function is called every 30 seconds in Startup.cs to keep the list updated
        /// </summary>
        /// <returns>The results of all the lasers in the connected lasers list</returns>
        public static async Task<List<RegisteredLaserDto>> GetStatus()
        {
            int length = LaserConnectionLogicState.RegisteredLasers.Count;
            if (length == 0)
            {
                return LaserConnectionLogicState.RegisteredLasers;
            }

            using HttpClient httpClient = new();
            for (int i = 0; i < length; i++)
            {
                RegisteredLaserDto connectedLaser = LaserConnectionLogicState.RegisteredLasers[i];
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"{connectedLaser.IPAddress}/telemetry");
                    LaserTelemetry laserTelemetry = await response.Content.ReadFromJsonAsync<LaserTelemetry>();
                }
                catch (HttpRequestException) {}
            }

            httpClient.Dispose();
            return LaserConnectionLogicState.RegisteredLasers;
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

            foreach (RegisteredLaserDto connectedLaser in LaserConnectionLogicState.RegisteredLasers.FindAll(cl =>
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
            LaserConnectionLogicState.RegisteredLasers.RemoveAll(cl => cl.Uuid == uuid);
        }
    }
}
