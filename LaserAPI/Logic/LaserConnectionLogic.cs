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
        private static readonly bool _ranByUnitTest;
        public static int LastXPosition { get; private set; }
        public static int LastYPosition { get; private set; }
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
            if (_ranByUnitTest)
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

                var utf8 = new UTF8Encoding();
                byte[] msg = utf8.GetBytes(json);

                await _stream.WriteAsync(msg, 0, msg.Length);

                LastXPosition = message.X;
                LastYPosition = message.Y;
            }
            catch (Exception)
            {
                _client.Close();
                _server.Stop();
                Connect();
            }
        }
    }
}
