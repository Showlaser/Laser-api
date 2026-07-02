using LaserAPI.Enums;
using LaserAPI.Interfaces;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.Patterns;
using LaserAPI.Models.Dto.RegisteredLaser;
using LaserAPI.Models.FromFrontend.Laser;
using LaserAPI.Models.FromFrontend.Points;
using LaserAPI.Models.FromFrontend.Showlaser;
using LaserAPI.Models.FromFrontend.Showlaser.SDCard;
using LaserAPI.Models.FromLaser;
using LaserAPI.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
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
            var registeredShowlasers = await _registeredLaserDal.All();
            int registeredShowlasersCount = registeredShowlasers.Count;
            for (int i = 0; i < registeredShowlasersCount; i++)
            {
                registeredShowlasers[i].Status = LaserStatus.ConnectionLost;
            }

            LaserConnectionLogicState.RegisteredLasers = registeredShowlasers;
        }

        private static bool RequestsCanBeSendToLaser(RegisteredLaserDto registeredLaser)
        {
            int length = LaserConnectionLogicState.RegisteredLasers.Count;
            if (length > 0 && LaserConnectionLogicState.RegisteredLasers.Exists(rl => rl.Uuid == registeredLaser.Uuid))
            {
                RegisteredLaserDto laser = LaserConnectionLogicState.RegisteredLasers.Find(l => l.Uuid == registeredLaser.Uuid);
                if (laser.Status == LaserStatus.Standby || laser.Status == LaserStatus.Emitting || laser.Status == LaserStatus.EmergencyButtonPressed)
                {
                    return true;
                }
            }

            return false;
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

        public async Task UpdateShowlaser(RegisteredLaserDto registeredLaser)
        {
            if (!RequestsCanBeSendToLaser(registeredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

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

                Console.WriteLine("Settings Update send successful");
                await _registeredLaserDal.Update(registerLaserFromDb);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(RegisteredLaserDto registeredLaser)
        {
            if (!RequestsCanBeSendToLaser(registeredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

            bool valid = ShowlaserValid(registeredLaser);
            if (!valid)
            {
                throw new InvalidDataException();
            }

            int index = LaserConnectionLogicState.RegisteredLasers.FindIndex(rl => rl.Uuid == registeredLaser.Uuid);
            LaserConnectionLogicState.RegisteredLasers[index] = registeredLaser;
            await _registeredLaserDal.Update(registeredLaser);
        }

        public static async Task AliveCheck()
        {
            int length = LaserConnectionLogicState.RegisteredLasers.Count;
            if (length == 0)
            {
                return;
            }

            using HttpClient httpClient = new();
            for (int i = 0; i < length; i++)
            {
                RegisteredLaserDto registeredLaser = LaserConnectionLogicState.RegisteredLasers[i];
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync($"http://{registeredLaser.IPAddress}/alive");
                    LaserConnectionLogicState.RegisteredLasers[i].Status = LaserStatus.Standby;
                }
                catch (HttpRequestException) 
                {
                    if (registeredLaser.Status != LaserStatus.ConnectionLost)
                    {
                        Console.WriteLine($"Connection lost for laser {registeredLaser.Name}");
                    }

                    LaserConnectionLogicState.RegisteredLasers[i].Status = LaserStatus.ConnectionLost;
                }
            }

            httpClient.Dispose();
        }

        public async Task<List<SDCardJsonFileWrapper>> GetSDCardFiles(RegisteredLaserDto registeredLaser)
        {
            if (!RequestsCanBeSendToLaser(registeredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

            using HttpClient httpClient = new();
            {
                try
                {
                    Console.WriteLine($"Get SDCard files from showlaser: {registeredLaser.Name}");
                    HttpResponseMessage response = await httpClient.GetAsync($"http://{registeredLaser.IPAddress}/sd-card");
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<SDCardJsonFileWrapper>>(json);
                }
                catch (HttpRequestException)
                {
                    if (registeredLaser.Status != LaserStatus.ConnectionLost)
                    {
                        Console.WriteLine($"Connection lost for laser {registeredLaser.Name}");
                        return [];
                    }
                }
            }

            httpClient.Dispose();
            return [];
        }

        // Shows are stored on the SD card as ".lzs" now. Normalise whatever name the
        // frontend supplied (no extension, or a legacy ".json") to a ".lzs" name.
        private static string ToLzsFilename(string filename)
        {
            string name = string.IsNullOrWhiteSpace(filename) ? "show" : filename.Trim();
            if (name.EndsWith(".lzs", StringComparison.OrdinalIgnoreCase))
            {
                return name;
            }
            if (name.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                name = name[..^5];
            }
            return name + ".lzs";
        }

        public async Task SaveSDCardFile(SDCardJsonFileWrapper fileWrapper)
        {
            if (!RequestsCanBeSendToLaser(fileWrapper.RegisteredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

            // Convert the show JSON to the compact .lzs binary here so the showlaser
            // never deserializes a multi-KB document in RAM: it streams the bytes we
            // send straight to its SD card. The filename goes in a header so the body
            // is the raw show and nothing needs parsing on the laser.
            byte[] lzs = LasershowBinaryConverter.JsonToBinary(fileWrapper.FileJson);
            string filename = ToLzsFilename(fileWrapper.Filename);

            using HttpClient httpClient = new();
            {
                try
                {
                    Console.WriteLine($"Save SDCard file on showlaser: {fileWrapper.RegisteredLaser.Name}");

                    using ByteArrayContent content = new(lzs);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    content.Headers.Add("X-Filename", filename);

                    HttpResponseMessage response = await httpClient.PostAsync(
                        $"http://{fileWrapper.RegisteredLaser.IPAddress}/sd-card-binary", content);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(json);
                    if (!apiResponse.Success)
                    {
                        throw new JsonException($"Error saving SDCard file: {apiResponse.Error}");
                    }
                }
                catch (HttpRequestException)
                {
                    if (fileWrapper.RegisteredLaser.Status != LaserStatus.ConnectionLost)
                    {
                        Console.WriteLine($"Connection lost for laser {fileWrapper.RegisteredLaser.Name}");
                    }
                }
            }

            httpClient.Dispose();
        }

        /// <summary>
        /// Plays a lasershow on the showlaser. Lasershows are stored on the SD card and
        /// streamed from there, so the show is uploaded first only if it is not already
        /// present, then playback is triggered by filename.
        /// </summary>
        public async Task PlayLasershow(SDCardJsonFileWrapper fileWrapper)
        {
            if (!RequestsCanBeSendToLaser(fileWrapper.RegisteredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

            string filename = ToLzsFilename(fileWrapper.Filename);

            List<SDCardJsonFileWrapper> filesOnSdCard = await GetSDCardFiles(fileWrapper.RegisteredLaser);
            bool alreadyOnSdCard = filesOnSdCard.Exists(f =>
                string.Equals(f.Filename, filename, StringComparison.OrdinalIgnoreCase));
            if (!alreadyOnSdCard)
            {
                await SaveSDCardFile(fileWrapper);
            }

            await SendPlaySDCardFileRequest(fileWrapper.RegisteredLaser, filename);
        }

        private static async Task SendPlaySDCardFileRequest(RegisteredLaserDto registeredLaser, string filename)
        {
            using HttpClient httpClient = new();
            {
                try
                {
                    Console.WriteLine($"Play SDCard file {filename} on showlaser: {registeredLaser.Name}");

                    string body = JsonConvert.SerializeObject(new { filename });
                    StringContent content = new(body, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(
                        $"http://{registeredLaser.IPAddress}/sd-card-play", content);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(json);
                    if (!apiResponse.Success)
                    {
                        throw new JsonException($"Error playing SDCard file: {apiResponse.Error}");
                    }
                }
                catch (HttpRequestException)
                {
                    if (registeredLaser.Status != LaserStatus.ConnectionLost)
                    {
                        Console.WriteLine($"Connection lost for laser {registeredLaser.Name}");
                    }
                }
            }

            httpClient.Dispose();
        }

        /// <summary>
        /// Plays a pattern or animation live on the showlaser. These are NOT stored on the
        /// laser: the show JSON is converted to the compact .lzs binary and streamed into
        /// the laser's RAM, where it is looped. Uses the same converter as a lasershow (a
        /// pattern is a single-frame show, an animation a multi-frame show).
        /// </summary>
        public async Task PlayLiveShow(SDCardJsonFileWrapper fileWrapper)
        {
            if (!RequestsCanBeSendToLaser(fileWrapper.RegisteredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

            byte[] lzs = LasershowBinaryConverter.JsonToBinary(fileWrapper.FileJson);

            using HttpClient httpClient = new();
            {
                try
                {
                    Console.WriteLine($"Project live show on showlaser: {fileWrapper.RegisteredLaser.Name}");

                    using ByteArrayContent content = new(lzs);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    HttpResponseMessage response = await httpClient.PostAsync(
                        $"http://{fileWrapper.RegisteredLaser.IPAddress}/live-binary", content);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<APIResponse>(json);
                    if (!apiResponse.Success)
                    {
                        throw new JsonException($"Error projecting live show: {apiResponse.Error}");
                    }
                }
                catch (HttpRequestException)
                {
                    if (fileWrapper.RegisteredLaser.Status != LaserStatus.ConnectionLost)
                    {
                        Console.WriteLine($"Connection lost for laser {fileWrapper.RegisteredLaser.Name}");
                    }
                }
            }

            httpClient.Dispose();
        }

        /// <summary>
        /// Stops whatever the showlaser is currently playing (an SD lasershow or a live
        /// pattern/animation) and blanks the laser.
        /// </summary>
        public async Task StopPlayback(RegisteredLaserDto registeredLaser)
        {
            if (!RequestsCanBeSendToLaser(registeredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

            using HttpClient httpClient = new();
            {
                try
                {
                    Console.WriteLine($"Stop playback on showlaser: {registeredLaser.Name}");
                    HttpResponseMessage response = await httpClient.PostAsync(
                        $"http://{registeredLaser.IPAddress}/stop", null);
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException)
                {
                    if (registeredLaser.Status != LaserStatus.ConnectionLost)
                    {
                        Console.WriteLine($"Connection lost for laser {registeredLaser.Name}");
                    }
                }
            }

            httpClient.Dispose();
        }

        public async Task DeleteSDCardFile(RegisteredLaserDto registeredLaser, SDCardJsonFileWrapper file)
        {
            if (!RequestsCanBeSendToLaser(registeredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

            using HttpClient httpClient = new();
            {
                try
                {
                    Console.WriteLine($"Delete SDCard file {file.Filename} from showlaser: {registeredLaser.Name}");
                    HttpResponseMessage response = await httpClient.PutAsJsonAsync($"http://{registeredLaser.IPAddress}/sd-card", file);
                    string json = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException)
                {
                    if (registeredLaser.Status != LaserStatus.ConnectionLost)
                    {
                        Console.WriteLine($"Connection lost for laser {registeredLaser.Name}");
                        throw new TimeoutException();
                    }
                }
            }

            httpClient.Dispose();
            return;
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

        public async Task ProjectSafetyZone(ProjectSafetyZoneWrapper projectSafetyZoneWrapper)
        {
            if (!RequestsCanBeSendToLaser(projectSafetyZoneWrapper.RegisteredLaser))
            {
                throw new InvalidOperationException("Request cannot be send to showlaser");
            }

            using HttpClient httpClient = new();
            {
                try
                {
                    Console.WriteLine($"Project safety zone on showlaser {projectSafetyZoneWrapper.RegisteredLaser.Name}");
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync($"http://{projectSafetyZoneWrapper.RegisteredLaser.IPAddress}/safety-zone", projectSafetyZoneWrapper);
                    string json = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException)
                {
                    if (projectSafetyZoneWrapper.RegisteredLaser.Status != LaserStatus.ConnectionLost)
                    {
                        Console.WriteLine($"Connection lost for laser {projectSafetyZoneWrapper.RegisteredLaser.Name}");
                        throw new TimeoutException();
                    }
                }
            }

            httpClient.Dispose();
            return;
        }
    }
}
