using LaserAPI.Interfaces.Helper;
using LaserAPI.Models.Helper.Laser;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserConnectionLogic : ILaserConnectionLogic
    {
        private Socket _socket;
        private int _lastXPosition;
        private int _lastYPosition;

        public LaserConnectionLogic()
        {
            Task.Run(async () => await Connect()).Wait();
        }

        public async Task Connect()
        {
            IPAddress address = IPAddress.Parse("192.168.1.177");
            IPEndPoint remoteEp = new(address, 80);

            _socket = new Socket(address.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            await _socket.ConnectAsync(remoteEp);
        }

        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void SendMessage(LaserMessage message)
        {
            try
            {
                int[] value = { message.RedLaser, message.GreenLaser, message.BlueLaser, message.X, message.Y };
                string result = string.Join(",", value);
                string json = "{d:[" + result + "]}";

                byte[] msg = Encoding.ASCII.GetBytes(json);
                _socket.Send(msg);
                _lastXPosition = message.X;
                _lastYPosition = message.Y;
            }
            catch (Exception)
            {
                Task.Run(async () => await Connect()).Wait();
            }
        }

        public int GetLastXPosition()
        {
            return _lastXPosition;
        }

        public int GetLastYPosition()
        {
            return _lastYPosition;
        }
    }
}
