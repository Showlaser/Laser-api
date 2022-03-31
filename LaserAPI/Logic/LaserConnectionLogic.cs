using LaserAPI.Models.Helper.Laser;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public static class LaserConnectionLogic
    {
        public static bool RanByUnitTest { get; set; } = false;
        public static LaserMessage PreviousLaserMessage { get; set; } = new();
        private static TcpListener _server;
        private static NetworkStream _stream;
        private static TcpClient _client;

        public static void Connect()
        {
            try
            {
                IPAddress localAddress = IPAddress.Parse("192.168.1.31");
                _server = new TcpListener(localAddress, 50000)
                {
                    Server =
                    {
                        SendTimeout = -1
                    }
                };
                _server.Start();

                Console.WriteLine("Waiting");
                _client = _server.AcceptTcpClient();
                Console.WriteLine("Connected");
                _stream = _client.GetStream();
            }
            catch (Exception)
            {
                _client.Close();
            }
        }

        public static async Task SendMessage(LaserMessage message)
        {
            LaserSafetyHelper.LimitLaserPowerPerLaserIfNecessary(ref message, 8);
            if (RanByUnitTest)
            {
                return;
            }

            if (_client == null || !_client.Connected)
            {
                Connect();
            }

            try
            {
                int[] value = { message.RedLaser, message.GreenLaser, message.BlueLaser, message.X, message.Y };
                string result = string.Join(",", value);
                string json = @"{""d"":[" + result + "]}";

                UTF8Encoding utf8 = new();

                byte[] msg = utf8.GetBytes(json);
                await _stream.WriteAsync(msg);

                byte[] bytes = new byte[msg.Length];
                await _stream.ReadAsync(bytes);

                PreviousLaserMessage = message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _client.Close();
                _server.Stop();
                Connect();
            }
        }
    }
}
