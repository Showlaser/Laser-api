using LaserAPI.Interfaces;
using LaserAPI.Models.FromFrontend.Laser;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserConnectionLogic : ILaserConnectionLogic
    {
        private readonly List<ConnectedLaser> _connectedLasers = [];

        public async Task Connect(string IPAddress, string laserId, string password)
        {
            ConnectedLaser connectedLaserFromList = _connectedLasers.FirstOrDefault(cl => cl.IPAddress == IPAddress);
            if (connectedLaserFromList == null)
            {
                _connectedLasers.Remove(connectedLaserFromList);
            }

            HttpClient httpClient = new()
            {
                BaseAddress = new Uri(IPAddress)
            };

            HttpResponseMessage response = await httpClient.GetAsync("connect");
            response.EnsureSuccessStatusCode();

            LaserTelemetry laserTelemetry = await response.Content.ReadFromJsonAsync<LaserTelemetry>();
            ConnectedLaser connectedLaser = new()
            {
                LaserConnectionId = laserId,
                Name = laserTelemetry.Name,
                ModelType = laserTelemetry.ModelType,
                Status = laserTelemetry.Status,
                Online = true,
                Password = password,
            };

            _connectedLasers.Add(connectedLaser);
        }

        public void Disconnect(string laserId)
        {
            ConnectedLaser connectedLaser = _connectedLasers.FirstOrDefault(cl => cl.LaserConnectionId == laserId);
            if (connectedLaser != null)
            {
                _connectedLasers.Remove(connectedLaser);
            }
        }

        public async Task<List<ConnectedLaser>> GetStatus()
        {
            using HttpClient httpClient = new();

            int length = _connectedLasers.Count;
            for (int i = 0; i < length; i++)
            {
                ConnectedLaser connectedLaser = _connectedLasers[i];

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"{connectedLaser.IPAddress}/telemetry?password={connectedLaser.Password}");
                    LaserTelemetry laserTelemetry = await response.Content.ReadFromJsonAsync<LaserTelemetry>();
                    _connectedLasers[i].Status = laserTelemetry.Status;
                    _connectedLasers[i].Online = true;
                }
                catch (HttpRequestException)
                {
                    _connectedLasers[i].Online = false;
                    _connectedLasers[i].Status = Enums.LaserStatus.ConnectionLost;
                }
            }

            httpClient.Dispose();
            return _connectedLasers;
        }

        public async Task SendData(List<PointWrapper> wrappedPoints)
        {
            using HttpClient httpClient = new();
            List<Task> tasks = [];

            foreach (ConnectedLaser connectedLaser in _connectedLasers.FindAll(cl => cl.Online))
            {
                string url = $"{connectedLaser.IPAddress}/send?password={connectedLaser.Password}";
                List<PointWrapper> wrappedPointToPlayOnLaser = wrappedPoints.FindAll(wp => wp.LaserToProjectOnUuid == connectedLaser.Uuid);

                tasks.Add(httpClient.PostAsJsonAsync(url, wrappedPointToPlayOnLaser));
            }

            await Task.WhenAll(tasks);
            httpClient.Dispose();
        }
    }
}
